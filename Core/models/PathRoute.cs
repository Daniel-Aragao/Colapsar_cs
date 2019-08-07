using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


namespace Core.models
{
    public class PathRoute
    {
        const string SEPARATOR = ",";
        const string PATH_SEPARATOR = ";";

        public Edge[] Edges { get; }
        public Node[] Nodes { get; }
        public Node Source { get; set;}
        public Node Target { get; set;}
        public double Distance { get; set; }
        public int Jumps { get { return Edges.Length; } }
        public EPathStatus Status { get; }
        public Exception Exception { get; }
        public int QuantityOfExpansions { get; set; }
        public TimeSpan DeltaTime { get; set; }

        public PathRoute(EPathStatus status, Node source, Node target, Edge[] edges, double distance)
        {
            ValidateCtor(source, target);

            if (edges == null || edges.Length == 0)
            {
                throw new ArgumentNullException("Edges can't be null or empty");
            }

            this.Edges = edges;
            this.Distance = distance;
            this.Status = status;

            this.Nodes = new Node[edges.Length + 1];

            for (var i = 0; i < edges.Length; i++)
            {
                var edge = edges[i];
                this.Nodes[i] = edge.Source;
                this.Nodes[i + 1] = edge.Target;
            }

            this.Source = source;
            this.Target = target;
        }

        // public PathRoute(Node[] nodes, double distance, EPathStatus status)
        // {
        //     if(nodes == null)
        //     {
        //         throw new ArgumentNullException("Nodes can't be null");
        //     }

        //     this.Nodes = nodes;
        //     this.Distance = distance;
        //     this.Status = status;
        // }

        public PathRoute(EPathStatus status, Node source, Node target)
        {
            ValidateCtor(source, target);

            this.Status = status;

            this.Source = source;
            this.Target = target;
        }

        public PathRoute(EPathStatus status, Node source, Node target, Exception e)
        {
            ValidateCtor(source, target);

            this.Status = status;
            this.Exception = e;

            this.Source = source;
            this.Target = target;
        }

        private void ValidateCtor(Node source, Node target)
        {
            if(source == null || target == null)
            {
                throw new ArgumentNullException("Neither Source or Target can be null");
            }
        }

        public string Path()
        {
            return String.Join(PathRoute.PATH_SEPARATOR, from node in this.Nodes select node.Id);
        }

        public override string ToString()
        {
            var returnString = this.Source.Id + PathRoute.SEPARATOR + this.Target.Id + PathRoute.SEPARATOR + this.Status.ToString() + PathRoute.SEPARATOR;

            switch (this.Status)
            {
                case EPathStatus.Found:
                    returnString += this.Jumps.ToString() + PathRoute.SEPARATOR;

                    if(this.QuantityOfExpansions != 0)
                    {
                        returnString += this.QuantityOfExpansions;
                    }

                    returnString += PathRoute.SEPARATOR;

                    if(this.DeltaTime != null)
                    {
                        returnString += this.DeltaTime.TotalMinutes;
                    }
                    
                    returnString += PathRoute.SEPARATOR;

                    returnString += this.Distance.ToString();

                    returnString += PathRoute.SEPARATOR;

                    returnString += this.Path();
                    break;
                
                case EPathStatus.NotFound:
                    returnString += "Path not found";
                    break;
                
                case EPathStatus.FailOnRouteBuilding:
                    returnString += "The path was found, but the pathroute instance couldn't be created";
                    break;

                case EPathStatus.SourceAndTargetAreEqual:
                    returnString += "Source is equal to target";
                    break;

                case EPathStatus.SourceAndTargetAreToCloseToCollapse:
                    returnString += "Can't collapse source and destination because the radius is bigger or equal then the distance between then";
                    break;

                case EPathStatus.SourceOrTargetDoNotExist:
                    returnString += "Source and target should should exist in the given Graph and must not be null";
                    break;

                case EPathStatus.UnexpectedException:
                    returnString += "An Unexpected error happened" + PathRoute.SEPARATOR + this.Exception.Message.Replace('\n', '\t') + PathRoute.SEPARATOR + this.Exception.TargetSite;
                    break;                
            }

            return returnString;
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
        Found = 0,
        NotFound = 1,
        FailOnRouteBuilding = 2,
        SourceOrTargetDoNotExist = 3,
        SourceAndTargetAreToCloseToCollapse = 4,
        SourceAndTargetAreEqual = 5,
        UnexpectedException = 6
    }

}