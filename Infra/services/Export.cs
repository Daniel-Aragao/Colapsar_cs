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

            if(!File.Exists(filePath))
            {
                File.Create(path);
            }

            var writer = new StreamWriter(filePath);

            Export.writers[filePath] = writer;

            return writer;
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