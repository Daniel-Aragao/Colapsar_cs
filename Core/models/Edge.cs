using System;
using System.Collections.Generic;

namespace Core.models
{
    public class Edge
    {
        public Node Source { get; set; }
        public Node Target { get; set; }
        public double Weight { get; set; }
        public string Label { get; set; }
        const int ROUND_FIXED = 5;

        private Dictionary<string, object> OtherAttributes { get; } = new Dictionary<string, object>();
        
        public Edge()
        {
            
        }
        
        public Edge(Node source, Node target, double weight=0)
        {
            if(source == null || target == null)
            {
                throw new ArgumentNullException("Neither source or target can be null");
            }

            this.Source = source;
            this.Target = target;
            this.Weight = weight;
        }

        public void PutAttribute(string attr, Object value)
        {
            this.OtherAttributes.Add(attr, value);
        }

        public Object GetAttribute(string attr)
        {
            return this.OtherAttributes[attr];
        }

        public int CompareTo(Edge e1, Edge e2)
        {
            return e1.Weight.CompareTo(e2.Weight);
        }

        public override bool Equals(object obj)
        {
            
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Edge a = (Edge) obj;
            var weight = Math.Round(this.Weight, ROUND_FIXED);

            if(a.Source.Equals(this.Source) && a.Target.Equals(this.Target) && weight == Math.Round(a.Weight, ROUND_FIXED))
            {
                return true;
            }
            
            return false;
        }
        
        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}