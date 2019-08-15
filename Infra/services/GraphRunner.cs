using System;
using Core.models;

namespace Infra.services
{
    public class GraphRunner
    {
        private Graph _graph;

        public GraphRunner(Graph graph)
        {
            this._graph = graph;
        }

        public double PathSize(RouteMeasures route)
        {
            double size = 0;

            if(route.Status != EPathStatus.Found)
            {
                return -1;
            }

            var nodes = route.NodeIDs;
            Node current = this._graph.GetNodeById(long.Parse(nodes[0]));

            for(var i = 1; i < nodes.Length; i++)
            {
                long nextNodeId = long.Parse(nodes[i]);
                var nextNode = this._graph.GetNodeById(nextNodeId);

                PathRoute pathRoute = Node.ShortestPathBetweenNeihbors(current, nextNode);

                if(pathRoute.Status == EPathStatus.Found)
                {
                    current = nextNode;
                    size += pathRoute.Distance;
                }
                else
                {
                    return -1;
                }
            }

            return size;
        }

        public bool ExistPath(RouteMeasures route)
        {
            return this.PathSize(route) >= 0;
        }
    }
}