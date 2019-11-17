using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.extensions;

namespace Core.models
{
    /// A class that represents digraphs (directed graphs)
    public class Graph
    {
        public string Name { get; set; }
        private Dictionary<long, Node> _nodes = new Dictionary<long, Node>();
        public int NodesSize { get {return this._nodes.Count;} }

        // public IList<Node> Nodes { get { return this._nodes.Values.ToList();} }
        private IList<Edge> _edges = new List<Edge>();
        public int EdgesSize { get {return this._edges.Count;} }
        public int Size { get { return this._nodes.Count; } }
        public Node Hub { get { return this._nodes.OrderByDescending(k => k.Value.Degree).First().Value; } }
        public bool Directed { get { return true; } }

        public Graph()
        {

        }

        public Graph(string name)
        {
            this.Name = name;
        }

        public Node CreateNode(long id, string label, double weight = 0)
        {
            if (_nodes.ContainsKey(id))
            {
                throw new ArgumentException("Id already exists");
            }

            var node = new Node(id, label, weight);

            this._nodes.Add(id, node);

            return node;
        }

        public Node CreateNode(Node node)
        {
            if (_nodes.ContainsKey(node.Id))
            {
                throw new ArgumentException("Id already exists");
            }
            
            var new_node = node.Clone();
            this._nodes.Add(node.Id, new_node);

            return new_node;
        }

        public Edge CreateEdge(Node source, Node target, double weight = 0)
        {
            if (source == null || target == null)
            {
                throw new ArgumentNullException("Neither Source or Target can be null");
            }

            var edge = new Edge(source, target, weight);

            source.Edges.Add(edge);

            if (source != target)
                target.Edges.Add(edge);

            this._edges.Add(edge);

            return edge;
        }

        public Edge CreateEdge(long idSource, long idTarget, double weight = 0)
        {
            if (!this._nodes.ContainsKey(idSource) || !this._nodes.ContainsKey(idTarget))
            {
                throw new ArgumentException("Source or Target id does not exist");
            }

            return this.CreateEdge(this._nodes[idSource], this._nodes[idTarget], weight);
        }

        public Edge CreateEdge(Edge edge)
        {
            if(edge == null)
            {
                throw new ArgumentNullException("Edge can't be null");
            }

            if (!this._nodes.ContainsKey(edge.Source.Id) || !this._nodes.ContainsKey(edge.Target.Id))
            {
                throw new ArgumentException("Source or Target id does not exist");
            }

            Edge new_edge = CreateEdge(this._nodes[edge.Source.Id], this._nodes[edge.Target.Id], edge.Weight);
            new_edge.Label = edge.Label;
            
            edge.CloneAttributes(new_edge);
            // this._edges.Add(new_edge);

            return new_edge;
        }

        public Node RemoveNode(long id)
        {
            if (this._nodes.ContainsKey(id))
            {
                Node node = this._nodes[id];

                this.RemoveEdges(node);

                if (this._nodes.Remove(id))
                {
                    return node;
                }
            }

            return null;
        }

        public Node RemoveNode(Node node)
        {
            return this.RemoveNode(node.Id);
        }

        public Edge RemoveEdge(Edge edge)
        {
            if (this._edges.Remove(edge))
            {
                edge.Source.Edges.Remove(edge);
                edge.Target.Edges.Remove(edge);

                return edge;
            }

            return null;
        }

        public IList<Edge> RemoveEdges(Node source, Node target)
        {
            IList<Edge> edges = this._edges.Where(edge => edge.Source == source && edge.Target == target).ToList();

            foreach (Edge edge in edges)
            {
                this.RemoveEdge(edge);
            }

            return edges;
        }

        public IList<Edge> RemoveEdges(Node node)
        {
            IList<Edge> edges = this._edges.Where(edge => edge.Source == node || edge.Target == node).ToList();

            foreach (Edge edge in edges)
            {
                this.RemoveEdge(edge);
            }

            return edges;
        }

        public bool ExistNode(long id)
        {
            return this._nodes.ContainsKey(id);
        }

        public Node GetNodeById(long id)
        {
            if(this._nodes.ContainsKey(id))
            {
                return this._nodes[id];
            }

            return null;
        }

        public Node GetNodeByLabel(string label)
        {
            return this._nodes.First(keypair => label.Equals(keypair.Value.Label)).Value;
        }

