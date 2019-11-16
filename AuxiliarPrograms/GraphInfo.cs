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
            
            foreach(var path in Directory.GetFiles(Constants.PATH_GRAPH))
            {
                Console.WriteLine(path);
                
                graph = Import.LoadCityFromText(path);

                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                
                var entropy = graph.Entropy();
                var cc = graph.GetClusteringCoefficient();
                var avgPathLength = graph.AveragePathLenght();
                var hubDegree = graph.Hub.Degree;
                
                Console.WriteLine("{ graph: {0}, entropy: {1}, avgPathLength: {2}, hubDegree: {3}, clusterCoefficient: {4} }", path, entropy, avgPathLength, hubDegree, cc);
            }

            // Console.WriteLine("Resultados negativos simbolizam que os valores para os resultados da direita são maiores");
            // Console.WriteLine("Resultados iguais       : " + countRight.ToString());
            // Console.WriteLine("Resultados diferentes   : " + (differentRoutesDistance.Count + differentRoutesStatus.Count).ToString());
            // Console.WriteLine("\tStatus diferentes     : " + differentRoutesStatus.Count.ToString());
            // Console.WriteLine("\tDistâncias diferentes : " + differentRoutesDistance.Count.ToString());
            // Console.WriteLine("Diferença de distância  : " + totalDistanceDifferenceAllPaths.ToString());
            // Console.WriteLine("\tCaminhos iguais       : " + totalDistanceDifferenceForEqualPaths.ToString());
            // Console.WriteLine("\tCaminhos diferentes   : " + totalDistanceDifferenceForDifferentPaths.ToString());
            // Console.WriteLine("Diferença de tempo      : " + totalTimeDifferenceAllPaths.ToString());
            // Console.WriteLine("\tCaminhos iguais       : " + totalTimeDifferenceForEqualPaths.ToString());
            // Console.WriteLine("\tCaminhos diferentes   : " + totalTimeDifferenceForDifferentPaths.ToString());
            
        }
    }
}
