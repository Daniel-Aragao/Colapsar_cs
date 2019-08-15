using System;
using System.Collections.Generic;

using Core;
using Core.models;
using Infra.services;

namespace AuxiliarPrograms
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                throw new ArgumentException("You must inform the program to run");
            }

            var program = args[0];

            if (program == "compare_outputs" || program == "CO")
            {
                if (args.Length < 5)
                {
                    throw new ArgumentException("You must inform two output file names and two file tipes");
                }
                Graph graph = null;

                if(args.Length >= 6)
                {
                    graph = Import.LoadCityFromText(Constants.PATH_GRAPH + args[5]);
                }

                // var path = "";
                var path = Constants.PATH_OUTPUTS;

                ILineImporter javaImporter = new JavaResultLineImporter();
                ILineImporter csImporter = new CSResultLineImporter();

                var measuresOrigin = Import.LoadRouteMeasuresFromTxt(path + args[1], args[3] == "java" ? javaImporter  : csImporter);
                var measuresTarget = Import.LoadRouteMeasuresFromTxt(path + args[2], args[4] == "java" ? javaImporter  : csImporter);
                
                Console.WriteLine("Outputs imported");

                var countRight = 0;
                List<Tuple<RouteMeasures, RouteMeasures>> differentRoutesStatus = new List<Tuple<RouteMeasures, RouteMeasures>>();
                List<Tuple<RouteMeasures, RouteMeasures>> differentRoutesDistance = new List<Tuple<RouteMeasures, RouteMeasures>>();

                double totalTimeDifference = 0;
                double totalDistanceDifference = 0;

                foreach (var measureOrigin in measuresOrigin)
                {
                    if (measuresTarget.ContainsKey(measureOrigin.Key))
                    {
                        RouteMeasures routeMeasureOrigin = measureOrigin.Value;
                        RouteMeasures routeMeasureTarget = measuresTarget[measureOrigin.Key];
                        
                        if(routeMeasureOrigin.Status == routeMeasureTarget.Status)
                        {
                            if(Math.Abs(routeMeasureOrigin.Distance - routeMeasureTarget.Distance) < Constants.DISTANCE_DIFFERENCE_THRESHOLD)
                            {
                                countRight++;
                            }
                            else
                            {
                                differentRoutesDistance.Add(new Tuple<RouteMeasures, RouteMeasures>(routeMeasureOrigin, routeMeasureTarget));
                            }

                            totalTimeDifference += (routeMeasureOrigin.DeltaTime - routeMeasureTarget.DeltaTime).TotalMinutes;
                            totalDistanceDifference += (routeMeasureOrigin.Distance - routeMeasureTarget.Distance);
                        }
                        else
                        {
                            differentRoutesStatus.Add(new Tuple<RouteMeasures, RouteMeasures>(routeMeasureOrigin, routeMeasureTarget));
                        }
                    }
                    else
                    {
                        throw new ArgumentException("The paths don't represent the same OD file, because the pair " + 
                            measureOrigin.Key + " in the origin file couldn't be found in the target path");
                    }
                }
                GraphRunner graphRunner = null;

                Console.WriteLine();

                if(graph != null)
                {
                    graphRunner = new GraphRunner(graph);
                }
                differentRoutesDistance.ForEach(t => 
                {
                    Console.WriteLine(
                        t.Item1.SourceId + "," + t.Item1.TargetId + ": " +
                        t.Item1.Distance.ToString() + " <> " + 
                        t.Item2.Distance.ToString() + " = " + 
                        (t.Item1.Distance - t.Item2.Distance).ToString() +
                        (graphRunner != null ? 
                            "\n\t GraphRunner => " + graphRunner.PathSize(t.Item1).ToString() + " <> " + graphRunner.PathSize(t.Item2).ToString() : "-")
                        );
                        Console.WriteLine();
                });
                // 3338853805,571723594  5980,10491667755 => 5979,52324019751 => 0,581676480044734 

                Console.WriteLine();

                Console.WriteLine("Resultados iguais    : " + countRight.ToString());
                Console.WriteLine("Resultados diferentes: " + (differentRoutesDistance.Count + differentRoutesStatus.Count).ToString());
                Console.WriteLine("\tStatus diferentes: " + differentRoutesStatus.Count.ToString());
                Console.WriteLine("\tDistâncias diferentes: " + differentRoutesDistance.Count.ToString());
                Console.WriteLine("Diferença de tempo   : " + totalTimeDifference.ToString());
                Console.WriteLine("Diferença de distância   : " + totalDistanceDifference.ToString());
            }
        }
    }
}
