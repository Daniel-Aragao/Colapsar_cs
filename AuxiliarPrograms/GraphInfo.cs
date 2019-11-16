using System;
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
            if (args.Length < 2)
            {
                throw new ArgumentException("You must inform o");
            }
            
            Graph graph = null;
            
            graph = Import.LoadCityFromText(Constants.PATH_GRAPH + args[1]);

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            
            GraphRunner graphRunner = null;

            Console.WriteLine();

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
