using Core.models;
using Infra.services;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using Xunit;

namespace Tests
{
    public class InfraTests
    {
        const double MIN_DIFFERENCE = 0.000001;
        const int ROUND_FIXED = 5;
        const int ROUND_FIXED_FOR_DISTANCE = 2;
        const string file_path = "/home/daniel/Documentos/Git/Colapsar_cs/Infra/misc/";

        Func<Graph>[] Gs;

        NodeEqualityComparer nodeEqualityComparer = new NodeEqualityComparer();

        public InfraTests()
        {
            Gs = new Func<Graph>[] { G1, G2 };
        }
        
        private Graph G1()
        {
            Graph g = new Graph("test graph 1 latitude, longetude");

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

        private Graph G2()
        {
            Graph g = new Graph("test graph 2 x,y");

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

    }
}
