import networkx as nx
import numpy as np
import matplotlib.pyplot as plt
from services.importer import Importer
from services.GraphUtils import Utils
from Entities.graph import Graph
from Entities.node import Node


def Histograma_PowerLaw(data):

    log_data = np.log(data)
    hist,bins = np.histogram(log_data,bins=10)

    return np.exp(bins),hist/np.exp(bins[:-1])


def Histograma_distancias_grafo():
    g = Importer.import_graph_from_txt("/home/daniel/Documentos/Git/Colapsar/caracterizacao-dados-reviews/graphs/giantscomponentes/Fortaleza-network-osm-2018-1.txt", show_progess=False)
    print('Grafo importado: ' + g.name)
    nodes = g.getNodesList()
    nodeListSize = len(nodes)

    dists = []

    average = 0
    count = 0

    for i, n1 in enumerate(nodes):
        minor = 9999999999

        for n2 in nodes:
            if n1 is not n2:
                distance = Utils.distance_betweenNodes(n1, n2)

                if minor > distance:
                    minor = distance

        average += minor
        dists.append(minor)

        count += 1
        if count/nodeListSize >= 0.1:
            print('Progresso: ' + i/nodeListSize)

    average = average/nodeListSize

    print("Média: " + average)
    plt.hist(dists) 
    plt.show()

def Histograma_distancias_saida():
    lines = Importer.import_file("output/distances_diff.txt")
    lines = lines[0][1].split(',')
    lines = [float(line) for line in lines]

    # print(len(lines), lines)

    plt.hist(lines, bins=40)
    plt.show()


Histograma_distancias_saida()