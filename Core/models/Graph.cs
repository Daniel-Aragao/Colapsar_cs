using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.models
{
    public class Graph
    {
        public string Name { get; set; }
        public Dictionary<int, Node> Nodes { get; private set; } = new Dictionary<int,Node>();
        public IList<Edge> Edges { get; private set; } = new List<Edge>();
        public int Size { get { return this.Nodes.Count;} }
        public Node Hub { get { return this.Nodes.OrderByDescending(k => k.Value.Degree).First().Value;}  }
        

        public Graph()
        {
             
        }

        public Graph(string name)
        {
            this.Name = name;
        }

        public Node CreateNode(int id, string label, double weight=0)
        {
            if(Nodes.ContainsKey(id))
            {
                return null;
            }

            var node = new Node(id, label, weight);

            this.Nodes.Add(id, node);

            return node;
        }

        public Edge CreateEdge(Node source, Node target, double weight=0)
        {
            if(source == null || target == null)
            {
                throw new ArgumentNullException("Source and Target can't be null");
            }

            var edge = new Edge(source, target, weight);

            source.Edges.Add(edge);

            if(source != target)
                target.Edges.Add(edge);

            return edge;
        }

        public Edge CreateEdge(int idSource, int idTarget, double weight=0)
        {
            if(!this.Nodes.ContainsKey(idSource) || !this.Nodes.ContainsKey(idTarget))
            {
                throw new ArgumentNullException("Source or Target id does not exist");
            }

            return this.CreateEdge(this.Nodes[idSource], this.Nodes[idTarget], weight);
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

        public double GetClusteringCoefficient(bool directed)
        {		
            double coefficient = 0.0f;
            
            double N = this.Nodes.Count;
            
            foreach (var pair in this.Nodes) {
                double coef = pair.Value.GetLocalClusteringCoefficient(directed);

                if(!Double.IsNaN(coef)){
                    coefficient += coef;
                }
            }
            
            return coefficient / N ;
        }

        public double Density(bool directed)
        { 
            var edgesSize = this.Edges.Count;
            var nodeSize = this.Nodes.Count;

            if(!directed)
            {
                edgesSize = 2 * edgesSize;
            }

            return edgesSize / (nodeSize * (nodeSize - 1));
        }

        public double Entropy()
        {
            
        }
    }
}