using Core.models;


namespace Infra.services.regions
{
    public abstract class SearchStrategyFactory
    {
        public abstract SearchStrategy GetStrategy(Graph g);
    }
}