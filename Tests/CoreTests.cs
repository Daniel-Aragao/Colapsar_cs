using Core.models;
using Infra.services;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Xunit;

namespace Tests
{
    public class CoreTests
    {   
        const double MIN_DIFFERENCE = 0.000001;
        const int ROUND_FIXED = 5;
        const int ROUND_FIXED_FOR_DISTANCE = 2;

        Func<Graph>[] Gs;

        NodeEqualityComparer nodeEqualityComparer = new NodeEqualityComparer();

        public CoreTests()
        {
            Gs = new Func<Graph>[] { G0, G1, G2, G3 };
        }

        private Graph G0()
        {
            Graph g = new Graph("test graph 0");

            g.CreateNode(1, "n1");
            g.CreateNode(2, "n2");
            g.CreateNode(3, "n3");
            g.CreateNode(4, "n4");
            g.CreateEdge(1, 2);
            g.CreateEdge(1, 3);
            g.CreateEdge(1, 4);
            g.CreateEdge(2, 3);
            g.CreateEdge(2, 4);
            g.CreateEdge(3, 4);

            return g;
        }

        private Graph G1()
        {
            Graph g = new Graph("test graph 1");

            g.CreateNode(1, "n1");
            g.CreateNode(2, "n2");
            g.CreateNode(3, "n3");
            g.CreateNode(4, "n4");
            g.CreateEdge(1, 2);
            g.CreateEdge(1, 3);
            g.CreateEdge(1, 4);
            g.CreateEdge(3, 4);

            return g;
        }

        private Graph G2()
        {
            Graph g = new Graph("test graph 2");

            g.CreateNode(1, "n1");
            g.CreateNode(2, "n2");
            g.CreateNode(3, "n3");
            g.CreateNode(4, "n4");
            g.CreateEdge(1, 2);
            g.CreateEdge(1, 3);
            g.CreateEdge(1, 4);

            return g;
        }

        private Graph G3()
        {
            Graph g = new Graph("test graph 3");

            g.CreateNode(1, "n1");
            g.CreateNode(2, "n2");
            g.CreateNode(3, "n3");
            g.CreateNode(4, "n4");
            g.CreateEdge(1, 2).Label = "1_2";
            g.CreateEdge(1, 3).Label = "1_3";
            g.CreateEdge(1, 4).Label = "1_4";
            g.CreateEdge(2, 1).Label = "2_1";
            g.CreateEdge(2, 3).Label = "2_3";
            g.CreateEdge(2, 4).Label = "2_4";
            g.CreateEdge(3, 1).Label = "3_1";
            g.CreateEdge(3, 2).Label = "3_2";
            g.CreateEdge(3, 4).Label = "3_4";
            g.CreateEdge(4, 1).Label = "4_1";
            g.CreateEdge(4, 2).Label = "4_2";
            g.CreateEdge(4, 3).Label = "4_3";

            return g;
        }

        public Graph getGraph(int id){
            return Gs[id]();
        }

        [Theory]
        [InlineData(0,4)]
        [InlineData(1,4)]
        [InlineData(2,4)]
        [InlineData(3,4)]
        public void TheGivenGraphMustHaveTheGivenNodes(int gId, int nodeSize)
        {
            Assert.Equal(nodeSize, getGraph(gId).NodesSize);
        }

        [Theory]
        [InlineData(0,6)]
        [InlineData(1,4)]
        [InlineData(2,3)]
        [InlineData(3,12)]
        public void TheGivenGraphMustHaveTheGivenEdges(int gId, int edgesSize)
        {
            Assert.Equal(edgesSize, getGraph(gId).EdgesSize);
        }

        [Fact]
        public void RemoveSecondEdgeFromG0AndRemoveFromGraphAndNodes()
        {
            Graph g0_modified = new Graph("test graph 0");

            g0_modified.CreateNode(1, "n1");
            g0_modified.CreateNode(2, "n2");
            g0_modified.CreateNode(3, "n3");
            g0_modified.CreateNode(4, "n4");
            g0_modified.CreateEdge(1, 2);
            g0_modified.CreateEdge(1, 4);
            g0_modified.CreateEdge(2, 3);
            g0_modified.CreateEdge(2, 4);
            g0_modified.CreateEdge(3, 4);

            var g0 = this.G0();
            var edge = g0.GetEdgeByIndex(1);

            g0.RemoveEdge(edge);
            

            Assert.True(g0_modified.Equals(g0));
        }

