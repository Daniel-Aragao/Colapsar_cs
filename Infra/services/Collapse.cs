using System;
using System.Collections.Generic;
using System.Linq;
using Core.models;

namespace Infra.services
{
    public class Collapse : SearchThread
    {
        public Collapse(Graph graph) : base(graph)
        {

        }

        public override PathRoute Search(Node source, Node target, double radius)
        {
            if (source == null || target == null || !this.Graph.ExistNode(source.Id) || !this.Graph.ExistNode(target.Id))
            {
                return new PathRoute(EPathStatus.SourceOrTargetDoNotExist);
            }
            else if (source == target)
            {
                return new PathRoute(EPathStatus.SourceAndTargetAreEqual);
            }
            else if (source.Position.DistanceFunction(source.Position, target.Position) <= radius)
            {
                return new PathRoute(EPathStatus.SourceAndTargetAreToCloseToCollapse);
            }

            var superSource = Collapse.collapse(this.Graph, source, radius, -1, 0);
            var superTarget = Collapse.collapse(this.Graph, target, radius, -2, 0);

            PathRoute pathroute = null;

            try
            {
                pathroute = Graph.ShortestPathHeuristic(superSource, superTarget);

                if (pathroute.Status == EPathStatus.Found)
                {
                    if (pathroute.Jumps > 1)
                    {
                        var edgeSource = Node.ShortestPathBetweenNeihbours(pathroute.Nodes[0], pathroute.Nodes[1]).Edges[0];
                        var edgeTarget = Node.ShortestPathBetweenNeihbours(pathroute.Nodes[pathroute.Nodes.Count() - 2], pathroute.Nodes[pathroute.Nodes.Count() - 1]).Edges[0];

                        pathroute.Nodes[0] = ((Edge)edgeSource.GetAttribute("original_edge")).Source;
                        pathroute.Nodes[pathroute.Nodes.Count() - 1] = ((Edge)edgeTarget.GetAttribute("original_edge")).Target;
                    }
                    else
                    {
                        pathroute.Nodes[0] = source;
                    }
                }
                
                return pathroute;
            }
            catch (Exception e)
            {
                Collapse.Expand(this.Graph, -1);
                Collapse.Expand(this.Graph, -2);

                return new PathRoute(EPathStatus.UnknowException, e);
            }
        }

        public static Node collapse(Graph graph, Node source, double radius, long id = -1, double weight = 0)
        {
            IList<Node> nodes = graph.GetNodesByRadius(source, radius);

            var superNode = graph.CreateNode(id, "super_" + id, weight);

            foreach (var node in nodes)
            {
                var edges = node.EdgesIn();

                foreach (var edge in edges)
                {
                    if (edge.Source != superNode)
                    {
                        var newEdge = graph.CreateEdge(edge.Source, superNode);
                        newEdge.PutAttribute("original_edge", edge);

                        foreach (var attribute in newEdge.GetAttributes())
                        {
                            newEdge.PutAttribute(attribute, newEdge.GetAttribute(attribute));
                        }
                    }
                }

                edges = node.EdgesOut();

                foreach (var edge in edges)
                {
                    if (edge.Target != superNode)
                    {
                        var newEdge = graph.CreateEdge(superNode, edge.Target);
                        newEdge.PutAttribute("original_edge", edge);

                        foreach (var attribute in newEdge.GetAttributes())
                        {
                            newEdge.PutAttribute(attribute, newEdge.GetAttribute(attribute));
                        }
                    }
                }
            }

            return superNode;
        }

        public static void Expand(Graph graph, long superNodeId)
        {
            Expand(graph, graph.GetNodeById(superNodeId));
        }

        public static void Expand(Graph graph, Node superNode)
        {
            var removed = graph.RemoveNode(superNode);
        }
    }
}