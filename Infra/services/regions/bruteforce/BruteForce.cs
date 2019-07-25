using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.models;

namespace Infra.services.regions
{
    public class BruteForce : SearchStrategy
    {
        public BruteForce(Graph graph): base(graph, Constants.ALGORITHMN_NAME_BRUTE_FORCE)
        {
            
        }

        public override PathRoute Search(Node source, Node target, double radius)
        {
            PathRoute betterPathRoute = this.SearchParametersEvaluation(source, target, radius);

            if(betterPathRoute != null)
            {
                return betterPathRoute;
            }

            var sources = this.Graph.GetNodesByRadius(source, radius);
            var targets = this.Graph.GetNodesByRadius(target, radius);

            foreach(var origin in sources)
            {
                foreach (var destination in targets)
                {
                    if(origin != destination)
                    {
                        PathRoute pathRoute = Graph.ShortestPathHeuristic(origin, destination);

                        if(betterPathRoute == null || betterPathRoute.Status != EPathStatus.Found || 
                            (betterPathRoute.Status == EPathStatus.Found && 
                                pathRoute.Status == EPathStatus.Found && 
                                betterPathRoute.Distance > pathRoute.Distance))
                        {
                            betterPathRoute = pathRoute;
                        }
                    }
                }
            }

            return betterPathRoute;
        }
    }
}