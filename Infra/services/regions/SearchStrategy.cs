using Core.models;


namespace Infra.services.regions
{
    public abstract class SearchStrategy
    {
        public string Name {get; private set;}
        protected Graph Graph {get;}

        public SearchStrategy(Graph graph, string name)
        {
            this.Graph = graph;
            this.Name = name;
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