        [Fact]
        public void RemoveEdgeFromEdge2ToEdge3FromG0AndRemoveFromGraphAndNodes()
        {
            Graph g0_modified = new Graph("test graph 0");

            g0_modified.CreateNode(1, "n1");
            g0_modified.CreateNode(2, "n2");
            g0_modified.CreateNode(3, "n3");
            g0_modified.CreateNode(4, "n4");
            g0_modified.CreateEdge(1, 2);
            g0_modified.CreateEdge(1, 3);
            g0_modified.CreateEdge(1, 4);
            g0_modified.CreateEdge(2, 4);
            g0_modified.CreateEdge(3, 4);

            var g0 = this.G0();
            var n2 = g0.GetNodeById(2);
            var n3 = g0.GetNodeById(3);

            g0.RemoveEdges(n2, n3);

            Assert.True(g0_modified.Equals(g0));
        }

        [Fact]
        public void RemoveThirdNodeFromG3AndRemoveFromGraphAndEdges()
        {
            Graph g3_modified = new Graph("test graph 3");

            g3_modified.CreateNode(1, "n1");
            g3_modified.CreateNode(2, "n2");
            g3_modified.CreateNode(4, "n4");
            g3_modified.CreateEdge(1, 2);
            g3_modified.CreateEdge(1, 4);
            g3_modified.CreateEdge(2, 1);
            g3_modified.CreateEdge(2, 4);
            g3_modified.CreateEdge(4, 1);
            g3_modified.CreateEdge(4, 2);
            
            var g3 = this.G3();
            var n3 = g3.GetNodeById(3);

            g3.RemoveNode(n3);

            Assert.Equal(g3_modified.NodesSize, g3.NodesSize);
            Assert.Equal(g3_modified.EdgesSize, g3.EdgesSize);
            Assert.True(g3_modified.Equals(g3));
        }

        [Fact]
        public void TheGraph0MustReturnTheCorrectsNodesFromN2Neighbors()
        {
            Graph g = this.G0();
            IList<Node> correctNodes = new List<Node>();
            correctNodes.Add(g.GetNodeById(1));
            correctNodes.Add(g.GetNodeById(3));
            correctNodes.Add(g.GetNodeById(4));

            var nodes = g.GetNodeById(2).Neighbors();

            Assert.Equal(correctNodes, nodes, nodeEqualityComparer);
        }

        [Fact]
        public void TheGraph1MustReturnTheCorrectsNodesFromN3Neighbors()
        {
            Graph g = this.G1();
            IList<Node> correctNodes = new List<Node>();
            correctNodes.Add(g.GetNodeById(1));
            correctNodes.Add(g.GetNodeById(4));

            var nodes = g.GetNodeById(3).Neighbors();

            Assert.Equal(correctNodes, nodes, nodeEqualityComparer);
        }

        [Fact]
        public void TheGraph1MustReturnTheCorrectsNodesFromN4Neighbors()
        {
            Graph g = this.G1();
            IList<Node> correctNodes = new List<Node>();
            correctNodes.Add(g.GetNodeById(1));
            correctNodes.Add(g.GetNodeById(3));

            var nodes = g.GetNodeById(4).Neighbors();

            Assert.Equal(correctNodes, nodes, nodeEqualityComparer);
            Assert.Equal(2, nodes.Count);
        }

