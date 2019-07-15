using System;
using System.Collections.Generic;


namespace Core.models
{
    public class PathRoute
    {
        public Edge[] Edges { get; }
        public Node[] Nodes { get; }
        public double Distance { get; }
        public int Jumps { get { return Nodes.Length - 1; }}
        public int QuantityOfExpansions { get; set; }
        public EPathStatus Status { get; }
        
        public PathRoute(Edge[] edges, double distance, EPathStatus status)
        {
            if(edges == null)
            {
                throw new ArgumentNullException("Edges can't be null");
            }

            this.Edges = edges;
            this.Distance = distance;
            this.Status = status;

            this.Nodes = new Node[edges.Length + 1];

            for(var i = 0; i < edges.Length; i++)
            {
                if(i % 2 == 0)
                {
                    var edge = edges[i];
                    this.Nodes[i] = edge.Source;
                    this.Nodes[i + 1] = edge.Target;
                }
            }
        }

        public PathRoute(Node[] nodes, double distance, EPathStatus status)
        {
            if(nodes == null)
            {
                throw new ArgumentNullException("Nodes can't be null");
            }

            this.Nodes = nodes;
            this.Distance = distance;
            this.Status = status;
        }

        public PathRoute(EPathStatus status)
        {
            this.Status = status;
        }

        // // override object.Equals
        // public override bool Equals(object obj)
        // {
        //     if (obj == null || GetType() != obj.GetType())
        //     {
        //         return false;
        //     }
            

        //     return base.Equals (obj);
        // }
        
        // public override int GetHashCode()
        // {
        //     return base.GetHashCode();
        // }
    }
    public enum EPathStatus
    {   
        Found=0,
        NotFound=1
    }

}