        public IList<Node> GetNodesByRadius(Node source, double radius)
        {
            IList<Node> neightbours = new List<Node>();

            foreach (var node in this._nodes)
            {
                if (source.Position.DistanceFunction(node.Value.Position, source.Position) <= radius)
                {
                    neightbours.Add(node.Value);
                }
            }

            return neightbours;
        }

        public Edge GetEdgeByLabel(string label)
        {
            return this._edges.First(edge => label.Equals(edge.Label));
        }

        public Edge GetEdgeByIndex(int index)
        {
            return this._edges[index];
        }

        public double GetClusteringCoefficient()
        {
            double coefficient = 0d;

            float N = this._nodes.Count;

            foreach (var pair in this._nodes)
            {
                double coef = pair.Value.GetLocalClusteringCoefficient();

                if (!Double.IsNaN(coef))
                {
                    coefficient += coef;
                }
            }

            return coefficient / N;
        }

        public double Density()
        {
            float edgesSize = this._edges.Count;
            float nodesSize = this._nodes.Count;

            // if(!directed)
            // {
            //     edgesSize = 2 * edgesSize;
            // }
            return (edgesSize / (nodesSize * (nodesSize - 1f)));
        }

        public double Entropy()
        {
            float nodesSize = this._nodes.Count;
            double summ = 0f;

            var probabilities = from node in this._nodes
                                group node by node.Value.Degree into degrees
                                select new { Key = degrees.Key, Value = degrees.Count() / nodesSize };

            foreach (var degreeProb in probabilities)
            {
                summ += degreeProb.Value * (Math.Log(degreeProb.Value, 2));
            }

            return -summ;
        }

        public static PathRoute ShortestPathHeuristic(Node source, Node target)
        {
            Func<Node, Node, double> heuristic = (src, tgt) => src.Position.DistanceFunction(src.Position, tgt.Position);
            return ShortestPathHeuristic(source, target, heuristic);
        }

        public static PathRoute ShortestPathHeuristic(Node source, Node target, Func<Node, Node, double> distanceHeuristic)
        {
            return ShortestPathHeuristic(source, target, Node.defaultShortestPath, distanceHeuristic);
        }

        public static PathRoute ShortestPathHeuristic(Node source, Node target, Func<Edge, double> edgeWeightCalculation, Func<Node, Node, double> distanceHeuristic)
        {
            // f(n) = g(n) + h(n)
            if (source == null || target == null || edgeWeightCalculation == null || distanceHeuristic == null)
            {
                throw new ArgumentNullException("No parameter can be null");
            }
            else if (source == target || source.Id == target.Id)
            {
                throw new ArgumentException("Source and target must be different");
            }

            IList<Node> border = new List<Node>();
            IDictionary<long, double> weightToNode = new Dictionary<long, double>();
            IDictionary<long, double> totalCostForNode = new Dictionary<long, double>();
            IDictionary<long, Edge> parents = new Dictionary<long, Edge>();
            int quantityOfExpansions = 0;

            IList<long> searched = new List<long>();

            border.Add(source);
            weightToNode.Add(source.Id, 0);
            totalCostForNode.Add(source.Id, distanceHeuristic(source, target));

            Node current = null;

            while (border.Count != 0)
            {
                current = border[0];
                border.RemoveAt(0);
                quantityOfExpansions++;

                if (current == target) break;

                var currentWeight = weightToNode[current.Id];

                IList<Node> childrens = null;

                childrens = current.NeighborsOut();

                foreach (Node children in childrens)
                {
                    var bestRouteToChildren = Node.ShortestPathBetweenNeihbors(current, children, edgeWeightCalculation);

                    if (bestRouteToChildren.Status == EPathStatus.NotFound)
                    {
                        throw new Exception("Expanded node lost reference to children");
                    }

                    Edge edge = bestRouteToChildren.Edges[0];
                    var weight = bestRouteToChildren.Distance;

                    double weightToChildren = currentWeight + weight;
                    double costToTarget = distanceHeuristic(children, target);
                    double costFunction = weightToChildren + costToTarget;

                    if (weightToNode.ContainsKey(children.Id))
                    {
                        if (weightToNode[children.Id] <= weightToChildren)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        border.Remove(children);
                    }

                    weightToNode[children.Id] = weightToChildren;
                    totalCostForNode[children.Id] = costFunction;
                    parents[children.Id] = edge;

                    var index = border.FindIndex(node => totalCostForNode[node.Id] > costFunction);

                    border.Insert(index >= 0 ? index : border.Count, children);
                }
            }

            if (current == target)
            {
                IList<Edge> edges = new List<Edge>();
                EPathStatus status = EPathStatus.Found;

                var edge = parents[target.Id];
                edges.Add(edge);

                Node parent = edge.Source;

                while (parent != source)
                {
                    edge = parents[parent.Id];
                    edges.Add(edge);

                    if (edge == null || edge.Source == null)
                    {
                        status = EPathStatus.FailOnRouteBuilding;
                        break;
                    }

                    parent = edge.Source;
                }

                var pathRoute = new PathRoute(status, source, target, edges.Reverse().ToArray(), weightToNode[target.Id]);
                pathRoute.QuantityOfExpansions = quantityOfExpansions;

                return pathRoute;
            }

            return new PathRoute(EPathStatus.NotFound, source, target);
        }

