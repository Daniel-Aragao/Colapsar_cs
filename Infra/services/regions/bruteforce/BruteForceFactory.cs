using Core.models;


namespace Infra.services.regions
{
    public class BruteForceFactory: SearchStrategyFactory
    {
        public override SearchStrategy GetStrategy(Graph g)
        {
            return new BruteForce(g);
        }
    }
}