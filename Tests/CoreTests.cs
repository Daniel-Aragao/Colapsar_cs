using Core.models;
using System.Collections;
using System.Collections.Generic;

using System;
using Xunit;

namespace Tests
{
    public class CoreTests
    {   
        const double MIN_DIFFERENCE = 0.000001;
        const int ROUND_FIXED = 5;
        const int ROUND_FIXED_FOR_DISTANCE = 2;

        Graph[] Gs;

        NodeEqualityComparer nodeEqualityComparer = new NodeEqualityComparer();

        public CoreTests()
        {
            Graph G1 = new Graph("test graph 1");

            G1.CreateNode(1, "n1");
            G1.CreateNode(2, "n2");
            G1.CreateNode(3, "n3");
            G1.CreateNode(4, "n4");

            G1.CreateEdge(1, 2);
            G1.CreateEdge(1, 3);
            G1.CreateEdge(1, 4);
            G1.CreateEdge(2, 3);
            G1.CreateEdge(2, 4);
            G1.CreateEdge(3, 4);

            Graph G2 = new Graph("test graph 2");

            G2.CreateNode(1, "n1");
            G2.CreateNode(2, "n2");
            G2.CreateNode(3, "n3");
            G2.CreateNode(4, "n4");

            G2.CreateEdge(1, 2);
            G2.CreateEdge(1, 3);
            G2.CreateEdge(1, 4);
            G2.CreateEdge(3, 4);

            Graph G3 = new Graph("test graph 3");

            G3.CreateNode(1, "n1");
            G3.CreateNode(2, "n2");
            G3.CreateNode(3, "n3");
            G3.CreateNode(4, "n4");
            
            G3.CreateEdge(1, 2);
            G3.CreateEdge(1, 3);
            G3.CreateEdge(1, 4);

            Gs = new Graph[] { G1, G2, G3 };
        }

        public Graph getGraph(int id){
            return Gs[id];
        }

        [Fact]
        public void TheGraph1MustReturnTheCorrectsNodesFromN2Neighbors()
        {
            Graph g = getGraph(0);
            IList<Node> correctNodes = new List<Node>();
            correctNodes.Add(g.Nodes[1]);
            correctNodes.Add(g.Nodes[3]);
            correctNodes.Add(g.Nodes[4]);

            var nodes = g.Nodes[2].Neighbors();

            Assert.Equal(correctNodes, nodes, nodeEqualityComparer);
        }

        [Fact]
        public void TheGraph2MustReturnTheCorrectsNodesFromN3Neighbors()
        {
            Graph g = getGraph(1);
            IList<Node> correctNodes = new List<Node>();
            correctNodes.Add(g.Nodes[1]);
            correctNodes.Add(g.Nodes[4]);

            var nodes = g.Nodes[3].Neighbors();

            Assert.Equal(correctNodes, nodes, nodeEqualityComparer);
        }

        [Fact]
        public void TheGraph2MustReturnTheCorrectsNodesFromN4Neighbors()
        {
            Graph g = getGraph(1);
            IList<Node> correctNodes = new List<Node>();
            correctNodes.Add(g.Nodes[1]);
            correctNodes.Add(g.Nodes[3]);

            var nodes = g.Nodes[4].Neighbors();

            Assert.Equal(correctNodes, nodes, nodeEqualityComparer);
            Assert.Equal(2, nodes.Count);
        }

        [Fact]
        public void TheNode1MustReturnTheCorrectEdgeNumberInEdgesWhenSourceOfTarget()
        {
            Graph g = getGraph(0);
            var target = g.Nodes[2];
            var edges = g.Nodes[1].EdgesWhenSourceOf(target);

            Assert.Equal(1, edges.Count);

            foreach(Edge edge in edges)
            {
                Assert.Equal(edge.Source, g.Nodes[1]);
                Assert.Equal(edge.Target, target);
            }
        }

        [Theory]
        [InlineData(0, 1, 3)]
        [InlineData(0, 2, 3)]
        [InlineData(0, 3, 3)]
        [InlineData(0, 4, 3)]
        [InlineData(1, 1, 3)]
        [InlineData(1, 2, 1)]
        [InlineData(1, 3, 2)]
        [InlineData(1, 4, 2)]
        [InlineData(2, 1, 3)]
        [InlineData(2, 2, 1)]
        [InlineData(2, 3, 1)]
        [InlineData(2, 4, 1)]
        public void ReturnTheCorrectAmountOfNeihboursForTheGivenGraphAndGivenNode(int gId, int nId, double result)
        {
            Assert.Equal(result, getGraph(gId).Nodes[nId].Neighbors().Count);
        }

        [Theory]
        [InlineData(0, 0.5)]
        [InlineData(1, 0.16667)]
        [InlineData(2, 0)]
        public void ReturnTheCorrectLocalClusterCoefficientGivenTheGraphAndNodeN1AndDirectedGraph(int gId, double result)
        {
            var coef = getGraph(gId).Nodes[1].GetLocalClusteringCoefficient();
            
            Assert.Equal(result, Math.Round(coef, CoreTests.ROUND_FIXED));
        }

