﻿using System;

using Core;
using Core.models;
using Infra.services;
using Infra.services.regions;
using Infra.services.multithread;

namespace SearchConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var defaultThreadNumber = Environment.ProcessorCount;            
            var file_path = Constants.PATH_GRAPH;
            var ods_path = Constants.PATH_ODs;

            string helpMessage = "====================> Region search program <====================\n";
            helpMessage += "\n==========> Description <==========\n";
            helpMessage += "\tThis program it's for research purpose and search the best path between two informed regions (set of nodes)\n";
            helpMessage += "\tusing the informed strategy and the graph also informed. It is also needed the radius to build the region and the number of\n";
            helpMessage += "\tpairs of origin and destinations to make data to the research. May also be informed the number of threads of execution\n";
            helpMessage += "\tto run the program (the default depends on the machine that is running this script)\n";
            helpMessage += "\n==========> Legend <==========\n";
            helpMessage += "\t1. None \"< or >\" must be used, they are just to delimiter parameters \n";
            helpMessage += "\t2. Arguments\n";
            helpMessage += "\t\t(*) <mandatory arguments>\n";
            helpMessage += "\t\t(#) <default arguments> (also optional) #default value\n";
            helpMessage += "\t\t( ) <optional arguments>\n";
            helpMessage += "\t\t    <any kind of argument> :type\n";
            helpMessage += "\t\t    <any kind of argument> (description)\n";
            helpMessage += "\t3. The order of the parameters must be followed (you can use # to avoid change default values or to ignore optional arguments)\n";
            helpMessage += "\n==========> Arguments <==========\n";
            helpMessage += "\t(*) <Strategy> : string (<C> to Collapse, <BF> to BruteForce)\n";
            helpMessage += "\t(*) <File path> : string\n";
            helpMessage += "\t(*) <Distance> :double (radius to search) \n";
            helpMessage += "\t(*) <OD size> :int (number of Origin and Destination to run)\n";
            helpMessage += "\t(#) <Number of threads to use> :int #"+ defaultThreadNumber +" (The number of logical processors in this machine)\n";
            helpMessage += "\t( ) <don't use default paths> :bool(<t> to true) (to use the path on the argument when searching for graph and OD pairs\n";


            if(args.Length == 1 && (args[0] == "-h" || args[0] == "--help"))
            {
                Console.WriteLine(helpMessage);
                return;
            }            
            else if (args.Length < 4 || args.Length > 6)
            {
                throw new ArgumentException("Must inform 4~6 argument must be passed\n" + helpMessage);
            }

            var argument = 0;

            string strategy = args[argument++];
            var path = args[argument++];
            var radius = double.Parse(args[argument++]);
            var OD = Int32.Parse(args[argument++]);

            if(args.Length >= argument + 1)
            {
                defaultThreadNumber = args[argument++] == "#"? defaultThreadNumber : Int32.Parse(args[3]);
            }

            if(args.Length >= argument + 1 && args[argument] == "t")
            {
                file_path = "";
                ods_path = "";
            }

            Graph graph = Import.LoadCityFromText(file_path + path);
            var ods = Import.LoadODsFromTxt(ods_path, OD);

            SearchStrategyFactory strategyFactory = null;

            var threadBuilder = new ThreadBuilder(graph, strategyFactory, ods, radius, defaultThreadNumber);
            
            threadBuilder.Begin();
        }
    }
}
//   <ItemGroup>
//     <ProjectReference Include="..\Core\Core.csproj" />
//     <ProjectReference Include="..\Infra\Infra.csproj" />
//   </ItemGroup>
