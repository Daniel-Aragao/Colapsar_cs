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

        Graph[] Gs;

        NodeEqualityComparer nodeEqualityComparer = new NodeEqualityComparer();

        public InfraTests()
        {
            Graph G1 = new Graph("test graph 1 latitude, longetude");

            G1.CreateNode(245656627, "cross");
            G1.CreateNode(4294951432, "cross");
            G1.CreateNode(4294951452, "cross");
            G1.CreateNode(4294951453, "cross");

            G1.CreateEdge(4294951453, 4294951452, 72.14034314043423).PutAttribute("type_route", "driving");
            G1.CreateEdge(4294951452, 4294951432, 154.2815560270423).PutAttribute("type_route", "driving");
            G1.CreateEdge(4294951432, 245656627, 11.210694705547965).PutAttribute("type_route", "driving");
            G1.CreateEdge(245656627, 4294951453, 80.210694705547965).PutAttribute("type_route", "driving");

            Graph G2 = new Graph("test graph 2 x,y");

            G2.CreateNode(64,"sintetic");
            G2.CreateNode(65,"sintetic");
            G2.CreateNode(84,"sintetic");
            G2.CreateNode(85,"sintetic");

            G2.CreateEdge(64, 84, 1);
            G2.CreateEdge(65, 85, 1);
            G2.CreateEdge(84, 65, 1);
            G2.CreateEdge(85, 64, 1);

            Gs = new Graph[] { G1, G2};
        }

        public Graph getGraph(int id){
            return Gs[id];
        }

        [Fact]
        public void LoadCityFromText_TestGraph1()
        {
            Graph graphImported = Import.LoadCityFromText(file_path + "test_graph_1.txt");

            Assert.True(getGraph(0).Equals(graphImported));
        }

        [Fact]
        public void LoadCityFromText_TestGraph2()
        {
            Graph graphImported = Import.LoadCityFromText(file_path + "test_graph_2.txt");

            Assert.True(getGraph(1).Equals(graphImported));
        }

    }
}
