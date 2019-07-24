using System;
using System.Collections.Generic;

using Core.models;

namespace Infra.services.multithread
{
    public class ThreadManager
    {
        private int ThreadsQuantity;
        private readonly object progressLock = new object();
        private int ODsSize;
        private double Radius;

        public ThreadManager(int threadsQuantity, int ODsSize, double radius)
        {
            this.ThreadsQuantity = threadsQuantity;
            this.ODsSize = ODsSize; 
            this.Radius = radius;

            throw new NotImplementedException();
        }

        public void addProgress(double progress, IList<PathRoute> pathRoute)
        {
            lock (progressLock)
            {
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