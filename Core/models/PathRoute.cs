using System;
using System.Collections.Generic;


namespace Core.models
{
    public class PathRoute
    {
        public Edge[] Edges { get; }
        public Node[] Nodes { get; }
        public double Distance { get; }
        public int Jumps { get { return Edges.Length;} }
        public int JumpsToFind { get; set; }
        public EPathStatus Status { get; set; }

        public PathRoute(Edge[] edges, double distance)
        {
            this.Edges = edges;
            this.Distance = distance;

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
    }
    public enum EPathStatus
    {   
        Found=0,
        NotFound=1
    }

}