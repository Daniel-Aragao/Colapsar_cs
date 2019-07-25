using Core.models;


namespace Infra.services.regions
{
    public class BruteForceFactory: SearchStrategyFactory
    {
        public BruteForceFactory() : base("BruteForce")
        {
            
        }

        public override SearchStrategy GetStrategy(Graph g)
        {
            return new BruteForce(g);
        }
    }
}