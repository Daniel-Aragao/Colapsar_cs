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
        public IList<Edge> EdgesIn
        {
            get { return Edges.Where(edge => edge.Source == this).ToList(); }
        }
        public IList<Edge> EdgesOut
        {
            get { return Edges.Where(edge => edge.Target == this).ToList(); }
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

        public Node(int id, float weight, string label)
        {
            this.Id = id;
            this.Weight = weight;
            this.Label = label;
        }
    }
}