using System;
using System.Collections.Generic;

namespace Core.models
{
    public class Edge
    {
        public Node Source { get; set; }
        public Node Target { get; set; }
        public double Weight { get; set; }
        public Dictionary<string,Object> OtherAttributes { get; private set;} = new Dictionary<string, object>();
        
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

            if(a.Source == this.Source && a.Target == this.Target)
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