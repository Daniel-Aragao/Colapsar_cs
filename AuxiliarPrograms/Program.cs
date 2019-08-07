using System;

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

                // var path = "";
                var path = Constants.PATH_OUTPUTS;

                var measuresOrigin = Import.LoadRouteMeasuresFromTxt(path + args[1], args[3] == "java" ? Constants.FIELD_ORDERING_COLAPSAR_JAVA : Constants.FIELD_ORDERING_COLAPSAR_CS);
                var measuresTarget = Import.LoadRouteMeasuresFromTxt(path + args[2], args[4] == "java" ? Constants.FIELD_ORDERING_COLAPSAR_JAVA : Constants.FIELD_ORDERING_COLAPSAR_CS);
                
                Console.WriteLine("Outputs imported");

                var countRight = 0;
                var countDifference = 0;
                TimeSpan totalTimeDifference = new TimeSpan();

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
                                totalTimeDifference = routeMeasureOrigin.DeltaTime - routeMeasureTarget.DeltaTime;
                            }
                            else
                            {
                                countDifference--;
                            }
                        }
                        else
                        {
                            countDifference--;
                        }
                    }
                    else
                    {
                        throw new ArgumentException("The paths don't represent the same OD file, because the pair " + 
                            measureOrigin.Key + " in the origin file couldn't be found in the target path");
                    }
                }

                Console.WriteLine("Resultados iguais    : " + countRight.ToString());
                Console.WriteLine("Resultados diferentes: " + countDifference.ToString());
                Console.WriteLine("Diferença de tempo   : " + totalTimeDifference.TotalMinutes.ToString());
            }
        }
    }
}
