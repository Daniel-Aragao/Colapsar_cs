using System;
using System.Collections.Generic;
using System.Linq;
using Core.models;

namespace Infra.services
{
    public class CollapseSearch
    {
        public Graph Graph {get;}

        public CollapseSearch(Graph graph)
        {
            this.Graph = graph;
        }

        public PathRoute Search(Node source, Node target, double radius)
        {
            

            var superSource = Collapse.collapse(this.Graph, source, radius, -1, 0);
            var superTarget = Collapse.collapse(this.Graph, target, radius, -2, 0);

            var pathroute = Graph.ShortestPathHeuristic(superSource, superTarget);
            
            if(pathroute.Status == EPathStatus.Found)
            {
                if(pathroute.Jumps > 1)
                {
                    var edgeSource = Node.ShortestPathBetweenNeihbours(pathroute.Nodes[0], pathroute.Nodes[1]).Edges[0];
                    var edgeTarget = Node.ShortestPathBetweenNeihbours(pathroute.Nodes[pathroute.Nodes.Count() - 2], pathroute.Nodes[pathroute.Nodes.Count() - 1]).Edges[0];

                    pathroute.Nodes[0] = ((Edge) edgeSource.GetAttribute("original_edge")).Source;
                    pathroute.Nodes[pathroute.Nodes.Count() - 1] = ((Edge) edgeTarget.GetAttribute("original_edge")).Target;
                }
            }
            // before remove not implemented exception you must:
            // check if source and target are too close
            // checked if source and target are different
            throw new NotImplementedException();

            // return pathroute;
        }      

    }
}