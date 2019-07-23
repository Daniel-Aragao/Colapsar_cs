using Core.models;


namespace Infra.services.regions
{
    public class CollapseFactory: SearchStrategyFactory
    {
        public override SearchStrategy GetStrategy(Graph g)
        {
            return new Collapse(g);
        }
    }
}