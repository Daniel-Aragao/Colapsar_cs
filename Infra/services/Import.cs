using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;

using Newtonsoft.Json;

using Core;
using Core.models;
using Core.extensions;

namespace Infra.services
{
    public class Import
    {
        public static Graph LoadCityFromText(string path, char separator=',')
        {
            if(!File.Exists(path))
            {
                Console.WriteLine("------------ Current directory: " + Directory.GetCurrentDirectory() + "------------");
                
                throw new ArgumentException("The path informed do not exist " + path);
            }
            
            Graph graph = new Graph(Path.GetFileName(path));

            using(StreamReader sr = File.OpenText(path))
            {
                string line ;
                string mode = null;
                int lineCount = 1;

                while((line = sr.ReadLine()) != null)
                {
                    if(line.Equals("nodes") || line.Equals("edges"))
                    {
                        mode = line;
                        lineCount++;
                        continue;
                    }
                    var lineSplited = line.Split(separator);

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
                        var node = graph.CreateNode(long.Parse(identityData[0], CultureInfo.InvariantCulture), identityData[1], Double.Parse(identityData[2], CultureInfo.InvariantCulture));

                        var x = 0D;
                        var y = 0D;

                        Func<Position, Position,double> distanceFunction = null;
                        Dictionary<string, double> distanceMap = null;

                        if(properties.ContainsKey("x") && properties.ContainsKey("y"))
                        {
                            x = Double.Parse(properties["x"], CultureInfo.InvariantCulture);
                            y = Double.Parse(properties["y"], CultureInfo.InvariantCulture);
                            properties.Remove("x");
                            properties.Remove("y");

                            distanceFunction = Position.EucledeanDistance;
                        }
                        else if(properties.ContainsKey("latitude") && properties.ContainsKey("longitude"))
                        {
                            x = Double.Parse(properties["longitude"], CultureInfo.InvariantCulture);
                            y = Double.Parse(properties["latitude"], CultureInfo.InvariantCulture);
                            properties.Remove("longitude");
                            properties.Remove("latitude");

                            distanceFunction = Position.GeoCoordinateDistance;
                        }else if(properties.ContainsKey("distance-map"))
                        {
                            distanceFunction = Position.Mapped;
                            distanceMap = JsonConvert.DeserializeObject<Dictionary<string, double>>(properties["distance-map"]);
                            properties.Remove("distance-map");
                        }

                        var position = new Position(x, y, distanceFunction);

                        if(distanceFunction == Position.Mapped)
                        {
                            position.DistanceMap = distanceMap;
                            position.MapId = node.Id.ToString();
                        }

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
                            weight = double.Parse(properties["distance"], CultureInfo.InvariantCulture);
                            properties.Remove("distance");
                        }else
                        {
                            weight = double.Parse(identityData[1], CultureInfo.InvariantCulture);
                        }
                        
                        var edge = graph.CreateEdge(long.Parse(identityData[0], CultureInfo.InvariantCulture), long.Parse(identityData[2], CultureInfo.InvariantCulture), weight);

                        if(properties.ContainsKey("label"))
                        {
                            edge.Label = properties["label"];
                            properties.Remove("label");
                        }
                        else if(properties.ContainsKey("name-street"))
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

        public static List<Tuple<long,long>> LoadODsFromTxt(string path, int ODSize)
        {
            List<Tuple<long,long>> returnList = new List<Tuple<long,long>>();

            using(StreamReader sr = File.OpenText(path))
            {
                string line;

                while((line = sr.ReadLine()) != null)
                {
                    var lineSplited = line.Split(Constants.SEPARATOR_ODs);
                    var tuple = new Tuple<long,long>(long.Parse(lineSplited[0]), long.Parse(lineSplited[1]));
                    returnList.Add(tuple);
                }
            }

            return returnList;
        }
    }
}

