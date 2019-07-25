using Core;
using Core.models;


namespace Infra.services.regions
{
    public class CollapseFactory: SearchStrategyFactory
    {
        public CollapseFactory() : base(Constants.ALGORITHMN_NAME_COLLAPSE)
        {
            
        }
        
        public override @string GetStrategy(Graph g)
        {
            return new Collapse(g);
        }
    }
}