        private double ThreadAveragePathLenght(IEnumerable<Node> nodes, Dictionary<long, Node>.ValueCollection targets)
        {
            double avg = 0;
            
            foreach (var source in nodes)
            {
                foreach (var target in targets)
                {
                    if (source != target)
                    {
                        var pathRoute = Graph.ShortestPathHeuristic(source, target);

                        if (pathRoute.Status == EPathStatus.Found)
                        {
                            avg += pathRoute.Distance;
                        }
                    }
                }
            }
            
            return avg;
        }

        public double AveragePathLenght()
        {
            double avg = 0;

            var nodes = this._nodes.Values;

            int nodesSize = nodes.Count;
            int possibleEdges = nodesSize * (nodesSize - 1);

            var nodesList = nodes.ToList();

            int threadsNumber = Environment.ProcessorCount;

            int interval = nodesSize / threadsNumber;
            int rest = nodesSize % threadsNumber;

            var threads = new Thread[threadsNumber];

            for(var i = 0; i < threadsNumber; i++)
            {
                int begin = i * interval;
                int end = interval;//i * interval + interval;

                if (i + 1 == threadsNumber)
                {
                    end += rest;
                }

                var tnodes = nodesList.GetRange(begin, end);

                var thread = new Thread(new ThreadStart(() => avg += this.ThreadAveragePathLenght(tnodes, nodes)));

                threads[i] = thread;
                thread.Start();
            }

            foreach(var thread in threads)
            {
                thread.Join();
            }

            avg = avg / possibleEdges;

            return avg;
        }

        public PathRoute Diamater()
        {
            PathRoute diameterPath = null;

            var nodes = this._nodes.Values;

            foreach (var source in nodes)
            {
                foreach (var target in nodes)
                {
                    if (source != target)
                    {
                        var pathRoute = Graph.ShortestPathHeuristic(source, target);

                        if (pathRoute.Status == EPathStatus.Found)
                        {
                            if (diameterPath == null || pathRoute.Distance > diameterPath.Distance)
                            {
                                diameterPath = pathRoute;
                            }
                        }
                    }
                }
            }

            return diameterPath;
        }

        public IList<Graph> ConnectedComponents()
        {
            throw new NotImplementedException();
        }

        public Graph BiggestComponent()
        {
            throw new NotImplementedException();
        }

        public Graph Clone()
        {
            Graph new_graph = new Graph(this.Name);

            foreach (var keypair in this._nodes)
            {
                new_graph.CreateNode(keypair.Value);
            }

            foreach (var edge in this._edges)
            {
                new_graph.CreateEdge(edge);
            }

            return new_graph;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var graph = (Graph)obj;

            if (this._nodes.Count != graph._nodes.Count || this._edges.Count != graph._edges.Count)
            {
                return false;
            }

            foreach (var node in this._nodes)
            {
                if (!graph._nodes.ContainsKey(node.Key))
                {
                    return false;
                }

                if (!graph._nodes[node.Key].Equals(node.Value))
                {
                    return false;
                }
            }

            foreach (var edge1 in this._edges)
            {
                var haveEqual = false;

                foreach (var edge2 in graph._edges)
                {
                    if (edge1.Equals(edge2))
                    {
                        haveEqual = true;
                        break;
                    }
                }

                if (!haveEqual)
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