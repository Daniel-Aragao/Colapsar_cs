using System;
using System.Globalization;
using System.Collections.Generic;
using Core;
using Core.models;
using Core.extensions;

public class JavaResultLineImporter : ILineImporter
{    
    public static Dictionary<string,int> fieldOrdering { get {return Constants.FIELD_ORDERING_COLAPSAR_JAVA;} }

    public RouteMeasures GetMeasures(string[] lineSplited)
    {
        EPathStatus status;
        string jumpsColumn = lineSplited[fieldOrdering["Jumps"]].ToLower();

        if(jumpsColumn.Contains("erro"))
        {
            byte erro = byte.Parse(jumpsColumn.Split(':')[1]);

            switch(erro)
            {
                case 1:
                    status = EPathStatus.SourceOrTargetDoNotExist;
                    break;
                case 2:
                    status = EPathStatus.SourceAndTargetAreToCloseToCollapse;
                    break;
                case 3:
                    status = EPathStatus.NotFound;
                    break;
                case 4:
                case 5:
                case 6:
                    status = EPathStatus.FailOnRouteBuilding;
                    break;

                default:
                    status = EPathStatus.UnexpectedException;
                    break;
            }
        }
        else
        {
            status = EPathStatus.Found;
        }
        
        var routeMeasures = new RouteMeasures(status);

        routeMeasures.SourceId = long.Parse(lineSplited[fieldOrdering["SourceId"]]);
        routeMeasures.TargetId = long.Parse(lineSplited[fieldOrdering["TargetId"]]);

        if(status == EPathStatus.Found)
        {
            routeMeasures.Jumps = Int32.Parse(lineSplited[fieldOrdering["Jumps"]]);
            
            if(!lineSplited[fieldOrdering["QuantityOfExpansions"]].IsNullOrWhiteSpace())
            {
                // dotnet run CO Fortaleza-network-osm-2019-1_1-BruteForce-10000-50-8.txt BruteForce-10000-50.0-Fortaleza-network-osm-2018-1_1.txt cs java
                routeMeasures.QuantityOfExpansions = Int32.Parse(lineSplited[fieldOrdering["QuantityOfExpansions"]], 0); 
            }
            
            if(!lineSplited[fieldOrdering["DeltaTime"]].IsNullOrWhiteSpace())
            {
                routeMeasures.DeltaTime = TimeSpan.FromMinutes(double.Parse(lineSplited[fieldOrdering["DeltaTime"]], CultureInfo.InvariantCulture));
            }

            routeMeasures.Distance = double.Parse(lineSplited[fieldOrdering["Distance"]], CultureInfo.InvariantCulture);
        }

        return routeMeasures;
    }
}