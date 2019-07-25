using System;
using System.Collections.Generic;

using Core;
using Core.models;
using Infra.services;
using Infra.services.log;
using Infra.services.regions;

namespace Infra.services.multithread
{
    public class ThreadManager
    {
        private int ThreadsQuantity;
        private int FinishedThreads;
        private readonly object progressLock = new object();
        private int ODsSize;
        private int ODsRunned;
        private double Radius;
        private string OutputFileName;
        private ILogger _logger;
        private TimeSpan TotalTime;
        private Dictionary<EPathStatus, int> StatusCount;
        Graph graph;
        string StrategyName;
        
        public ThreadManager(Graph graph, int threadsQuantity, int ODsSize, double radius, string strategyName)
        {
            this.graph = graph;
            this.StrategyName = strategyName;
            this.ThreadsQuantity = threadsQuantity;
            this.ODsSize = ODsSize; 
            this.Radius = radius;
            this.ODsRunned = 0;

            this.StatusCount = new Dictionary<EPathStatus, int>();

            var now = DateTime.Now;

            this.OutputFileName = graph.Name;
            this.OutputFileName += Constants.SEPARATOR_FILE_NAMES + this.ODsSize.ToString();
            this.OutputFileName += Constants.SEPARATOR_FILE_NAMES + this.Radius.ToString();
            this.OutputFileName += Constants.SEPARATOR_FILE_NAMES + this.ThreadsQuantity;
            // this.FileName += Constants.SEPARATOR_FILE_NAMES + now.Year + "_" + now.Month + "_" + now.Day;
            // this.FileName += "_" + now.Hour + "_" + now.Minute + "_" + now.Second;

            this._logger = LoggerFactory.GetLogger();

            this._logger.WriteLine("Iniciando "+ StrategyName +":{\n\tGrafo:" + graph.Name + "\n\tThreads: " + this.ThreadsQuantity +
				"\n\tRaio: " + this.Radius + "\n\tODs: " + this.ODsSize + "\n}");
        }

        public void addProgress(double progress, IList<PathRoute> pathRoutes)
        {
            lock (progressLock)
            {
                var outputWriter = Export.GetWriter(this.OutputFileName);
                
                foreach (var pathRoute in pathRoutes)
                {
                    if(!this.StatusCount.ContainsKey(pathRoute.Status))
                    {
                        this.StatusCount.Add(pathRoute.Status, 0);
                    }

                    this.StatusCount[pathRoute.Status] = this.StatusCount[pathRoute.Status] + 1;

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
                this.TotalTime += totalTime;
                this.FinishedThreads += 1;

                this._logger.WriteLine("\t\tFim: " + "threadname");

                if(this.FinishedThreads == this.ThreadsQuantity)
                {

                    this._logger.WriteLine("\nQuantidade de rotas: " + this.ODsSize);

                    foreach(var keypair in this.StatusCount)
                    {
                        // keypair.Key keypair.value
                        this._logger.WriteLine("Status: " + keypair.Key + " Qtd.: " + keypair.Value);
                    }
                    this._logger.WriteLine("Tempo decorrido para "+ StrategyName +" para " + this.Radius
					+ " metros = " + this.TotalTime.TotalMinutes + "\n"
					+ "para o arquivo: " + this.graph.Name + "\n");
                    
                    var outputWriter = Export.GetWriter(this.OutputFileName);
                    outputWriter.Close();
                }
            }
        }
    }
}