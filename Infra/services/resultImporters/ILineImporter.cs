using Core.models;

public interface ILineImporter
{
    RouteMeasures GetMeasures(string[] line);
}