        [Fact]
        public void InGraph0TheNode1MustReturnTheCorrectEdgeNumberIn_EdgesWhenSourceOfTarget()
        {
            Graph g = G0();
            var target = g.GetNodeById(2);
            var edges =  g.GetNodeById(1).EdgesWhenSourceOf(target);

            Assert.Equal(1, edges.Count);

            foreach(Edge edge in edges)
            {
                Assert.Equal(edge.Source, g.GetNodeById(1));
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
        [InlineData(3, 1, 3)]
        [InlineData(3, 2, 3)]
        [InlineData(3, 3, 3)]
        [InlineData(3, 4, 3)]
        public void ReturnTheCorrectAmountOfNeihboursForTheGivenGraphAndGivenNode(int gId, int nId, double result)
        {
            Assert.Equal(result, getGraph(gId).GetNodeById(nId).Neighbors().Count);
        }

        [Theory]
        [InlineData(0, 0.5)]
        [InlineData(1, 0.16667)]
        [InlineData(2, 0)]
        [InlineData(3, 1)]
        public void ReturnTheCorrectLocalClusterCoefficientGivenTheGraphAndNodeN1AndDirectedGraph(int gId, double result)
        {
            var coef = getGraph(gId).GetNodeById(1).GetLocalClusteringCoefficient();
            
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
        [InlineData(3, 0)]
        public void ReturnTheCorrectShannonsEntropyForTheGivenGraph(int gId, double result)
        {
            var entropy = getGraph(gId).Entropy();

            Assert.Equal(result, Math.Round(entropy, CoreTests.ROUND_FIXED));
        }

        [Fact]
        public void ForTheGivenPathReturnTheCorrectNodesForGraph0()
        {
            var g = G0();
            
            Edge[] edges = new Edge[3];
            edges[0] = g.GetNodeById(1).EdgesWhenSourceOf(g.GetNodeById(2))[0];
            edges[1] = g.GetNodeById(2).EdgesWhenSourceOf(g.GetNodeById(3))[0];
            edges[2] = g.GetNodeById(3).EdgesWhenSourceOf(g.GetNodeById(4))[0];

            Node[] nodes = new Node[] { g.GetNodeById(1), g.GetNodeById(2), g.GetNodeById(3), g.GetNodeById(4)};

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

        [Theory]
        [InlineData(0, 0.5)]
        [InlineData(1, 0.33333)]
        [InlineData(2, 0.25)]
        [InlineData(3, 1)]
        public void ReturnTheCorrectSDensityForTheGivenGraph(int gId, double result)
        {
            var density = getGraph(gId).Density();

            Assert.Equal(result, Math.Round(density, CoreTests.ROUND_FIXED));
        }

        [Fact]
        public void ShortestPathBetwenAradAndBucharestMustBe418km()
        {
            // to bucharest from arad running 418km (arad > sibiu > rimnicu vilcea > pitesti > bucharest)

            Graph graph = Import.LoadCityFromText(InfraTests.file_path + "test_graph_3.norvig.txt");
            
            var arad = graph.GetNodeByLabel("Arad");
            var sibiu = graph.GetNodeByLabel("Sibiu");
            var rimnicuVilcea = graph.GetNodeByLabel("Rimnicu Vilcea");
            var pitesti = graph.GetNodeByLabel("Pitesti");
            var bucharest = graph.GetNodeByLabel("Bucharest");

            var route = Graph.ShortestPathHeuristic(arad, bucharest);

            Assert.Equal(EPathStatus.Found, route.Status);
            Assert.Equal(new Node[] {arad, sibiu, rimnicuVilcea, pitesti, bucharest}, route.Nodes);
            Assert.Equal(418, route.Distance);
        }

        [Fact]
        public void ShortestPathBetwen5729And2500MustBe15056_65()
        {
            // to bucharest from arad running 418km (arad > sibiu > rimnicu vilcea > pitesti > bucharest)

            Graph graph = Import.LoadCityFromText(InfraTests.file_path + "test_graph_4.bus-network.txt");
            
            var p_5729 = graph.GetNodeById(5729);
            var p_2500 = graph.GetNodeById(2500);

            var route = Graph.ShortestPathHeuristic(p_5729, p_2500);

            IEnumerable<long> expected = new long[] {2500, 2498, 2502, 2503, 2504, 2496, 2499, 2491, 2485, 2479, 2472, 2450, 2446, 2852, 5225, 4389, 4018, 2413, 5553, 2829, 2830, 2823, 2835, 2836, 2821, 2914, 2915, 2912, 2922, 3875, 2899, 2901, 1646, 1642, 1622, 1629, 1630, 1633, 0183, 0176, 0175, 0179, 0445, 0424, 0426, 5735, 5734, 5741, 5733, 5732, 5731, 5730, 5729};
            expected = expected.Reverse();

            var result = new List<Node>(route.Nodes);

            var resultLong = from r in result select r.Id;

            Assert.Equal(EPathStatus.Found, route.Status);
            Assert.Equal(expected, resultLong);
            Assert.Equal(15056.65, Math.Round(route.Distance, ROUND_FIXED_FOR_DISTANCE));
        }
        [Fact]
        public void ShortestPathBetwenSmallWorld5x5From4And21MustBe5()
        {
            Graph graph = Import.LoadCityFromText(InfraTests.file_path + "SmallWorldGraph_5_5_0.05_2019_7_12_17_35_12.txt");
            
            var p_4 =  graph.GetNodeById(4);
            var p_21 = graph.GetNodeById(21);

            var route = Graph.ShortestPathHeuristic(p_4, p_21);

            IEnumerable<long> expected = new long[] {4, 3, 12, 17, 16, 21};

            var result = new List<Node>(route.Nodes);

            var resultLong = from r in result select r.Id;

            Assert.Equal(EPathStatus.Found, route.Status);
            Assert.Equal(expected, resultLong);
            Assert.Equal(5, Math.Round(route.Distance, ROUND_FIXED_FOR_DISTANCE));
        }

        [Fact]
        public void ShortestPathBetwenSmallWorld5x5From21And4MustBe5()
        {
            Graph graph = Import.LoadCityFromText(InfraTests.file_path + "SmallWorldGraph_5_5_0.05_2019_7_12_17_35_12.txt");
            
            var p_4 =  graph.GetNodeById(4);
            var p_21 = graph.GetNodeById(21);

            var route = Graph.ShortestPathHeuristic(p_21, p_4);

            IEnumerable<long> expected = new long[] {21, 16, 17, 12, 3, 4};

            var result = new List<Node>(route.Nodes);

            var resultLong = from r in result select r.Id;

            Assert.Equal(EPathStatus.Found, route.Status);
            Assert.Equal(expected, resultLong);
            Assert.Equal(5, Math.Round(route.Distance, ROUND_FIXED_FOR_DISTANCE));
        }

        [Fact]
        public void ShortestPathBetwenRegular5x5From8And20MustBe6()
        {
            Graph graph = Import.LoadCityFromText(InfraTests.file_path + "RegularGraph_5_5_2019_7_12_17_34_57.txt");
            
            var p_8 =  graph.GetNodeById(8);
            var p_20 = graph.GetNodeById(20);

            var route = Graph.ShortestPathHeuristic(p_8, p_20);

            IEnumerable<long> expected = new long[] {8, 7, 12, 17, 16, 21, 20};

            var result = new List<Node>(route.Nodes);

            var resultLong = (from r in result select r.Id).ToList();

            
            Assert.Equal(EPathStatus.Found, route.Status);
            Assert.Equal(6, Math.Round(route.Distance, ROUND_FIXED_FOR_DISTANCE));
            Assert.Equal(expected, resultLong);
        }

        [Fact]
        public void ShortestPathBetwenRegular5x5From20And8MustBe6()
        {
            Graph graph = Import.LoadCityFromText(InfraTests.file_path + "RegularGraph_5_5_2019_7_12_17_34_57.txt");
            
            var p_8 =  graph.GetNodeById(8);
            var p_20 = graph.GetNodeById(20);

            var route = Graph.ShortestPathHeuristic(p_20, p_8);

            IEnumerable<long> expected = new long[] {20, 21, 16, 11, 12, 13, 8};

            var result = new List<Node>(route.Nodes);

            var resultLong = from r in result select r.Id;
            
            Assert.Equal(EPathStatus.Found, route.Status);
            Assert.Equal(6, Math.Round(route.Distance, ROUND_FIXED_FOR_DISTANCE));
            Assert.Equal(expected, resultLong);
        }

        [Fact]
        public void Diameter8ForRegular5x5Graph()
        {
            Graph graph = Import.LoadCityFromText(InfraTests.file_path + "RegularGraph_5_5_2019_7_12_17_34_57.txt");

            var route = graph.Diamater();

            IEnumerable<long> expected = new long[] {20, 21, 16, 11, 12, 13, 8, 9, 4};

            var result = new List<Node>(route.Nodes);

            var resultLong = from r in result select r.Id;
            
            Assert.Equal(EPathStatus.Found, route.Status);
            Assert.Equal(8, Math.Round(route.Distance, ROUND_FIXED_FOR_DISTANCE));
            Assert.Equal(expected, resultLong);
        }

        [Fact]
        public void Diameter8ForSmallWorld5x5Graph()
        {
            Graph graph = Import.LoadCityFromText(InfraTests.file_path + "SmallWorldGraph_5_5_0.05_2019_7_12_17_35_12.txt");

            var route = graph.Diamater();

            IEnumerable<long> expected = new long[] {9, 8, 13, 12, 17, 16, 15, 20};

            var result = new List<Node>(route.Nodes);

            var resultLong = from r in result select r.Id;
            
            Assert.Equal(EPathStatus.Found, route.Status);
            Assert.Equal(expected, resultLong);
            Assert.Equal(7, Math.Round(route.Distance, ROUND_FIXED_FOR_DISTANCE));
        }

        [Fact]
        public void ReturnTheCorrectAvgPathLengthForGraph5()
        {
        //Given
            Graph graph = Import.LoadCityFromText(InfraTests.file_path + "test_graph_5.avgplength.txt");
            var avgpl = graph.AveragePathLenght();

            Assert.Equal(1.33333, Math.Round(avgpl, ROUND_FIXED));
        }

        [Fact]
        public void ReturnTheCorrectAvgPathLengthForRegularGraph3x3()
        {
        //Given
            Graph graph = Import.LoadCityFromText(InfraTests.file_path + "RegularGraph_3_3_2019_7_15_16_56_22.txt");
            var avgpl = graph.AveragePathLenght();

            Assert.Equal(2, Math.Round(avgpl, ROUND_FIXED));
        }

        [Fact]
        public void GetTheCorrectEdgesInForBucharestInNorvigGraph()
        {
            Graph graph = Import.LoadCityFromText(InfraTests.file_path + "test_graph_3.norvig.txt");

            var bucharest = graph.GetNodeByLabel("Bucharest");

            var edgesIn = (from edge in bucharest.EdgesIn()
                                    select edge.Source.Id).ToList();

            Assert.Equal(new long[] {11, 12, 14, 15}, edgesIn);
        }

        [Fact]
        public void GetTheCorrectEdgesInForSibiuInNorvigGraph()
        {
            Graph graph = Import.LoadCityFromText(InfraTests.file_path + "test_graph_3.norvig.txt");

            var bucharest = graph.GetNodeByLabel("Sibiu");

            var edgesIn = (from edge in bucharest.EdgesIn()
                                    select edge.Source.Id).ToList();

            Assert.Equal(new long[] {1, 3, 9, 11}, edgesIn);
        }

        [Fact]
        public void GetTheCorrectEdgesOutForBucharestInNorvigGraph()
        {
            Graph graph = Import.LoadCityFromText(InfraTests.file_path + "test_graph_3.norvig.txt");

            var bucharest = graph.GetNodeByLabel("Bucharest");

            var edgesIn = (from edge in bucharest.EdgesOut()
                                    select edge.Target.Id).ToList();

            Assert.Equal(new long[] {11, 12, 14, 15}, edgesIn);
        }

        [Fact]
        public void GetTheCorrectEdgesOutForSibiuInNorvigGraph()
        {
            Graph graph = Import.LoadCityFromText(InfraTests.file_path + "test_graph_3.norvig.txt");

            var bucharest = graph.GetNodeByLabel("Sibiu");

            var edgesIn = (from edge in bucharest.EdgesOut()
                                    select edge.Target.Id).ToList();

            Assert.Equal(new long[] {1, 3, 9, 11}, edgesIn);
        }

        [Fact]
        public void GetTheCorrectNeighborsInForBucharestInNorvigGraph()
        {
            Graph graph = Import.LoadCityFromText(InfraTests.file_path + "test_graph_4.bus-network.txt");
            var p2163 = graph.GetNodeById(2163);

            p2163.NeighborsIn();
            var edgesIn = (from edge in p2163.EdgesIn()
                                    select edge.Source.Id).ToList();

            Assert.Equal(new long[] {2165, 2522}, edgesIn);
        }

        [Fact]
        public void GetTheCorrectNeighborsOutForBucharestInNorvigGraph()
        {
            Graph graph = Import.LoadCityFromText(InfraTests.file_path + "test_graph_4.bus-network.txt");
            var p2162 = graph.GetNodeById(2162);

            p2162.NeighborsIn();
            var edgesIn = (from edge in p2162.EdgesOut()
                                    select edge.Target.Id).ToList();

            Assert.Equal(new long[] {2529, 6236}, edgesIn);
        }

        [Fact]

        public void  CorrectlyBuildPathRouteFromEdges_1To3_3To2_2To4()
        {
            Graph g = getGraph(3);

            var p_1 = g.GetNodeById(1);
            var p_3 = g.GetNodeById(3);
            var p_2 = g.GetNodeById(2);
            var p_4 = g.GetNodeById(4);

            var e_1_3 = g.GetEdgeByLabel("1_3");
            var e_3_2 = g.GetEdgeByLabel("3_2");
            var e_2_4 = g.GetEdgeByLabel("2_4");

            var edges = new Edge[] {e_1_3, e_3_2, e_2_4};

            var pathRoute = new PathRoute(edges, 3, EPathStatus.Found);

            Assert.Equal(new Node[]{p_1, p_3, p_2, p_4}, pathRoute.Nodes);
            Assert.Equal(3, pathRoute.Jumps);
            Assert.Equal(EPathStatus.Found, pathRoute.Status);
        }

        [Fact]

        public void  CorrectlyBuildPathRouteFromEdges_1To3()
        {
            Graph g = getGraph(3);

            var p_1 = g.GetNodeById(1);
            var p_3 = g.GetNodeById(3);

            var e_1_3 = g.GetEdgeByLabel("1_3");

            var edges = new Edge[] {e_1_3};

            var pathRoute = new PathRoute(edges, 1, EPathStatus.Found);

            Assert.Equal(new Node[]{p_1, p_3}, pathRoute.Nodes);
            Assert.Equal(1, pathRoute.Jumps);
            Assert.Equal(EPathStatus.Found, pathRoute.Status);
        }

        [Fact]
        public void CorrectlyBuildPathRouteFromEdges_AradToSibiu()
        {
            // to bucharest from arad running 418km (arad > sibiu > rimnicu vilcea > pitesti > bucharest)

            Graph graph = Import.LoadCityFromText(InfraTests.file_path + "test_graph_3.norvig.txt");
            
            var arad = graph.GetNodeByLabel("Arad");
            var sibiu = graph.GetNodeByLabel("Sibiu");
            var rimnicuVilcea = graph.GetNodeByLabel("Rimnicu Vilcea");
            var pitesti = graph.GetNodeByLabel("Pitesti");
            var bucharest = graph.GetNodeByLabel("Bucharest");

            var edge_arad_sibiu = graph.GetEdgeByLabel("arad_sibiu");
            var edge_sibiu_rimnicu_vilcea = graph.GetEdgeByLabel("sibiu_rimnicu_vilcea");
            var edge_rimnicu_vilcea_pitesti = graph.GetEdgeByLabel("rimnicu_vilcea_pitesti");
            var edge_pitesti_bucharest = graph.GetEdgeByLabel("pitesti_bucharest");


            var edges = new Edge[] {edge_arad_sibiu, edge_sibiu_rimnicu_vilcea, edge_rimnicu_vilcea_pitesti, edge_pitesti_bucharest};

            var pathRoute = new PathRoute(edges, 1, EPathStatus.Found);

            // pathRoute.Nodes.ToList().ForEach(Console.WriteLine);

            Assert.Equal(EPathStatus.Found, pathRoute.Status);
            Assert.Equal(new Node[]{arad, sibiu, rimnicuVilcea, pitesti, bucharest}, pathRoute.Nodes);
            Assert.Equal(4, pathRoute.Jumps);
        }

        // Graph.cs TESTS TO BE IMPLEMENTED

        // public void connectedComponent()
        // {
        //     throw new NotImplementedException();
        // }

        // public void biggestComponent()
        // {
        //     throw new NotImplementedException();
        // }
    }
}
