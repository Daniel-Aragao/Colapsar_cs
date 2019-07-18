using System;
using System.Collections.Generic;
using System.Linq;
using Core.models;

namespace Infra.services
{
    public class Collapse
    {
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

                        foreach(var attribute in newEdge.getAttributes())
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

                        foreach(var attribute in newEdge.getAttributes())
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