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
        private SearchStrategy Strategy; 
        private List<Tuple<long,long>> ODs;
        private double Radius;
        private ThreadManager ThreadManager;

        public ThreadSearch(Graph graph, SearchStrategy strategy, List<Tuple<long,long>> ODs, double radius, ThreadManager threadManager)
        {
            this._graph = graph;
            this.Strategy = strategy;
            this.ODs = ODs;
            this.Radius = radius;
            this.ThreadManager = threadManager;
        }

        public void Search()
        {
            foreach(var od in this.ODs)
            {
                var sourceId = od.Item1;
                var targetId = od.Item2;

                var timeStart = DateTime.Now;

                var pathRoute = this.Strategy.Search(this._graph.GetNodeById(sourceId), this._graph.GetNodeById(targetId), this.Radius);                

                var timeEnd = DateTime.Now;
                var time = timeEnd - timeStart;
                // time.
                // pathRoute.DeltaTime = timeEnd - timeStart; 
                // update output after 10%+ ODs concluded
            }
        }
    }
}