using Core;
using Core.models;


namespace Infra.services.regions
{
    public class BruteForceFactory: SearchStrategyFactory
    {
        public BruteForceFactory() : base(Constants.ALGORITHMN_NAME_BRUTE_FORCE)
        {
            
        }

        public override SearchStrategy GetStrategy(Graph g)
        {
            return new BruteForce(g);
        }
    }
}