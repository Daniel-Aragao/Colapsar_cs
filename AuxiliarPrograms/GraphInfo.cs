using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;

using Core;
using Core.models;
using Infra.services;

namespace AuxiliarPrograms
{
    public static class GraphInfo
    {
        public static void GetInfo(string[] args)
        {            
            //if (args.Length < 2)
            //{
            //    throw new ArgumentException("You must inform o");
            //}
            
            Graph graph = null;
            var paths = Directory.GetFiles(Constants.PATH_GRAPH);
            Array.Reverse(paths);
            
            foreach(var path in paths)
            {
                Console.WriteLine(path);
                
                graph = Import.LoadCityFromText(path);

                Console.WriteLine("graph loaded " + graph.Name);

                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                
                double hubDegree = graph.Hub.Degree; Console.WriteLine("hub degree " + hubDegree);
                double cc = graph.GetClusteringCoefficient(); Console.WriteLine("cluster coefficient " + cc);
                double entropy = graph.Entropy(); Console.WriteLine("entropy " + entropy);
                double avgPathLength = graph.AveragePathLenght(); Console.WriteLine("avgPathLength " + avgPathLength);
                
                Console.WriteLine($"{{ graph: {path}, entropy: {entropy}, avgPathLength: {avgPathLength}, hubDegree: {hubDegree}, clusterCoefficient: {cc} }}");
            }
        }
    }
}
