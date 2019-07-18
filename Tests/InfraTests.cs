using Core.models;
using Infra.services;

using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace Tests
{
    public class InfraTests
    {
        const double MIN_DIFFERENCE = 0.000001;
        const int ROUND_FIXED = 5;
        const int ROUND_FIXED_FOR_DISTANCE = 2;
        public const string file_path = "misc/";
        // public const string file_path = "/home/daniel/Documentos/Git/Colapsar_cs/Tests/misc/";
        // (.*Colapsar_cs\/).*
        // public const string file_path = "/home/danielaragao/Documents/Git/Colapsar_cs/Tests/misc/";

        Func<Graph>[] Gs;

        NodeEqualityComparer nodeEqualityComparer = new NodeEqualityComparer();

        public InfraTests()
        {
            Gs = new Func<Graph>[] { G0, G1 };
        }
        
        private Graph G0()
        {
            Graph g = new Graph("test graph 0 latitude, longetude");

            g.CreateNode(245656627, "cross");
            g.CreateNode(4294951432, "cross");
            g.CreateNode(4294951452, "cross");
            g.CreateNode(4294951453, "cross");

            g.CreateEdge(4294951453, 4294951452, 72.14034314043423).PutAttribute("type_route", "driving");
            g.CreateEdge(4294951452, 4294951432, 154.2815560270423).PutAttribute("type_route", "driving");
            g.CreateEdge(4294951432, 245656627, 11.210694705547965).PutAttribute("type_route", "driving");
            g.CreateEdge(245656627, 4294951453, 80.210694705547965).PutAttribute("type_route", "driving");

            return g;
        }

        private Graph G1()
        {
            Graph g = new Graph("test graph 1 x,y");

            g.CreateNode(64,"sintetic");
            g.CreateNode(65,"sintetic");
            g.CreateNode(84,"sintetic");
            g.CreateNode(85,"sintetic");

            g.CreateEdge(64, 84, 1);
            g.CreateEdge(65, 85, 1);
            g.CreateEdge(84, 65, 1);
            g.CreateEdge(85, 64, 1);

            return g;
        }

        public Graph getGraph(int id){
            return Gs[id]();
        }

        [Fact]
        public void LoadCityFromText_TestGraph1()
        {
            Graph graphImported = Import.LoadCityFromText(file_path + "test_graph_1.txt");

            Assert.True(getGraph(0).Equals(graphImported));
        }
        
        [Fact]
        public void FailOnLoadCityFromText_TestGraph1()
        {
            Graph graphImported = Import.LoadCityFromText(file_path + "test_graph_1.txt");
            var g = getGraph(0);

            g.Edges[2].PutAttribute("type_route", 14);

            Assert.False(g.Equals(graphImported));
        }

        [Fact]
        public void LoadCityFromText_TestGraph2()
        {
            Graph graphImported = Import.LoadCityFromText(file_path + "test_graph_2.txt");

            Assert.True(getGraph(1).Equals(graphImported));
        }

        [Fact]
        public void FailOnLoadCityFromText_TestGraph2()
        {
            Graph graphImported = Import.LoadCityFromText(file_path + "test_graph_2.txt");
            var g = getGraph(1);

            g.Edges[2].PutAttribute("type_route", 14);

            Assert.False(g.Equals(graphImported));
        }

        [Fact]
        public void CollapseBucharestFromNorvigGraphCorrectlyWithRadius100()
        {
            Graph graph = Import.LoadCityFromText(InfraTests.file_path + "test_graph_3.norvig.txt");

            var bucharest = graph.getNodeByLabel("Bucharest");

            var superNode = Collapse.collapse(graph, bucharest, 100, -2, 3);
            
            Assert.Equal(-2, superNode.Id);
            Assert.Equal(3, superNode.Weight);

            var superNodeEdgesIn = (from edge in superNode.EdgesIn()
                                    select edge.Source.Id).ToList();

            var superNodeEdgesOut = (from edge in superNode.EdgesOut()
                                    select edge.Target.Id).ToList();
            
            superNodeEdgesIn.ForEach(Console.WriteLine);

            Assert.Equal(new long[] {8, 9, 13, 11, 12, 12, 14, 15}, superNodeEdgesIn);
            Assert.Equal(new long[] {8, 9, 11, 12, 13, 14, 15}, superNodeEdgesOut);

            // throw new NotImplementedException();
        }

        // collapse tests

    }
}
