using Core.models;
using Infra.services.regions;

using System;
using System.Threading;
using System.Collections.Generic;
using Infra.services.log;

namespace Infra.services.multithread
{
    public class ThreadBuilder
    {
        private Graph _graph;
        private SearchStrategyFactory StrategyFactory;
        private List<Tuple<long, long>> ODs;
        private double Radius;
        private int ThreadsQuantity;

        public ThreadBuilder(Graph g, SearchStrategyFactory strategyFactory, List<Tuple<long, long>> ODs, double radius, int threads)
        {
            this._graph = g;
            this.StrategyFactory = strategyFactory;
            this.ODs = ODs;
            this.Radius = radius;
            this.ThreadsQuantity = Math.Max(Math.Min(Math.Min(ODs.Count, threads), Environment.ProcessorCount), 1);

            // new Thread(new ThreadStart(ThreadManager))
        }

        public void Begin()
        {
            var logger = LoggerFactory.GetLogger();

            int interval = this.ODs.Count / this.ThreadsQuantity;
            int rest = this.ODs.Count % this.ThreadsQuantity;

            ThreadManager manager = new ThreadManager(this._graph, this.ThreadsQuantity, this.ODs.Count, this.Radius, this.StrategyFactory.SearchName);

            for (int i = 0; i < this.ThreadsQuantity; i++)
            {
                int begin = i * interval;
                int end = interval;//i * interval + interval;

                if (i + 1 == this.ThreadsQuantity)
                {
                    end += rest;
                }

                List<Tuple<long, long>> portion = this.ODs.GetRange(begin, end);

                Graph graphClone = this._graph.Clone();
                SearchStrategy searhcStrategy = this.StrategyFactory.GetStrategy(graphClone);

                var ts = new ThreadSearch(graphClone, searhcStrategy, portion, this.Radius, manager);
                var thread = new Thread(new ThreadStart(ts.Search));
                thread.Name = "T" + (i + 1);
                
                logger.WriteLine(thread.Name + " ODs: " + portion.Count.ToString());

                thread.Start();
            }
        }
    }
}