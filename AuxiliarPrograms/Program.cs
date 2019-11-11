using System;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;

using Core;
using Core.models;
using Infra.services;

namespace AuxiliarPrograms
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                throw new ArgumentException("You must inform the program to run");
            }

            var program = args[0];

            if (program == "compare_outputs" || program == "CO")
            {
                Compare.Comparer(args);
            }
            else if(program == "graph_info" || program == "GI")
            {
                GraphInfo.GetInfo(args);
            }
        }
    }
}
