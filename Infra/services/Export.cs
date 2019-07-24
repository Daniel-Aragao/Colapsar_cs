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

        public static StreamWriter GetWriter(string fileName, string path=Constants.PATH_OUTPUTS)
        {
            return new StreamWriter(path + fileName);
        }

        // public static void WriteSearchOutput(string msg, string path=Constants.PATH_OUTPUTS)
        // {

        // }
    }
}