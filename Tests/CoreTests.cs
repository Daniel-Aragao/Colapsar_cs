using Core.models;
using System.Collections;
using System.Collections.Generic;

using System;
using Xunit;

namespace Tests
{
    public class CoreTests
    {   
        const double MinNormal = 2.2250738585072014E-308d;
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
        [InlineData(1, 0.166666667)]
        [InlineData(2, 0)]
        public void ReturnTheCorrectLocalClusterCoefficientGivenTheGraphAndNodeN1AndDirectedGraph(int gId, double result)
        {
            var coef = getGraph(gId).Nodes[1].GetLocalClusteringCoefficient(true);
            
            Assert.True(0.000001 > Math.Abs(coef - result));
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 0.333333333333333)]
        [InlineData(2, 0)]
        public void ReturnTheCorrectLocalClusterCoefficientGivenTheGraphAndNodeN1AndUnDirectedGraph(int gId, double result)
        {
            var coef = getGraph(gId).Nodes[1].GetLocalClusteringCoefficient(false);

            Assert.True(0.000001 > Math.Abs(coef - result));
            // Assert.Equal(result, coef);
        }
    }
}
