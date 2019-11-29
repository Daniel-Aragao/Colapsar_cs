import networkx as nx
from Entities.graph import Graph
from Entities.node import Edge

class NetowrkxConverter:

    def __init__(self):
        self.nxGraph = None
        self.graph = None
    
    @staticmethod
    def ConvertToNetowrkx(graph: Graph):
        converter = NetowrkxConverter()

        converter.nxGraph = nx.Graph()
        converter.graph = graph

        for edge in graph.getEdgesList():
            converter.nxGraph.add_edge(edge.source, edge.target)

        return converter
    
    # @staticmethod
    # def ConvertFromNetworkx(nxGraph: nx.Graph):
    #     converter = NetowrkxConverter()

    #     return converter        
    
    def getDiameter(self):
        pairs = nx.all_pairs_shortest_path(self.nxGraph)

        biggest = 0

        for pair in pairs:
            paths = list(pair[1].values())

            for path in paths:
                size = len(path)

                if biggest < size:
                    biggest = size

        return biggest

    def getClusterCoefficient(self):
        return nx.average_clustering(self.nxGraph)

    def getAvgPathLength(self):
        return nx.average_shortest_path_length(self.nxGraph)
    
    def getGiantComponentGraph(self):
        giantComponent = self.getGiantComponent()

        newGraph = Graph(self.graph.name+"_giantComponent")

        for edge in self.graph._edges:
            for node in giantComponent:
                if edge.isIncident(node):
                    newGraph.add_edge(edge)
        
        return NetowrkxConverter.ConvertToNetowrkx(newGraph)


    def getGiantComponent(self):
        components = nx.connected_components(self.nxGraph)

        biggestLength = 0
        biggest = None

        for component in components:
            componentLength = len(component)
            
            if biggestLength < componentLength:
                biggest = component
                biggestLength = componentLength
        
        return biggest