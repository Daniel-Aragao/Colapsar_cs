import networkx as nx
import numpy as np
import matplotlib.pyplot as plt

def show_sintetic_graph(graph):

    g = nx.Graph()
    pos = {}
    label = {}

    for node in graph.getNodesList():
        g.add_node(node.id)
        pos[node.id] = (node.x, node.y)
        label[node.id] = node.id

    for edge in graph.getEdgesList():
        g.add_edge(edge.source.id, edge.target.id)

    g = nx.relabel_nodes(g, label)
    nx.draw(g, pos=pos, node_size=300, node_color='red')
    nx.draw_networkx_labels(g, pos=pos, font_size=12)
    
    plt.show()

def Histograma_PowerLaw(data):
    log_data = np.log(data)
    hist,bins = np.histogram(log_data,bins=10)
    return np.exp(bins),hist/np.exp(bins[:-1])

def show_histogram(graph):
    # plt.hist([node.getDegree() for node in graph.getNodesList()], bins='auto')
    # plt.xscale("log")
    # plt.yscale("log")
    # plt.show()
    hist = Histograma_PowerLaw([node.getDegree() for node in g.getNodesList()])
    plt.plot(hist[0][:-1], hist[1], 'o')
    plt.xscale("log")
    plt.yscale("log")
    plt.plot(hist[0][:-1], hist[1], 'o')
    plt.show()