using System;

using Core;
using Infra.services;


namespace Infra.services.log
{
    public class LoggerFactory
    {
        private static ILogger _logger;

        public static ILogger Define(bool writeToFile, string programName)
        {
            if(writeToFile)
            {
                var now = DateTime.Now;

                var logFileName = "LOG-(" + Constants.SEPARATOR_FILE_NAMES + now.Year + "_" + now.Month + "_" + now.Day;
                logFileName += "_" + now.Hour + "_" + now.Minute + "_" + now.Second + ")-"+ programName +"-.txt";

                logFileName = Constants.PATH_LOGS + logFileName;

                return new ConsoleAndFileLogger(Export.GetWriter(logFileName));
            }

            return new ConsoleLogger();
        }

        public static ILogger GetLogger()
        {
            if(_logger == null)
            {
                throw new InvalidOperationException("You must define (LoggerFactory.Define) logger first");
            }

            return _logger;
        }
    }
}
