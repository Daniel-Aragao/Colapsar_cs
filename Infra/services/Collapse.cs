using System;
using System.Collections.Generic;
using System.Linq;
using Core.models;

namespace Infra.services
{
    public class Collapse : SearchThread
    {
        public Graph Graph {get;}

        public Collapse(Graph graph): base(graph)
        {

        }

        public override PathRoute Search(Node source, Node target, double radius)
        {
            if(source == null || target == null || !this.Graph.Nodes.ContainsKey(source.Id) || !this.Graph.Nodes.ContainsKey(target.Id))
            {
                return new PathRoute(EPathStatus.SourceOrTargetDoNotExist);
            }
            else if(source == target)
            {
                return new PathRoute(EPathStatus.SourceAndTargetAreEqual);
            }
            else if(source.Position.DistanceFunction(source.Position, target.Position) <= radius)
            {
                return new PathRoute(EPathStatus.SourceAndTargetAreToCloseToCollapse);
            }

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
                }else
                {
                    pathroute.Nodes[0] = source;
                }
            }
            // before remove not implemented exception you must:
            // check if source and target are too close
            // checked if source and target are different
            throw new NotImplementedException();

            return pathroute;
        }
        
        public static Node collapse(Graph graph, Node source, double radius, long id=-1, double weight=0)
        {
            IList<Node> nodes = Collapse.GetNodesInRadius(graph.Nodes, source, radius);

            var superNode = graph.CreateNode(id, "super_" + id, weight);

            foreach(var node in nodes)
            {
                var edges = node.EdgesIn();

                foreach(var edge in edges)
                {
                    if (edge.Source != superNode)
                    {
                        var newEdge = graph.CreateEdge(edge.Source, superNode);
                        newEdge.PutAttribute("original_edge", edge);

                        foreach(var attribute in newEdge.GetAttributes())
                        {
                            newEdge.PutAttribute(attribute, newEdge.GetAttribute(attribute));
                        }
                    }
                }

                edges = node.EdgesOut();

                foreach(var edge in edges)
                {
                    if (edge.Target != superNode)
                    {
                        var newEdge = graph.CreateEdge(superNode, edge.Target);
                        newEdge.PutAttribute("original_edge", edge);

                        foreach(var attribute in newEdge.GetAttributes())
                        {
                            newEdge.PutAttribute(attribute, newEdge.GetAttribute(attribute));
                        }
                    }
                }
            }

            return superNode;
        }

        public static IList<Node> GetNodesInRadius(IDictionary<long, Node> nodes, Node source, double radius)
        {
            IList<Node> neightbours = new List<Node>();

            foreach(var node in nodes)
            {
                if(source.Position.DistanceFunction(node.Value.Position, source.Position) <= radius)
                {
                    neightbours.Add(node.Value);
                }
            }

            return neightbours;
        }
    }
}