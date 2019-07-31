using System;
using Core.models;


namespace Infra.services.regions
{
    public abstract class SearchStrategyFactory
    {
        protected string searchName = "Search";
        public string SearchName {get {return searchName;}}

        protected SearchStrategyFactory(string searchName)
        {
            this.searchName = searchName;
        }

        public abstract SearchStrategy GetStrategy(Graph g);

        public static SearchStrategyFactory GetFactory(string strategy)
        {
            SearchStrategyFactory strategyFactory = null;

            if(strategy == "C")
            {
                strategyFactory = new CollapseFactory();
            }
            else if(strategy == "BF")
            {
                strategyFactory = new BruteForceFactory();
            }
            else
            {
                throw new ArgumentException("Stratregy invalid try \"C\" to Collapse and \"BF\" to Brute Force ");
            }

            return strategyFactory;
        }
    }
}