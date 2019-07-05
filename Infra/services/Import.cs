using System;
using System.IO;
using System.Collections.Generic;
using Core.models;
using Core.extensions;

namespace Infra.services
{
    public class Import
    {
        public static Graph LoadCityFromText(string path)
        {
            if(!File.Exists(path))
            {
                throw new ArgumentException("The path informed do not exist" + path);
            }
            
            Graph graph = new Graph(Path.GetFileName(path));

            using(StreamReader sr = File.OpenText(path))
            {
                string line;
                string mode = null;
                int lineCount = 0;

                while((line = sr.ReadLine()) != null)
                {
                    if(line.Equals("nodes") || line.Equals("edges"))
                    {
                        mode = line;
                        lineCount++;
                        continue;
                    }
                    var lineSplited = line.Split(',');

                    if(lineSplited.Length % 2 != 1)
                    {
                        throw new FileLoadException("The file don't have the correct format"
                            + " on the line (" + lineCount + "): " + line 
                            + " (lines should have id,label,weight,key,value,key,value..." 
                            + " with pairs of key and value optional)");
                    }

                    var identityData = lineSplited.SubArray(0, 3);
                    Dictionary<string, string> properties = new Dictionary<string, string>();

                    for(var i = 3; i < lineSplited.Length; i+=2)                   
                    {
                        properties.Add(lineSplited[i], lineSplited[i + 1]);
                    }

                    if(mode.Equals("nodes"))
                    {
                        var node = graph.CreateNode(Int32.Parse(identityData[0]), identityData[1], Double.Parse(identityData[2]));

                        var x = 0D;
                        var y = 0D;

                        Func<Position, Position,double> distanceFunction = null;

                        if(properties.ContainsKey("x") && properties.ContainsKey("y"))
                        {
                            x = Double.Parse(properties["x"]);
                            y = Double.Parse(properties["y"]);
                            properties.Remove("x");
                            properties.Remove("y");

                            distanceFunction = Position.EucledeanDistance;
                        }
                        else if(properties.ContainsKey("latitude") && properties.ContainsKey("longitude"))
                        {
                            x = Double.Parse(properties["longitude"]);
                            y = Double.Parse(properties["latitude"]);
                            properties.Remove("longitude");
                            properties.Remove("latitude");

                            distanceFunction = Position.GeoCoordinateDistance;
                        }

                        var position = new Position(x, y, distanceFunction);
                        node.Position = position;

                        foreach(var property in properties)
                        {
                            node.PutAttribute(property.Key, property.Value);
                        }

                    }else if(mode.Equals("edges"))
                    {
                        double weight = 0;

                        if(properties.ContainsKey("distance"))
                        {
                            weight = Double.Parse(properties["distance"]);
                            properties.Remove("distance");
                        }else
                        {
                            weight = Double.Parse(identityData[1]);
                        }
                        
                        var edge = graph.CreateEdge(Int32.Parse(identityData[0]), Int32.Parse(identityData[2]), weight);

                        if(properties.ContainsKey("name-street"))
                        {
                            edge.Label = properties["name-street"];
                            properties.Remove("name-street");
                        }

                        foreach(var property in properties)
                        {
                            edge.PutAttribute(property.Key, property.Value);
                        }
                    }

                    lineCount++;
                }
            }

            return graph;
        }
    }
}

