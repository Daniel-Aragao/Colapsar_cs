using System;
using System.Collections.Generic;

using Core;
using Core.models;
using Infra.services;
using Infra.services.log;

namespace Infra.services.multithread
{
    public class ThreadManager
    {
        private int ThreadsQuantity;
        private readonly object progressLock = new object();
        private int ODsSize;
        private int ODsRunned;
        private double Radius;
        private string OutputFileName;
        private ILogger _logger;

        public ThreadManager(Graph graph, int threadsQuantity, int ODsSize, double radius)
        {
            this.ThreadsQuantity = threadsQuantity;
            this.ODsSize = ODsSize; 
            this.Radius = radius;
            this.ODsRunned = 0;

            var now = DateTime.Now;

            this.OutputFileName = graph.Name;
            this.OutputFileName += Constants.SEPARATOR_FILE_NAMES + this.ODsSize.ToString();
            this.OutputFileName += Constants.SEPARATOR_FILE_NAMES + this.Radius.ToString();
            this.OutputFileName += Constants.SEPARATOR_FILE_NAMES + this.ThreadsQuantity;
            // this.FileName += Constants.SEPARATOR_FILE_NAMES + now.Year + "_" + now.Month + "_" + now.Day;
            // this.FileName += "_" + now.Hour + "_" + now.Minute + "_" + now.Second;

            this._logger =LoggerFactory.GetLogger();

            this._logger.WriteLine("Iniciando Força Bruta:{\n\tGrafo:" + graph.Name + "\n\tThreads: " + this.ThreadsQuantity +
				"\n\tRaio: " + this.Radius + "\n\tODs: " + this.ODsSize + "\n}");
        }

        public void addProgress(double progress, IList<PathRoute> pathRoutes)
        {
            lock (progressLock)
            {
                var outputWriter = Export.GetWriter(this.OutputFileName);
                
                foreach (var pathRoute in pathRoutes)
                {
                    outputWriter.WriteLine(pathRoute.ToString());
                }

                outputWriter.Flush();

                this.ODsRunned += pathRoutes.Count;
                
                var logMessage = String.Format("Progresso ({0}): {1:0.00}%\tODs:{2}/{3}\tTempo: {4}", "threadname", progress, this.ODsRunned, this.ODsSize);
                this._logger.WriteLine(logMessage);
            }
        }

        public void endThread(TimeSpan totalTime)
        {
            lock (progressLock)
            {
                throw new NotImplementedException();
            }
        }
    }
}