        // [Theory]
        // [InlineData(0, 1)]
        // [InlineData(1, 0.33333)]
        // [InlineData(2, 0)]
        // public void ReturnTheCorrectLocalClusterCoefficientGivenTheGraphAndNodeN1AndUnDirectedGraph(int gId, double result)
        // {
        //     var coef = getGraph(gId).Nodes[1].GetLocalClusteringCoefficient(directed=false);

        //     Assert.Equal(result, Math.Round(coef, CoreTests.ROUND_FIXED));
        // }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1.5)]
        [InlineData(2, 0.81128)]
        public void ReturnTheCorrectShannonsEntropyForTheGivenGraph(int gId, double result)
        {
            var entropy = getGraph(gId).Entropy();

            Assert.Equal(result, Math.Round(entropy, CoreTests.ROUND_FIXED));
        }

        [Fact]
        public void ForTheGivenPathReturnTheCorrectNodes()
        {
            var g = getGraph(0);
            
            Edge[] edges = new Edge[3];
            edges[0] = g.Nodes[1].EdgesWhenSourceOf(g.Nodes[2])[0];
            edges[1] = g.Nodes[2].EdgesWhenSourceOf(g.Nodes[3])[0];
            edges[2] = g.Nodes[3].EdgesWhenSourceOf(g.Nodes[4])[0];

            Node[] nodes = new Node[] { g.Nodes[1], g.Nodes[2], g.Nodes[3], g.Nodes[4]};

            PathRoute pr = new PathRoute(edges, 4, EPathStatus.Found);

            Assert.Equal(nodes, pr.Nodes);
        }

        [Theory]
        [InlineData(10, 10, 13, 14,5)]
        [InlineData(0, 0, 3, 4, 5)]
        [InlineData(0, 7, 3, 6, 3.16228)]
        public void ReturnTheGivenEuclideanBasedOnThePositionsPassed(double x1, double y1, double x2, double y2, double result)
        {
            var p1 = new Position(x1, y1, Position.EucledeanDistance);
            var p2 = new Position(x2, y2, Position.EucledeanDistance);            

            Assert.Equal(result, Math.Round(p1.Distance(p2), CoreTests.ROUND_FIXED));
        }

        // [Theory]
        // [InlineData(50.06638, 5.71472, 58.64389, 3.07000, 996.17)]
        // [InlineData(139.74477, 35.6544, 39.8261, 21.4225, 9480.66)]
        // public void ReturnTheGivenHaversineBasedOnThePositionsPassed(double x1, double y1, double x2, double y2, double result)
        // {
        //     var p1 = new Position(x1, y1, Position.HaversineDistance);
        //     var p2 = new Position(x2, y2, Position.HaversineDistance);
        //     Assert.Equal(result, Math.Round(p1.Distance(p2), CoreTests.ROUND_FIXED_FOR_DISTANCE));
        // }

        [Theory]
        [InlineData(50.0663800, 5.71472, 58.64389, 3.07000, 996.18)]
        [InlineData(50.0663800, 5.71472, 50.0663800, 5.71472, 0)]
        [InlineData(58.64389, 3.07000, 58.64389, 3.07000, 0)]
        [InlineData(139.74477, 35.6544, 39.8261, 21.4225, 9488.84)]
        [InlineData(139.74477, 35.6544, 139.74477, 35.6544, 0)]
        [InlineData(39.8261, 21.4225, 39.8261, 21.4225, 0)]
        public void ReturnTheGivenGeoLocationBasedOnThePositionsPassed(double x1, double y1, double x2, double y2, double result)
        {
            var p1 = new Position(x1, y1, Position.GeoCoordinateDistance);
            var p2 = new Position(x2, y2, Position.GeoCoordinateDistance);
            
            Assert.Equal(result, Math.Round(p1.Distance(p2), CoreTests.ROUND_FIXED_FOR_DISTANCE));
        }


        // Graph.cs TESTS TO BE IMPLEMENTED
        // public void entropy()
        // {
        //     throw new NotImplementedException();
        // }

        // public void density()
        // {
        //     throw new NotImplementedException();
        // }

        // public void avgPathLenght()
        // {
        //     throw new NotImplementedException();
        // }

        // public void diameter()
        // {
        //     throw new NotImplementedException();
        // }

        // public void shortpath()
        // {
        //     throw new NotImplementedException();
        // }

        // public void graph_CRUD_operations()
        // {
        //     throw new NotImplementedException();
        // }

        // public void connectedComponent()
        // {
        //     throw new NotImplementedException();
        // }

        // public void biggestComponent()
        // {
        //     throw new NotImplementedException();
        // }

        // public void ShortestPathHeuristic()
        // {
        //     throw new NotImplementedException();
        // }
    }
}
