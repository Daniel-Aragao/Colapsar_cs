using System;
using System.Collections.Generic;

using Core;
using Core.models;
using Infra.services.regions;

namespace Infra.services.multithread
{
    public class ThreadSearch
    {
        private Graph _graph; 
        private @string Strategy; 
        private List<Tuple<long,long>> ODs;
        private double Radius;
        private ThreadManager ThreadManager;
        public int ODSize { get {return this.ODs.Count;} }

        public ThreadSearch(Graph graph, @string strategy, List<Tuple<long,long>> ODs, double radius, ThreadManager threadManager)
        {
            this._graph = graph;
            this.Strategy = strategy;
            this.ODs = ODs;
            this.Radius = radius;
            this.ThreadManager = threadManager;
        }

        public void Search()
        {
            var percent = (this.ODSize * 10)/100;
            var pathRoutes = new List<PathRoute>();
            var threadTimeStart = DateTime.Now;
            TimeSpan threadTimeDelta = threadTimeStart - threadTimeStart;

            for(int i = 1, j = 0; i <= this.ODs.Count; i++)
            {
                var od = this.ODs[i];

                var sourceId = od.Item1;
                var targetId = od.Item2;

                var timeStart = DateTime.Now;

                var pathRoute = this.Strategy.Search(this._graph.GetNodeById(sourceId), this._graph.GetNodeById(targetId), this.Radius);                

                var timeEnd = DateTime.Now;

                pathRoute.DeltaTime = timeEnd - timeStart;
                threadTimeDelta += pathRoute.DeltaTime;

                pathRoutes.Add(pathRoute);

                if(percent <= i - j || i == this.ODSize)
                {
                    var progress = i/this.ODSize;
                    this.ThreadManager.addProgress(progress, pathRoutes);

                    j = i;
                    pathRoutes = new List<PathRoute>();
                }
            }

            this.ThreadManager.endThread(threadTimeDelta);
        }
    }
}