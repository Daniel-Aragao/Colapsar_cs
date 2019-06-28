using System;
using System.Collections.Generic;
using System.Linq;

namespace colapsar_cs.models
{
    public class Node
    {
        public int Id { get; set; }
        public double Weight { get; set; }
        public string Label { get; set; }
        public List<Edge> Edges { get; } = new List<Edge>();

        public int Degree { get { return Edges.Count();} }
                
        public Dictionary<string,Object> OtherAttributes { get; } = new Dictionary<string, object>();
        
        public Node()
        {
            var e = new Edge();
        }

        public Node(int id)
        {
            this.Id = id;
        }

        public Node(int id, string label, double weight=0f)
        {
            this.Id = id;
            this.Weight = weight;
            this.Label = label;
        }

        public double GetClusteringCoefficient(bool directed=true)
        {		
            IList<Node> neighbors = this.Neighbors();
            
            double k = neighbors.Count();
            
            if(k == 1)
                return 1;
            
            double y = 0;
            
            for (int i = 0; i < neighbors.Count() - 1; i++) {
                for (int j = i + 1; j < neighbors.Count(); j++) {
                    Node neighbor1 = neighbors[i];
                    Node neighbor2 = neighbors[j];
                    
                    if(neighbor1.IsNeighbor(neighbor2)){
                        y += 1;
                    }
                }
            }
            
            // if(!directed)
            y *= 2;
                
            double coefficient = y / (k * (k - 1));
            
            return coefficient;
        }
        
        public IList<Edge> EdgesIn()
        {
            return Edges.Where(edge => edge.Source == this).ToList();
        }
        public IList<Edge> EdgesOut()
        {
            return Edges.Where(edge => edge.Target == this).ToList();
        }

        public IList<Node> Neighbors()
        {
            return (from edge in Edges select (edge.Source == this ? edge.Target : edge.Source)).Distinct().ToList();
        }

        public IList<Node> NeighborsIn()
        {
            return (from edge in Edges where edge.Target == this select edge.Source).Distinct().ToList();
        }

        public IList<Node> NeighborsOut()
        {
            return (from edge in Edges where edge.Source == this select edge.Target).Distinct().ToList();
        }
        public bool IsNeighbor(Node neighbor)
        {
            return this.Edges.Exists(edge => edge.Source == neighbor || edge.Target == neighbor);
        }
    }
}