using System;

namespace Infra.services.multithread
{
    public class ThreadManager
    {
        private int ThreadsQuantity;
        private int ODsSize;
        private double Radius;

        public ThreadManager(int threadsQuantity, int ODsSize, double radius)
        {
            this.ThreadsQuantity = threadsQuantity;
            this.ODsSize = ODsSize; 
            this.Radius = radius;

            throw new NotImplementedException();
        }
    }
}