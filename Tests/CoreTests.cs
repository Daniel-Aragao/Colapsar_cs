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
        [InlineData(50.0663800, 5.71472, 58.64389, 3.07000, 996179.04)]
        [InlineData(50.0663800, 5.71472, 50.0663800, 5.71472, 0)]
        [InlineData(58.64389, 3.07000, 58.64389, 3.07000, 0)]
        [InlineData(139.74477, 35.6544, 39.8261, 21.4225, 9488844.54)]
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
        public void ShortestPathInFortalezaFrom2858142596And1704432085()
        {
            Graph graph = Import.LoadCityFromText(InfraTests.file_path + "Fortaleza-network-osm-2018-1_1.txt");
            
            var p_2858142596 = graph.GetNodeById(2858142596);
            var p_1704432085 = graph.GetNodeById(1704432085);;

            var route = Graph.ShortestPathHeuristic(p_2858142596, p_1704432085);

            IEnumerable<long> expected = new long[] {2858142596,2858142599,2858142600,2858141707,2518367027,2858139697,2858141705,2518367026,2504538926,2858139689,5047127603,264954716,264954710,622541314,2794860284,622541381,4440651247,2504538942,622541186,253390958,253390959,253390982,3816414754,622541201,622541204,253390882,253390883,253390884,253390885,1974309513,2283197355,253390888,2283197354,253390889,253390890,2281462076,253390891,2281462073,253390793,2281461454,253390792,2281462078,2281461449,2504592750,253390901,253390791,2422683832,622541286,622541279,2856243310,253390790,253390789,2504592747,2508980113,2504592748,2274590442,2274590441,253390787,2856243323,253390786,2422698131,253390784,263929677,2274590371,253390783,2274590369,253390782,263929679,2274590368,4680736322,253390780,2274590366,2274590367,1898402929,1898394257,2722597796,1897534771,5112788401,2504583277,1898385354,2422719329,1898385358,1897551876,1897524342,2722597792,2274588153,1897523099,1897519757,1897519759,1728558960,2422729047,5357370145,253390826,2279071315,2279071316,253390827,2279071312,1728558949,2279071304,253390828,2279071305,253390829,2279071308,253390830,2069267712,1728558961,263929607,2069267728,263929610,2504594610,2069267684,2069267734,2069267680,2069267709,2069267683,628524625,628524956,263929642,628524959,265639841,628524937,628524877,628524880,628524882,628524885,628524887,628525276,628524683,628524061,628524062,628524063,5220036901,4411522006,4411522007,4411522008,4411522009,4411522010,4411522011,3761441152,3761447359,4109056605,1703094272,4228511567,4228511566,4438994430,4438994429,2240318958,2269598286,3702928167,2269598285,1663130727,1703057047,1703057063,1663130734,3702970087,3702970093,5226479524,1703057140,1703057170,5226479525,1663130757,3978750937,1663130760,1703057221,1703057230,5226478989,1703057233,1663130779,1663130782,1703057244,2874531014,2280381058,1703057254,1703057262,1663130803,1663130815,1663130823,1703108207,1663130826,1703057282,1663130837,1703057286,1703057300,1703057307,1703057312,2874685292,2050384904,2874685291,2050384738,2050384997,2874685296,1703108413,2050384898,1703108421,5241285962,5241285964,2050385065,1663130908,1703108428,2874692839,1703108459,1703547889,1703108481,1703108487,5381458497,1703108490,1703108493,1703108495,1703108505,1703547893,2874693550,1704450223,1703547939,1703547946,1703547967,1703548021,5331865643,5331865642,1703548075,1704431780,1704431819,1704431822,1704431831,2875041457,1704431861,2876163395,1704431877,2876163387,2876163385,1704431923,1704431958,1704431967,1704431987,1704432000,1704432032,1704432059,1704432105};

            var result = new List<Node>(route.Nodes);

            var resultLong = from r in result select r.Id;
            
            Assert.Equal(EPathStatus.Found, route.Status);
            Assert.Equal(Math.Round(15502.30718, ROUND_FIXED_FOR_DISTANCE), Math.Round(route.Distance, ROUND_FIXED_FOR_DISTANCE));
            Assert.Equal(expected, resultLong);
        }

        [Fact]
        public void ShortestPathInFortalezaFrom1714788084And2511576184()
        {
            // 1714788084,2511576184,478,0,13792,27856.44260411505,1714788084;1714788098;1714788156;1714788186;1714788190;2822782546;2822782539;1714788203;2822753032;2822753034;2822753036;2822753040;2822753044;2822754819;2822754809;2822754810;2822755240;2822771638;2822754827;2822754828;2822754829;2822754830;2822754831;2822754832;2822771678;2822754834;2822754836;2822754839;2822754840;2822761838;1302847928;1289741991;1302847993;4342952121;264899072;264898998;264898950;264899030;1295782598;1295782609;1295782802;1295783221;1295783326;1295782871;1295783228;1295782903;1295783878;1295783073;1295783591;3920955061;2844119133;2844119139;1295782724;1295783867;1295783283;1295782828;1295783850;1295783676;1295783147;2844122702;2844122703;1703402968;2844130245;1295783309;1275276121;1703403018;1275276089;1295783151;1295782962;4402735870;1295782604;1703403055;1703403071;1703403104;1703403126;1275276075;1295783585;1295783610;1295783707;1295782912;1295782558;2722597767;1275276155;2845981150;2845981153;2845981156;1703074849;1703074854;1703074857;1703074859;1703074862;2426576820;2426576823;1288548242;1288548239;2426576822;2845981138;2240620131;1295782798;1288548243;2722597781;1295783055;1288548273;1288548288;1288548251;2240350382;1288548180;1703056179;1288548390;1703056184;2240593328;1703056188;1288548344;1288548403;1288548149;1288548191;1288548189;1288548234;1288548410;1288548435;2240564118;1903563938;1275276030;2265431788;2245140034;2265496576;2265431789;2265431790;2265431791;1275276097;2267211871;1275276033;1275276150;264943731;1243889540;264943738;4228511566;4438994430;4109056608;3761441141;264943742;3761447360;4459614291;2017571226;2266561288;2017571262;2017571460;2017571423;2017571240;2017571399;2017571486;2017571349;2017571238;264943711;2017571492;2017571214;2017571484;264943703;2017571231;4202768702;1702929086;1895469170;2017571474;1895463725;2017571538;2017571260;1895459268;1895459604;2283272572;264943646;2017571290;2017571455;2017571389;2017571397;2017571237;2017571208;5137545212;1892629052;2017571429;1895442677;1895446688;1895443017;2017571425;3248751592;2048222686;4323507273;263929433;5137545207;2854224088;1892629051;4475941077;2854224084;5137545206;1892628960;2854224073;2854224071;2854224069;2854224067;2854158069;2048191378;1702857789;264943558;1892629053;264943561;2048191373;1702857802;2048208675;2854153713;4396343177;2048208718;5166206490;4396343175;264943569;2048208674;2048208669;2048208693;2048208716;2048208654;2048208642;2048208657;2048208721;2048208699;2048208698;2048208705;2048208715;1892629049;2856166591;5347995589;1892629113;1893510726;2506650018;4396343174;1892629066;1892629011;1892629266;1892629014;2506649997;2722597822;2506650009;2408832958;2283276809;2653823631;4434120659;3694547097;3694547039;3694547073;3694547032;3694547038;3694547071;3694547070;3694547063;3694546524;3694547056;3694547026;3694547033;3694547053;3694547072;3694547096;3694547075;3694547068;3694546522;3694547089;3694547084;3694547095;3694547083;2506608712;2506649994;2653726374;2506608713;2653726376;2506608714;4396289670;264955114;264955115;2506630952;2506630955;2914812697;264955118;264955119;2653761767;4396288744;2653765728;264955122;2281386296;264955125;264955126;264955127;1704730609;2281386299;4952722181;2858074948;1704730639;2653765747;1704730704;2858074979;2858074984;2653765755;1704730728;2858074991;1704730946;4952722179;1704731009;1704731012;1704731079;1704731105;1704731221;1704731311;1704731326;1704731393;1704731481;2654185540;2654185542;1704731688;1704731735;2858063628;5202432487;2858051993;1704732216;1704732227;1704732379;1704732454;1704732472;2858046927;5371262019;2623048753;2623048754;2623048758;3240196646;2521279625;2521253404;4389392797;2506608621;2506608591;2506546974;5202462860;2506546971;5202462881;5368276552;2506546978;2506546980;1704819483;2503390900;2506546984;1704819485;1668155047;2527301706;2527301707;2527301705;1704819487;1704819518;2527301708;1326628342;1326628361;1327035249;1326628346;2527329245;1326628356;2527329219;2527329181;2503390904;2503786511;2527329273;2503786510;2527329231;2860014192;2503786509;2503786506;2527301701;1704819355;2503786461;1704819353;2527349224;2503786489;2503786496;2527349232;2503786503;2503786505;2503786504;582268035;582268058;2503786516;4424927525;2503798904;582268057;3990984444;2885460760;1704862792;582268056;3990997663;2885460765;1704862758;2860536327;2885460770;1704862702;2885460779;1704862693;2860536332;2885460787;2511560574;2860419526;622415315;2860419542;2860418846;622394186;622394236;1704862566;622394225;1704862563;2860415124;1704862545;1704862543;2860414111;2860414113;1704862536;1704862530;3233731258;1704862516;1704862513;2860407853;1704862498;1704862482;1704862476;1704862467;2860407842;1704862444;2860407836;2511560573;2860407830;1704862422;2860407824;2511560555;3772127067;2511560556;2511567230;2511560557;3789269033;3789269042;2511567232;2511560558;2511560559;2511567233;2511560560;2511560561;2511567228;3772127069;2511576189;2511576157;2511576158;2860338365;2511576159;4951313520;2511576161;2511576162;2511576163;2511576164;4951315235;4951315233;4951315238;2511576165;4951315229;2511576166;4951315242;4951315240;4951315241;2511576167;4951315248;2511576168;2511576176;2511576177;4951315257;2511576178;2511576179;4951315259;2511576180;2511576181;2511576182;2511576175;2511576183;2511576184
            Graph graph = Import.LoadCityFromText(InfraTests.file_path + "Fortaleza-network-osm-2018-1_1.txt");
            
            var p_1714788084 = graph.GetNodeById(1714788084);
            var p_2511576184 = graph.GetNodeById(2511576184);

            var route = Graph.ShortestPathHeuristic(p_1714788084, p_2511576184);

            IEnumerable<long> expected = new long[] {1714788084,1714788098,1714788156,1714788186,1714788190,2822782546,2822782539,1714788203,2822753032,2822753034,2822753036,2822753040,2822753044,2822754819,2822754809,2822754810,2822755240,2822771638,2822754827,2822754828,2822754829,2822754830,2822754831,2822754832,2822771678,2822754834,2822754836,2822754839,2822754840,2822761838,1302847928,1289741991,1302847993,4342952121,264899072,264898998,264898950,264899030,1295782598,1295782609,1295782802,1295783221,1295783326,1295782871,1295783228,1295782903,1295783878,1295783073,1295783591,3920955061,2844119133,2844119139,1295782724,1295783867,1295783283,1295782828,1295783850,1295783676,1295783147,2844122702,2844122703,1703402968,2844130245,1295783309,1275276121,1703403018,1275276089,1295783151,1295782962,4402735870,1295782604,1703403055,1703403071,1703403104,1703403126,1275276075,1295783585,1295783610,1295783707,1295782912,1295782558,2722597767,1275276155,2845981150,2845981153,2845981156,1703074849,1703074854,1703074857,1703074859,1703074862,2426576820,2426576823,1288548242,1288548239,2426576822,2845981138,2240620131,1295782798,1288548243,2722597781,1295783055,1288548273,1288548288,1288548251,2240350382,1288548180,1703056179,1288548390,1703056184,2240593328,1703056188,1288548344,1288548403,1288548149,1288548191,1288548189,1288548234,1288548410,1288548435,2240564118,1903563938,1275276030,2265431788,2245140034,2265496576,2265431789,2265431790,2265431791,1275276097,2267211871,1275276033,1275276150,264943731,1243889540,264943738,4228511566,4438994430,4109056608,3761441141,264943742,3761447360,4459614291,2017571226,2266561288,2017571262,2017571460,2017571423,2017571240,2017571399,2017571486,2017571349,2017571238,264943711,2017571492,2017571214,2017571484,264943703,2017571231,4202768702,1702929086,1895469170,2017571474,1895463725,2017571538,2017571260,1895459268,1895459604,2283272572,264943646,2017571290,2017571455,2017571389,2017571397,2017571237,2017571208,5137545212,1892629052,2017571429,1895442677,1895446688,1895443017,2017571425,3248751592,2048222686,4323507273,263929433,5137545207,2854224088,1892629051,4475941077,2854224084,5137545206,1892628960,2854224073,2854224071,2854224069,2854224067,2854158069,2048191378,1702857789,264943558,1892629053,264943561,2048191373,1702857802,2048208675,2854153713,4396343177,2048208718,5166206490,4396343175,264943569,2048208674,2048208669,2048208693,2048208716,2048208654,2048208642,2048208657,2048208721,2048208699,2048208698,2048208705,2048208715,1892629049,2856166591,5347995589,1892629113,1893510726,2506650018,4396343174,1892629066,1892629011,1892629266,1892629014,2506649997,2722597822,2506650009,2408832958,2283276809,2653823631,4434120659,3694547097,3694547039,3694547073,3694547032,3694547038,3694547071,3694547070,3694547063,3694546524,3694547056,3694547026,3694547033,3694547053,3694547072,3694547096,3694547075,3694547068,3694546522,3694547089,3694547084,3694547095,3694547083,2506608712,2506649994,2653726374,2506608713,2653726376,2506608714,4396289670,264955114,264955115,2506630952,2506630955,2914812697,264955118,264955119,2653761767,4396288744,2653765728,264955122,2281386296,264955125,264955126,264955127,1704730609,2281386299,4952722181,2858074948,1704730639,2653765747,1704730704,2858074979,2858074984,2653765755,1704730728,2858074991,1704730946,4952722179,1704731009,1704731012,1704731079,1704731105,1704731221,1704731311,1704731326,1704731393,1704731481,2654185540,2654185542,1704731688,1704731735,2858063628,5202432487,2858051993,1704732216,1704732227,1704732379,1704732454,1704732472,2858046927,5371262019,2623048753,2623048754,2623048758,3240196646,2521279625,2521253404,4389392797,2506608621,2506608591,2506546974,5202462860,2506546971,5202462881,5368276552,2506546978,2506546980,1704819483,2503390900,2506546984,1704819485,1668155047,2527301706,2527301707,2527301705,1704819487,1704819518,2527301708,1326628342,1326628361,1327035249,1326628346,2527329245,1326628356,2527329219,2527329181,2503390904,2503786511,2527329273,2503786510,2527329231,2860014192,2503786509,2503786506,2527301701,1704819355,2503786461,1704819353,2527349224,2503786489,2503786496,2527349232,2503786503,2503786505,2503786504,582268035,582268058,2503786516,4424927525,2503798904,582268057,3990984444,2885460760,1704862792,582268056,3990997663,2885460765,1704862758,2860536327,2885460770,1704862702,2885460779,1704862693,2860536332,2885460787,2511560574,2860419526,622415315,2860419542,2860418846,622394186,622394236,1704862566,622394225,1704862563,2860415124,1704862545,1704862543,2860414111,2860414113,1704862536,1704862530,3233731258,1704862516,1704862513,2860407853,1704862498,1704862482,1704862476,1704862467,2860407842,1704862444,2860407836,2511560573,2860407830,1704862422,2860407824,2511560555,3772127067,2511560556,2511567230,2511560557,3789269033,3789269042,2511567232,2511560558,2511560559,2511567233,2511560560,2511560561,2511567228,3772127069,2511576189,2511576157,2511576158,2860338365,2511576159,4951313520,2511576161,2511576162,2511576163,2511576164,4951315235,4951315233,4951315238,2511576165,4951315229,2511576166,4951315242,4951315240,4951315241,2511576167,4951315248,2511576168,2511576176,2511576177,4951315257,2511576178,2511576179,4951315259,2511576180,2511576181,2511576182,2511576175,2511576183,2511576184};

            var result = new List<Node>(route.Nodes);

            var resultLong = from r in result select r.Id;
            
            Assert.Equal(EPathStatus.Found, route.Status);
            Assert.Equal(Math.Round(27856.44260, ROUND_FIXED_FOR_DISTANCE), Math.Round(route.Distance, ROUND_FIXED_FOR_DISTANCE));
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
