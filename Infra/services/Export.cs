using System;
using System.IO;
// using System.Globalization;
using System.Collections.Generic;
using Core;

using Newtonsoft.Json;

// using Core;
// using Core.models;
// using Core.extensions;

namespace Infra.services
{
    public class Export
    {
        private static Dictionary<string, StreamWriter> writers = new Dictionary<string, StreamWriter>();

        public static StreamWriter GetWriter(string fileName, string path=Constants.PATH_OUTPUTS)
        {
            var filePath = path + fileName;

            if(Export.writers.ContainsKey(filePath))
            {
                return writers[filePath];
            }

            // if(!File.Exists(filePath))
            // {
            //     File.Create(filePath);
            // }

            Export.CreatePaths();

            var writer = new StreamWriter(filePath);

            Export.writers[filePath] = writer;

            return writer;
        }

        private static void CreatePaths()
        {
            if(!File.Exists(Constants.PATH_GRAPH))
            {                
                Directory.CreateDirectory(Constants.PATH_GRAPH);
            }

            if(!File.Exists(Constants.PATH_LOGS))
            {
                Directory.CreateDirectory(Constants.PATH_LOGS);
            }

            if(!File.Exists(Constants.PATH_ODs))
            {
                Directory.CreateDirectory(Constants.PATH_ODs);
            }

            if(!File.Exists(Constants.PATH_OUTPUTS))
            {
                Directory.CreateDirectory(Constants.PATH_OUTPUTS);
            }
        }

        public static void WriteLineSearchOutput(string msg, StreamWriter writer)
        {
            writer.WriteLine(msg);
        }

        public static void WriteSearchOutput(string msg, StreamWriter writer)
        {
            writer.Write(msg);
        }
    }
}