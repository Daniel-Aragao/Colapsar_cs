import numpy as np
import matplotlib.colors as mcolors
import networkx as nx
import matplotlib.pyplot as plt
from mpl_toolkits.basemap import Basemap as Basemap
import operator

def main(cidade, gc, path, plothist=False, saveimg=True, filter_deviation=False):
    print("Cidade: " + cidade)
    print("GC: " + gc)
    print("Path: " + path)

    # PLOT
    plt.rc('patch',linewidth=2)
    plt.rc('axes', linewidth=2, labelpad=10)
    plt.rc('xtick.minor', size=4, width=2)
    plt.rc('xtick.major', size=8, width=2, pad=8)
    plt.rc('ytick.minor', size=4, width=2)
    plt.rc('ytick.major', size=8, width=2, pad=8)
    # plt.rc('text', usetex=True)
    # plt.rc('font', family='serif', serif='Computer Modern', size=30)
    # PLOT

    # m.readshapefile('/Users/usuario/Downloads/anexos/shape', 'bairrosfortaleza')

    nome = []
    lat = []
    lng = []
    is_edges = False

    with open(path, 'rb') as f:
        linha = f.readlines()

        # print(linha)

        node = []
        target = []

        for line in linha:
            if line == b'nodes\n' :
                continue

            elif line == b'edges\n':
                is_edges = True
                continue

            quebra = str(line).strip(r"b|\\n|\n|'|\"").split(',')

            if not is_edges:

                nomes = str(quebra[0])
                lats = float(quebra[4])
                lngs = float(quebra[6])

                nome.append(nomes)
                lat.append(lats)
                lng.append(lngs)

            else:
                nodes = quebra[0].strip('\n')
                targets = quebra[2].strip('\n')

                node.append(nodes)
                target.append(targets)

    slng = sorted(lng, key=lambda x: float(x))
    slat = sorted(lat, key=lambda x: float(x))
    
    # maior coordenada
    urcrnrlon = slng[::-1][0]  # -38.393815
    urcrnrlat = slat[::-1][0]  # -3.689536

    # menor coordenada
    llcrnrlon = slng[0]  # -38.651157
    llcrnrlat = slat[0]  # -3.893816
    # -41.061550, -63.451664
    m = Basemap(
        llcrnrlon=llcrnrlon,
        llcrnrlat=llcrnrlat,
        urcrnrlon=urcrnrlon,
        urcrnrlat=urcrnrlat)

    if(plothist):
        print('Histograma Longetude')
        plot_hist(lng)

        print('Histograma Latitude')
        plot_hist(lat)
        

    if(filter_deviation):
        # print('Filtrando Latitude')
        min_interval, max_interval = get_deviation(lat)
        remove_by_index_with_value(min_interval, False, lat, [lng, nome])
        remove_by_index_with_value(max_interval, True, lat, [lng, nome])

        # print('Filtrando Longetude')
        min_interval, max_interval = get_deviation(lng)
        remove_by_index_with_value(min_interval, False, lng, [lat, nome])
        remove_by_index_with_value(max_interval, True, lng, [lat, nome])

        print('Filtrado')


    mx, my = m(lng, lat)

    quantidade_nos = len(nome)
    quantidade_arestas = len(node)

    g = nx.DiGraph()
    pos = {}

    for i in range(quantidade_arestas):
        # print(nome[i] + ", " + str(lat[i])+ ", " + str(lng[i]))
        o = node[i]
        t = target[i]
        if(not filter_deviation or (o in nome and t in nome)):
            g.add_edge(o, t)

    for j in range(quantidade_nos):
        pos[nome[j]] = (mx[j], my[j])

    degrees = g.degree()
    nos = g.nodes()
    n_color = np.asarray([degrees[n] * 7 for n in nos])
    nodes = g.nodes()
    d = nx.degree(g)    

    n_nodes = g.number_of_nodes()
    # n_links = g.number_of_edges()
    print('Numero de paradas: ' + str(n_nodes))
    # print('Numero de rotas: ' + str(n_links))

    deg=nx.degree_centrality(g)

    # print(max(deg.values()))
    valor_maximo = max(deg.items(), key=operator.itemgetter(1))[0]
    valor_minimo = min(deg.items(), key=operator.itemgetter(1))[0]

    print(valor_maximo+': ' + str(deg[valor_maximo]))
    print(valor_minimo+': ' + str(deg[valor_minimo]))
    # print(valor)

    # myarray = np.array(list(deg.values()),dtype=float)
    # print(np.mean(myarray))
    # print(np.mean(myarray)*(n-1))
    # print(' ')

    sc = nx.draw_networkx_nodes(g, pos, with_labels=False, nodelist=nodes, node_size=n_color,
                                node_color=n_color, ax=None, linewidths=0.4)

    # print(type(sc))

    sc.set_norm(mcolors.LogNorm())
    sc.set_edgecolor('black')
    # nx.draw_networkx_edges(G,pos,edge_color='black')

    plt.axis('off')

    if(saveimg):
        plt.savefig("./Imagens/" + cidade +"/" + gc + cidade + ".png", format="PNG")
    else:
        plt.show()        

    plt.close()
    print()

def get_deviation(array):
    std = np.std(array)
    mean = np.mean(array)
    min_interval = mean-std/2
    max_interval = mean+std/2
    # print([mean-std/2, mean-std/2])
    # print(std,mean)
    return min_interval, max_interval

def plot_hist(array):
    min_interval, max_interval = get_deviation(array)
    
    plt.hist(array, bins="auto", normed=True)
    plt.plot([min_interval, min_interval], [0,1])
    plt.plot([max_interval, max_interval], [0,1])
    plt.show()

def remove_by_index_with_value(value, bigger_than, target, others_targets):
    indexes = []
    for i, target_value in enumerate(target):
        if(bigger_than):
            if(target_value > value):
                indexes.append(i)
        else:
            if(target_value < value):
                indexes.append(i)

    indexes.reverse()

    for index in indexes:
        # print(index, target1[index], target2[index])
        del target[index]

        for item in others_targets:
            del item[index]
            

cidades = [
    # "Buenos Aires", # filtrar pontos
    # "Cairo",
    # "Cidade do mexico",
    # "Daca",
    # "Delhi",
    # "karachi",
    # "LA",
    # "Lagos",
    # "London", # filtrar pontos
    # "Manila",
    # "Moscou",
    "Mumbai-network-osm-2019-1_2.txt"
    # "NY",
    # "Paris", # filtrar pontos - zoom
    # "Pequim",
    # "Rio de Janeiro",
    # "SAO",
    # "Toquio", # filtrar pontos - zoom
    # "xangai",
    # "Fortaleza"
]

for cidade in cidades:
    # path = "../caracterizacao-dados-reviews/graphs/"+cidade+"-network-osm-2018-1.txt"
    # main(cidade, "", path, plothist=False, saveimg=False, filter_deviation=False)

    path = "../caracterizacao-dados-reviews/graphs/giantscomponentes/"+cidade
    main(cidade, "GC", path, plothist=False, saveimg=False, filter_deviation=False)
    

# main(cidade, GC, path)
