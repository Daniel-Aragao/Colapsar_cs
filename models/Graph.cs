using System;
using System.Collections.Generic;
using System.Linq;

namespace colapsar_cs.models
{
    public class Graph
    {
        public string Name { get; set; }
        public Dictionary<int, Node> Nodes { get; private set; } = new Dictionary<int,Node>();
        public IList<Edge> Edges { get; private set; } = new List<Edge>();
        public int Size { get { return this.Nodes.Count();} }
        public Node Hub { get { return this.Nodes.OrderByDescending(k => k.Value.Degree).First().Value;}  }

        public Graph()
        {
             
        }

        public Graph(string name)
        {
            this.Name = name;
        }

        public Node CreateNode(int id, string label, double weight)
        {
            if(Nodes.ContainsKey(id))
            {
                return null;
            }

            var node = new Node(id, label, weight);

            this.Nodes.Add(id, node);

            return node;
        }

        public Edge CreateEdge(Node source, Node target, double weight)
        {
            var edge = new Edge(source, target, weight);

            source.Edges.Add(edge);
            target.Edges.Add(edge);

            return edge;
        }

        public Node RemoveNode(int id)
        {
            if(this.Nodes.ContainsKey(id)){
                Node node = this.Nodes[id];

                if(this.Nodes.Remove(id)){
                    return node;
                }
            }

            return null;
        }

        public Node RemoveNode(Node node)
        {
            return this.RemoveNode(node);
        }

        public Edge RemoveEdge(Edge edge)
        {
            if(this.Edges.Remove(edge))
            {
                return edge;
            }

            return null;
        }

        public IList<Edge> RemoveEdges(Node source, Node target, bool removeFromNodes=false)
        {
            IList<Edge> edges = this.Edges.Where(edge => edge.Source == source && edge.Target == target).ToList();

            foreach(Edge edge in edges)
            {
                this.Edges.Remove(edge);

                if(removeFromNodes)
                {
                    source.Edges.Remove(edge);
                    target.Edges.Remove(edge);
                }
            }

            return edges;
        }

        public double GetClusteringCoefficient()
        {		
            double coefficient = 0.0f;
            
            double N = this.Nodes.Count();
            
            foreach (var pair in this.Nodes) {
                double coef = pair.Value.GetClusteringCoefficient();

                if(!Double.IsNaN(coef)){
                    coefficient += coef;
                }
            }
            
            return coefficient / N ;
        }
    }
}