using System;

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
        public const char SEPARATOR_FILE_NAMES = '-';
        public const string FILE_NAMES_OD = "-ODs-";
        public const string FILE_EXTENSION_OD = ".txt";
        public const string FILE_EXTENSION_OUTPUT = ".txt";

        // VALUES
        public const bool LOG_TO_FILE = true;
        public const string ALGORITHMN_NAME_COLLAPSE = "Collapse";
        public const string ALGORITHMN_NAME_BRUTE_FORCE = "BruteForce";

    }
}