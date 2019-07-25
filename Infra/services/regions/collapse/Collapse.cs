using System;
using System.Collections.Generic;
using System.Linq;

using Core;
using Core.models;

namespace Infra.services.regions
{
    public class Collapse : SearchStrategy
    {
        public Collapse(Graph graph) : base(graph, Constants.ALGORITHMN_NAME_COLLAPSE)
        {

        }

        public override PathRoute Search(Node source, Node target, double radius)
        {
            PathRoute pathRoute = this.SearchParametersEvaluation(source, target, radius);

            if (pathRoute != null)
            {
                return pathRoute;
            }

            var superSource = Collapse.collapse(this.Graph, source, radius, -1, 0);
            var superTarget = Collapse.collapse(this.Graph, target, radius, -2, 0);

            try
            {
                pathRoute = Graph.ShortestPathHeuristic(superSource, superTarget);

                if (pathRoute.Status == EPathStatus.Found)
                {
                    if (pathRoute.Jumps > 1)
                    {
                        var fakeSource = Node.ShortestPathBetweenNeihbours(pathRoute.Nodes[0], pathRoute.Nodes[1]).Edges[0];
                        var fakeTarget = Node.ShortestPathBetweenNeihbours(pathRoute.Nodes[pathRoute.Nodes.Count() - 2], pathRoute.Nodes[pathRoute.Nodes.Count() - 1]).Edges[0];

                        var originalSource = (Edge)fakeSource.GetAttribute("original_edge");
                        var originalTarget = (Edge)fakeTarget.GetAttribute("original_edge");
                        //TODO: update the pathroute distance too?
                        
                        pathRoute.Nodes[0] = originalSource.Source;
                        pathRoute.Nodes[pathRoute.Nodes.Count() - 1] = originalTarget.Target;

                        pathRoute.Edges[0] = originalSource;
                        pathRoute.Edges[pathRoute.Edges.Count() - 1] = originalTarget;
                    }
                    else
                    {
                        pathRoute.Nodes[0] = source;
                    }
                }

            }
            catch (Exception e)
            {
                pathRoute = new PathRoute(EPathStatus.UnexpectedException, e);
            }
            finally
            {
                Collapse.Expand(this.Graph, -1);
                Collapse.Expand(this.Graph, -2);
            }

            return pathRoute;
        }

        public static Node collapse(Graph graph, Node source, double radius, long id = -1, double weight = 0)
        {
            IList<Node> nodes = graph.GetNodesByRadius(source, radius);

            var superNode = graph.CreateNode(id, "super_" + id, weight);
            superNode.Position = new Position(source.Position.X, source.Position.Y, source.Position.DistanceFunction);

            foreach (var node in nodes)
            {
                var edges = node.EdgesIn();

                foreach (var edge in edges)
                {
                    if (edge.Source != superNode)
                    {
                        var newEdge = graph.CreateEdge(edge.Source, superNode, edge.Weight);
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
                        var newEdge = graph.CreateEdge(superNode, edge.Target, edge.Weight);
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
            graph.RemoveNode(superNodeId);
        }

        public static void Expand(Graph graph, Node superNode)
        {
            Expand(graph, superNode.Id);
        }

        protected override PathRoute SearchParametersEvaluation(Node source, Node target, double radius)
        {
            var pathRoute = base.SearchParametersEvaluation(source, target, radius);

            if (pathRoute != null)
            {
                return pathRoute;
            }
            else if (source.Position.DistanceFunction(source.Position, target.Position) <= radius)
            {
                return new PathRoute(EPathStatus.SourceAndTargetAreToCloseToCollapse);
            }

            return null;
        }
    }
}