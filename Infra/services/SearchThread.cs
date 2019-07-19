using Core.models;


namespace Infra.services
{
    public abstract class SearchThread
    {
        protected Graph Graph {get;}

        public SearchThread(Graph graph)
        {
            this.Graph = graph;
        }

        public abstract PathRoute Search(Node source, Node target, double radius);
    }
}