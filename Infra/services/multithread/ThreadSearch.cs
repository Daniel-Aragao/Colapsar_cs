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
        private IList<string> ODs;
        private double Radius;

        public ThreadSearch(Graph graph, SearchStrategy strategy, IList<string> ODs, double radius)
        {
            this._graph = graph;
            this.Strategy = strategy;
            this.ODs = ODs;
            this.Radius = radius;
        }

        public void Search()
        {
            foreach(string od in ODs)
            {
                var splitedODs = od.Split(Constants.SEPARATOR_ODs);

                var sourceId = long.Parse(splitedODs[0]);
                var targetId = long.Parse(splitedODs[1]);

                // time start
                var pathRoute = this.Strategy.Search(this._graph.GetNodeById(sourceId), this._graph.GetNodeById(targetId), this.Radius);                
                // time end

                // pathRoute.DeltaTime = timeEnd - timeStart; 
                // update output after 10%+ ODs concluded
            }
        }
    }
}