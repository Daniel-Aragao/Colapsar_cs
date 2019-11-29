from services.importer import Importer
import os

from services.converter import NetowrkxConverter
from Entities.graph import Graph

def show_info(path):
    graph = Importer.import_graph_from_txt(path, show_progess=False)
    nxGraphConverter = NetowrkxConverter.ConvertToNetowrkx(graph).getGiantComponentGraph()
    graph = nxGraphConverter.graph

    print()
    print("               Graph:", os.path.basename(path))
    print("       Branch Factor:", graph.get_branching_factor())
    print("     Number of Nodes:", graph.getNodesListSize())
    print("     Number of Edges:", graph.getEdgesListSize())
    print("             Density:", graph.getDensity())
    print("            Diameter:", nxGraphConverter.getDiameter())
    print("             Entropy:", "")
    print(" Cluster Coefficient:", nxGraphConverter.getClusterCoefficient())
    print("          Hub Degree:", graph.getHubDegree())
    print("    Avg. Path Length:", nxGraphConverter.getAvgPathLength())
    print()
    # print(nxGraph.getGiantComponent())

if __name__ == '__main__':
    import sys

    if(len(sys.argv) > 1):
        show_info(sys.argv[1])
    else:
        show_info("/home/daniel/Documentos/Git/Colapsar/python_applications/output/graphs/RegularGraph_20_20_2018_12_10_17_15_56.txt")
