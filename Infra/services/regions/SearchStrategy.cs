using Core.models;


namespace Infra.services.regions
{
    public abstract class SearchStrategy
    {
        protected Graph Graph {get;}

        public SearchStrategy(Graph graph)
        {
            this.Graph = graph;
        }

        public abstract PathRoute Search(Node source, Node target, double radius);

        protected virtual PathRoute SearchParametersEvaluation(Node source, Node target, double radius)
        {
            if (source == null || target == null || !this.Graph.ExistNode(source.Id) || !this.Graph.ExistNode(target.Id))
            {
                return new PathRoute(EPathStatus.SourceOrTargetDoNotExist);
            }
            else if (source == target)
            {
                return new PathRoute(EPathStatus.SourceAndTargetAreEqual);
            }

            return null;
        }
    }
}