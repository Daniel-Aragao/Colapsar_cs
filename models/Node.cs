using System;
using System.Collections.Generic;
using System.Linq;

namespace colapsar_cs.models
{
    public class Node
    {
        public int Id { get; set; }
        public float Weight { get; set; }
        public string Label { get; set; }
        public List<Edge> Edges { get; } = new List<Edge>();

        public int Degree { get { return Edges.Count();} }
        public IList<Edge> EdgesIn
        {
            get { return Edges.Where(edge => edge.Source == this).ToList(); }
        }
        public IList<Edge> EdgesOut
        {
            get { return Edges.Where(edge => edge.Target == this).ToList(); }
        }

        public IList<Node> Neighbors
        {
            get 
            { 
                return (from edge in Edges select (edge.Source == this ? edge.Target : edge.Source)).Distinct().ToList();
            }
        }

        public IList<Node> NeighborsIn
        {
            get 
            { 
                return (from edge in Edges where edge.Target == this select edge.Source).Distinct().ToList();
            }
        }

        public IList<Node> NeighborsOut
        {
            get 
            { 
                return (from edge in Edges where edge.Source == this select edge.Target).Distinct().ToList();
            }
        }
        
        public Dictionary<string,Object> OtherAttributes { get; } = new Dictionary<string, object>();
        
        public Node()
        {
            var e = new Edge();
        }

        public Node(int id)
        {
            this.Id = id;
        }

        public Node(int id, string label, float weight=0f)
        {
            this.Id = id;
            this.Weight = weight;
            this.Label = label;
        }

        // public double getClusteringCoefficient(){	
		
        //     IList<Node> neighbors = this.Neighbors;
            
        //     double k = neighbors.Count();
            
        //     if(k == 1)
        //         return 1;
            
        //     double y = 0;
            
        //     for (int i = 0; i < neighbors.Count(); i++) {
        //         for (int j = 0; j < neighbors.Count(); j++) {
        //             if(j > i){
        //                 Node neighbor1 = neighbors[i];
        //                 Node neighbor2 = neighbors[j];
                        
        //                 if(neighbor1.IsNeighbor(neighbor2)){
        //                     y += 1;
        //                 }
        //             }			
        //         }
        //     }
            
        //     double coefficient = (2 * y) / (k*(k-1));
            
        //     return coefficient;
        // }
        
        public bool IsNeighbor(Node neighbor)
        {
            return this.Edges.Exists(edge => edge.Source == neighbor || edge.Target == neighbor);
        }
    }
}