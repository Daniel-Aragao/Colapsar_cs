// using System;
using System.Collections.Generic;

namespace Core
{
    public class Constants
    {
        // PATHS
        public const string PATH_GRAPH = "./graphs/";
        public const string PATH_ODs = "./ODs/";
        public const string PATH_LOGS = "./logs/";
        public const string PATH_OUTPUTS = "./outputs/";

        // FILE NAMES AND EXTENSIONS
        public const char SEPARATOR_ODs = ',';
        public const char SEPARATOR_PATH_IDS = ';';
        public const char SEPARATOR_FILE_NAMES = '-';
        public const string FILE_NAMES_OD = "-ODs-";
        public const string FILE_EXTENSION_OD = ".txt";
        public const string FILE_EXTENSION_OUTPUT = ".txt";

        // VALUES
        public const bool LOG_TO_FILE = true;
        public const string ALGORITHMN_NAME_COLLAPSE = "Collapse";
        public const string ALGORITHMN_NAME_BRUTE_FORCE = "BruteForce";
        public const float DISTANCE_DIFFERENCE_THRESHOLD = 0.001f;

        // DICTIONARY
        public const string FIELD_ORDERING_COLAPSAR_CS_LEGEND = "origem,destino,status,saltos,expansões,tempo,distancia,rota";
        public const string FIELD_ORDERING_COLAPSAR_JAVA_LEGEND = "origem,destino,saltos,totalSaltos,tempo,distancia,rota";
        public static Dictionary<string,int> FIELD_ORDERING_COLAPSAR_CS = new Dictionary<string, int> ()
        {
            ["SourceId"] = 0,
            ["TargetId"] = 1,
            ["status"] = 2,
            ["Jumps"] = 3,
            ["QuantityOfExpansions"] = 4,
            ["DeltaTime"] = 5,
            ["Distance"] = 6
        };

        public static Dictionary<string,int> FIELD_ORDERING_COLAPSAR_JAVA = new Dictionary<string, int> ()
        {
            ["SourceId"] = 0,
            ["TargetId"] = 1,
            ["status"] = -1,
            ["Jumps"] = 2,
            ["QuantityOfExpansions"] = 3,
            ["DeltaTime"] = 4,
            ["Distance"] = 5
        };
    }
}