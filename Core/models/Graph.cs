using System;
using System.Collections.Generic;
using System.Linq;
using Core.extensions;

namespace Core.models
{
    /// A class the represents digraphs (directed graphs)
    public class Graph
    {
        public string Name { get; set; }
        public Dictionary<long, Node> Nodes { get; private set; } = new Dictionary<long,Node>();
        public IList<Edge> Edges { get; private set; } = new List<Edge>();
        public int Size { get { return this.Nodes.Count;} }
        public Node Hub { get { return this.Nodes.OrderByDescending(k => k.Value.Degree).First().Value;}  }
        // public bool Directed { get; set; }
        
        public Graph()
        {

        }

        public Graph(string name)
        {
            this.Name = name;
        }

        public Node CreateNode(long id, string label, double weight=0)
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

        public Edge CreateEdge(long idSource, long idTarget, double weight=0)
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

        public double GetClusteringCoefficient()
        {		
            double coefficient = 0.0f;
            
            double N = this.Nodes.Count;
            
            foreach (var pair in this.Nodes) {
                double coef = pair.Value.GetLocalClusteringCoefficient();

                if(!Double.IsNaN(coef)){
                    coefficient += coef;
                }
            }
            
            return coefficient / N ;
        }

        public double Density()
        { 
            var edgesSize = this.Edges.Count;
            var nodeSize = this.Nodes.Count;

            // if(!directed)
            // {
            //     edgesSize = 2 * edgesSize;
            // }

            return edgesSize / (nodeSize * (nodeSize - 1));
        }

        public double Entropy()
        {
            float nodesSize = this.Nodes.Count;
            double summ = 0f;

            var probabilities = from node in this.Nodes
            group node by node.Value.Degree into degrees
            select new { Key = degrees.Key, Value = degrees.Count()/nodesSize };

            foreach (var degreeProb in probabilities)
            {
                summ += degreeProb.Value * ( Math.Log(degreeProb.Value, 2) );
            }

            return - summ;
        }
        
        public static PathRoute ShortestPathHeuristic(Node source, Node target)
        {
            Func<Node, Node, double> heuristic = (src, tgt) => Position.GeoCoordinateDistance(src.Position, tgt.Position);
            return ShortestPathHeuristic(source, target, Node.defaultShortestPath, heuristic);
        }

        public static PathRoute ShortestPathHeuristic(Node source, Node target, Func<Node, Node, double> distanceHeuristic)
        {
            return ShortestPathHeuristic(source, target, Node.defaultShortestPath, distanceHeuristic);
        }

        public static PathRoute ShortestPathHeuristic(Node source, Node target, Func<Edge, double> edgeWeightCalculation, Func<Node, Node, double> distanceHeuristic)
        {   
            // f(n) = g(n) + h(n)
            if(source == null || target == null || edgeWeightCalculation == null || distanceHeuristic == null)
            {
                throw new ArgumentNullException("No parameter can be null");
            }

            if(source == target || source.Id == target.Id)
            {
                throw new ArgumentException("Source and target must be different");
            }

            IList<Node> border = new List<Node>();
            IDictionary<long, double> weightToNode = new Dictionary<long, double>();
            IDictionary<long, double> totalCostForNode = new Dictionary<long, double>();
            IDictionary<long, Node> parents = new Dictionary<long, Node>();
            int quantityOfExpansions = 0;

            IList<long> searched = new List<long>();

            border.Add(source);
            weightToNode.Add(source.Id, 0);
            totalCostForNode.Add(source.Id, distanceHeuristic(source, target));

            Node current = null;

            while(border.Count != 0)
            {
                current = border[0];
                border.RemoveAt(0);
                quantityOfExpansions++;

                if(current == target) break;

                var currentWeight = weightToNode[current.Id];
                
                IList<Node> childrens = null;

                childrens = current.NeighborsOut();
                
                foreach (Node children in childrens)
                {
                    var bestRouteToChildren = Node.shortestPathBetweenNeihbours(current, children, edgeWeightCalculation);

                    Edge edge = bestRouteToChildren.Edges[0];
                    var weight = bestRouteToChildren.Distance;

                    double weightToChildren = currentWeight + weight ;
                    double costToTarget = distanceHeuristic(children, target);
                    double costFunction = weightToChildren + costToTarget;

                    if(weightToNode.ContainsKey(children.Id))
                    {
                        if(weightToNode[children.Id] <= weightToChildren)
                        {
                            continue;
                        }
                    }else
                    {
                        border.Remove(children);
                    }

                    weightToNode[children.Id] = weightToChildren;
                    totalCostForNode[children.Id] = costFunction;
                    parents[children.Id] = current;

                    var index = border.FindIndex(node => totalCostForNode[node.Id] > costFunction);
                    
                    border.Insert(index >= 0 ? index : border.Count, children);

                }
            }

            if(current == target)
            {
                IList<Node> nodes = new List<Node>();
                nodes.Add(target);

                Node parent = target;

                while(parent != source)
                {
                    parent = parents[parent.Id];
                    nodes.Add(parent);
                }

                var pathRoute = new PathRoute(nodes.Reverse().ToArray(), weightToNode[target.Id], EPathStatus.Found);
                pathRoute.QuantityOfExpansions = quantityOfExpansions;

                return pathRoute;
            }

            return new PathRoute(EPathStatus.NotFound);            
        }

        public double AveragePathLenght()
        {
            double avg = 0;

            var nodes = this.Nodes.Values;

            int nodesSize = nodes.Count;
            int possibleEdges = nodesSize * (nodesSize - 1);

            foreach(var source in nodes)
            {
                foreach(var target in nodes)
                {
                    if(source != target)
                    {
                        var pathRoute = Graph.ShortestPathHeuristic(source, target);

                        if(pathRoute.Status == EPathStatus.Found)
                        {
                            avg += pathRoute.Distance;
                        }
                    }
                }
            }

            avg = avg / possibleEdges;

            return avg;
        }

        public double Diamater()
        {
            double diameter = 0;

            var nodes = this.Nodes.Values;

            int nodesSize = nodes.Count;
            int possibleEdges = nodesSize * (nodesSize - 1);

            foreach(var source in nodes)
            {
                foreach(var target in nodes)
                {
                    if(source != target)
                    {
                        var pathRoute = Graph.ShortestPathHeuristic(source, target);

                        if(pathRoute.Status == EPathStatus.Found)
                        {
                            diameter = Math.Max(diameter, pathRoute.Distance);
                        }
                    }
                }
            }

            return diameter;
        }

        public IList<Graph> ConnectedComponents()
        {
            throw new NotImplementedException();
        }

        public Graph BiggestComponent()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            var graph = (Graph) obj;

            foreach (var node in this.Nodes)
            {
                if(!graph.Nodes.ContainsKey(node.Key))
                {
                    return false;
                }
            }

            foreach (var edge1 in this.Edges)
            {
                var haveEqual = false;

                foreach (var edge2 in graph.Edges)
                {
                    if(edge1.Equals(edge2))
                    {
                        haveEqual = true;
                        break;
                    }
                }

                if(!haveEqual)
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}