import matplotlib.pyplot as plt
import networkx as nx
from node import Node
from node import Edge
from mpl_toolkits.basemap import Basemap

graphName = "Fortaleza"

#basemap Fortaleza
m = Basemap(
    llcrnrlon=-38.651157,
    llcrnrlat=-3.893816,
    urcrnrlon=-38.393815,
    urcrnrlat=-3.689536)

graphPath= r'caracterizacao-dados-reviews/graphs/'
# graphPath= r'caracterizacao-dados-reviews/graphs/'
path = graphPath+graphName+'-network-osm-2018-1.txt'

graphFile = open(path,'rb')
print(path)
lines = graphFile.read()

lines = str(lines)[2::].split(r'\n')
# print(lines)

state = ''
Nodes = []
Edges = []

for line in lines:
    line = line.strip()
    
    if(line == 'edges'):
        state = line
        continue

    elif(line == 'nodes'):
        state = line
        continue

    elif(line == "" or line.isspace() or line == "\n"):
        continue

    props = line.split(',')
    if(state == 'nodes'):
        Nodes.append(Node(props[0], props[1], props[2], props[4], props[6]))
    
    elif(state == 'edges'):
        Edges.append(Edge(props[0], props[2], props[4]))


g = nx.DiGraph()
lat = []
lng = []
ids = []

for node in Nodes:
    g.add_node(node.id, pos=(node.longitude,node.latitude))
    lat.append(node.latitude)
    lng.append(node.longitude)
    ids.append(node.id)

mx, my= m(lat,lng)
pos = {}
for i in range(len(mx)):
    pos[ids[i]] = (mx[i],my[i])

print(pos)
   
# pos = nx.get_node_attributes(g,'pos')
# print(pos)

nx.draw(g, pos, node_size=0.2)
plt.show()
