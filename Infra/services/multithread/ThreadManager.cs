using System;
using System.Threading;
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
        private EmailSender emailSender;
        private string logBody;
        private bool progressSendMail = true;

        private bool threadFail = false;

        public ThreadManager(Graph graph, int threadsQuantity, int ODsSize, double radius, string strategyName, EmailSender emailSender = null)
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
            this.OutputFileName += Constants.SEPARATOR_FILE_NAMES + strategyName;
            this.OutputFileName += Constants.SEPARATOR_FILE_NAMES + this.ODsSize.ToString();
            this.OutputFileName += Constants.SEPARATOR_FILE_NAMES + this.Radius.ToString();
            this.OutputFileName += Constants.SEPARATOR_FILE_NAMES + this.ThreadsQuantity.ToString();
            this.OutputFileName += Constants.FILE_EXTENSION_OUTPUT;

            var outputWriter = Export.GetWriter(this.OutputFileName);
            outputWriter.WriteLine(Constants.FIELD_ORDERING_COLAPSAR_CS_LEGEND);
            // this.FileName += Constants.SEPARATOR_FILE_NAMES + now.Year + "_" + now.Month + "_" + now.Day;
            // this.FileName += "_" + now.Hour + "_" + now.Minute + "_" + now.Second;

            this._logger = LoggerFactory.GetLogger();

            var subject = "Iniciando " + StrategyName;

            this.logBody = "{\n\tGrafo:" + graph.Name + "\n\tThreads: " + this.ThreadsQuantity +
                "\n\tRaio: " + this.Radius + "\n\tODs: " + this.ODsSize + "\n\tEstratégia:" + StrategyName + "\n}";

            this._logger.WriteLine(subject + ":" + logBody);

            if (emailSender != null)
            {
                emailSender.SendMail(logBody, subject);
                this.emailSender = emailSender;
            }
        }

        public void addProgress(float progress, IList<PathRoute> pathRoutes, TimeSpan threadTimeDelta)
        {
            lock (progressLock)
            {
                var outputWriter = Export.GetWriter(this.OutputFileName);

                foreach (var pathRoute in pathRoutes)
                {
                    if (!this.StatusCount.ContainsKey(pathRoute.Status))
                    {
                        this.StatusCount.Add(pathRoute.Status, 0);
                    }

                    this.StatusCount[pathRoute.Status] = this.StatusCount[pathRoute.Status] + 1;

                    outputWriter.WriteLine(pathRoute.ToString());
                }

                outputWriter.Flush();

                this.ODsRunned += pathRoutes.Count;

                var totalPercent = (((float)this.ODsRunned) / this.ODsSize) * 100;

                var logMessage = String.Format("Progresso ({0}): {1:0.00}%\tODs: {2}/{3}({4:000}%)\tTempo: {5:0.00000}", Thread.CurrentThread.Name, progress, this.ODsRunned, this.ODsSize, totalPercent, threadTimeDelta.TotalMinutes);
                this._logger.WriteLine(logMessage);

                if (this.emailSender != null)
                {
                    if (totalPercent >= 50 && progressSendMail)
                    {
                        emailSender.SendMail(this.logBody + "\n" + logMessage, "Progresso");
                        progressSendMail = false;
                    }
                }

                if (threadFail)
                {
                    throw new OperationCanceledException("Another thread aborted");
                }
            }
        }

        public void endThread(TimeSpan totalTime)
        {
            lock (progressLock)
            {
                this.TotalTime += totalTime;
                this.FinishedThreads += 1;

                this._logger.WriteLine("\t\tFim: " + Thread.CurrentThread.Name);

                if (this.FinishedThreads == this.ThreadsQuantity)
                {

                    this._logger.WriteLine("\nQuantidade de rotas: " + this.ODsSize);

                    foreach (var keypair in this.StatusCount)
                    {
                        // keypair.Key keypair.value
                        this._logger.WriteLine("Status: " + keypair.Key + " Qtd.: " + keypair.Value);
                    }

                    var endString = "Tempo decorrido para " + StrategyName + " para " + this.Radius
                    + " metros = " + this.TotalTime.TotalMinutes + "\n";

                    this._logger.WriteLine(endString + "para o arquivo: " + this.graph.Name + "\n");

                    if (this.emailSender != null)
                    {
                        this.emailSender.SendMail(this.logBody + "\n" + endString, "FIM " + this.graph.Name + " ODs" + this.ODsSize);
                    }

                    var outputWriter = Export.GetWriter(this.OutputFileName);
                    outputWriter.Close();
                }
            }
        }

        public void errorThread(Exception e)
        {
            if (!(e is OperationCanceledException))
            {
                if (this.emailSender != null)
                {
                    emailSender.SendMail(this.logBody + "\n" + e.ToString(), "ERRO");
                }

                this._logger.WriteLine("Erro na thread: " + Thread.CurrentThread.Name);
                this._logger.WriteLine(e.ToString());
                this.threadFail = true;
            }
            else
            {
                this._logger.WriteLine("Abortando thread: " + Thread.CurrentThread.Name);
            }

        }
    }
}
