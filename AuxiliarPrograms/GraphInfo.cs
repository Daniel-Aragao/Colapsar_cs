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
            // if (args.Length < 2)
            // {
            //    throw new ArgumentException("You must inform o");
            // }
            
            Graph graph = null;
            var paths = Directory.GetFiles(Constants.PATH_GRAPH);
            // var paths = new string [] { args[1] };//Directory.GetFiles(Constants.PATH_GRAPH);
            Array.Reverse(paths);

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            
            foreach(var path in paths)
            {
                ILogger logger = LoggerFactory.Define(false, "GraphInfo-" + path);
                // logger.WriteLine(path);
                
                graph = Import.LoadCityFromText(path);
                // graph = Import.LoadCityFromText(Constants.PATH_GRAPH + path + Constants.FILE_EXTENSION_OUTPUT);

                // logger.WriteLine("graph loaded " + graph.Name);
                
                double hubDegree = graph.Hub.Degree; //logger.WriteLine($"hub degree {hubDegree}");
                double cc = graph.GetClusteringCoefficient(); //logger.WriteLine($"cluster coefficient {cc}");
                double entropy = graph.Entropy(); //logger.WriteLine($"entropy {entropy}");
                double density = graph.Density(); //logger.WriteLine($"entropy {entropy}");
                // double avgPathLength = graph.AveragePathLenght(); logger.WriteLine($"avgPathLength {avgPathLength}");
                
                // logger.WriteLine($"{{ graph: {path}, entropy: {entropy}, avgPathLength: {avgPathLength}, hubDegree: {hubDegree}, clusterCoefficient: {cc} }}");
                logger.WriteLine($"{{ graph: {path}, entropy: {entropy}, hubDegree: {hubDegree}, clusterCoefficient: {cc}, density: {density} }}");
                // logger.WriteLine();
            }
        }
    }
}

/*
{ graph: ./graphs/Toquio-network-osm-2018-1_1.txt, entropy: 1.24523023500709, hubDegree: 10, clusterCoefficient: 0.012354917926845 }
{ graph: ./graphs/SmallWorldGraph_100_100_0.3_2019_11_6_23_54_17_1.txt, entropy: 2.52797768394538, hubDegree: 20, clusterCoefficient: 0.0274925856412167 }
{ graph: ./graphs/SmallWorldGraph_100_100_0.03_2019_11_6_23_54_41_1.txt, entropy: 1.07995371981321, hubDegree: 14, clusterCoefficient: 0 }
{ graph: ./graphs/ScaleFreeGraph_10000_2019_11_6_23_55_3_1.txt, entropy: 1.47783590277275, hubDegree: 260, clusterCoefficient: 0.7654 }
{ graph: ./graphs/RegularGraph_100_100_2019_11_6_23_48_53_1.txt, entropy: 0.243681106818123, hubDegree: 8, clusterCoefficient: 0 }
{ graph: ./graphs/RandomGraph_100_100_2019_11_6_23_53_14_1.txt, entropy: 2.92030905896945, hubDegree: 28, clusterCoefficient: 0.0732545753991833 }
{ graph: ./graphs/Paris-network-osm-2019-1_1.txt, entropy: 1.74325672768858, hubDegree: 16, clusterCoefficient: 0.0310099573257468 }
{ graph: ./graphs/NY-network-osm-2018-1_1.txt, entropy: 2.1423270024202, hubDegree: 12, clusterCoefficient: 0.0272517296034556 }
{ graph: ./graphs/Mumbai-network-osm-2019-1_2.txt, entropy: 1.63147687297832, hubDegree: 12, clusterCoefficient: 0.0350501653954455 }
{ graph: ./graphs/Fortaleza-network-osm-2019-1_1.txt, entropy: 2.10530891171572, hubDegree: 11, clusterCoefficient: 0.0576580112294399 }
*/