using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;

using Core;
using Core.models;
using Infra.services;
using Infra.services.log;

namespace AuxiliarPrograms
{
    public static class GraphInfo
    {
        public static void GetInfo(string[] args)
        {            
            if (args.Length < 2)
            {
               throw new ArgumentException("You must inform o");
            }
            
            Graph graph = null;
            // var paths = Directory.GetFiles(Constants.PATH_GRAPH);
            var paths = new string [] { args[1] };//Directory.GetFiles(Constants.PATH_GRAPH);
            Array.Reverse(paths);

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            
            foreach(var path in paths)
            {
                ILogger logger = LoggerFactory.Define(true, "GraphInfo-" + path);
                logger.WriteLine(path);
                
                graph = Import.LoadCityFromText(Constants.PATH_GRAPH + path + Constants.FILE_EXTENSION_OUTPUT);

                logger.WriteLine("graph loaded " + graph.Name);
                
                double hubDegree = graph.Hub.Degree; logger.WriteLine($"hub degree {hubDegree}");
                double cc = graph.GetClusteringCoefficient(); logger.WriteLine($"cluster coefficient {cc}");
                double entropy = graph.Entropy(); logger.WriteLine($"entropy {entropy}");
                double avgPathLength = graph.AveragePathLenght(); logger.WriteLine($"avgPathLength {avgPathLength}");
                
                logger.WriteLine($"{{ graph: {path}, entropy: {entropy}, avgPathLength: {avgPathLength}, hubDegree: {hubDegree}, clusterCoefficient: {cc} }}");
                logger.WriteLine();
            }
        }
    }
}
