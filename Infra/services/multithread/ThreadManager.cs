using System;
using System.Collections.Generic;

using Core;
using Core.models;
using Infra.services;

namespace Infra.services.multithread
{
    public class ThreadManager
    {
        private int ThreadsQuantity;
        private readonly object progressLock = new object();
        private int ODsSize;
        private double Radius;
        private string FileName;

        public ThreadManager(Graph graph, int threadsQuantity, int ODsSize, double radius)
        {
            this.ThreadsQuantity = threadsQuantity;
            this.ODsSize = ODsSize; 
            this.Radius = radius;

            var now = DateTime.Now;

            this.FileName = graph.Name;
            this.FileName += Constants.SEPARATOR_FILE_NAMES + this.ODsSize.ToString();
            this.FileName += Constants.SEPARATOR_FILE_NAMES + this.Radius.ToString();
            this.FileName += Constants.SEPARATOR_FILE_NAMES + this.ThreadsQuantity;
            // this.FileName += Constants.SEPARATOR_FILE_NAMES + now.Year + "_" + now.Month + "_" + now.Day;
            // this.FileName += "_" + now.Hour + "_" + now.Minute + "_" + now.Second;
        }

        public void addProgress(double progress, IList<PathRoute> pathRoutes)
        {
            lock (progressLock)
            {
                
                var writer = Export.GetWriter(this.FileName);
                foreach (var pathRoute in pathRoutes)
                {
                    writer.WriteLine(pathRoute.ToString());
                }
                throw new NotImplementedException();
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