import random
import math
import datetime as dt

from Entities.graph import Graph

class Generator:

    @staticmethod
    def __generate_graph_name(txt):
        dtnow = dt.datetime.now()
        return str(txt) + "_" + str(dtnow.year) + "_" + str(dtnow.month) + "_" + str(dtnow.day) + "_" + str(dtnow.hour) + "_" + str(dtnow.minute) + "_" + str(dtnow.second)

    @staticmethod
    def create_regular_graph(m,n,p=0,horizontal_loop=True,vertical_loop=True, exporter=None):
        """
        m -> number of nodes in first dimension
        n -> number of nodes in second dimension
        p -> probability to creating non regular random edges instead of creating a regular (default 0)

        The resulting number of nodes is m*n and edges m*n*2

        returns a regular graph
        """
        graphname = Generator.__generate_graph_name("RegularGraph_"+str(m)+"_"+str(n))
        print("Creating " + graphname)

        graph = Graph(graphname)

        first_line = None
        last_line = None
        current_line = []
        pedges = 0

        for j in range(0, n):
            last_node = None
            first_of_list = None
            current_line = []
            
            for i in range (0, m):
                id = graph.next_id()
                node = graph.create_node(weight=0, id=id, x=j, y=i, label="sintetic")

                current_line.append(node)

                if(last_node is None):
                    first_of_list = node

                else:
                    graph.create_edge(last_node, node, weight=1.0, label="sintetic")

                last_node = node
            
            if horizontal_loop:
                if Generator.__is_sorted(p):
                    pedges += 1
                else:
                    graph.create_edge(last_node, first_of_list, weight=1.0, label="sintetic")


            if last_line is None:
                first_line = current_line

            else:
                #create edges through the current line and the last line
                pedges += Generator.__create_edges_betwen_lines(graph, current_line, last_line, p)
            
            # print(str(len(graph._nodes)) + " nodes created")
            last_line = current_line
        
        # create edges from the last line to the first line

        if vertical_loop:
            pedges += Generator.__create_edges_betwen_lines(graph, last_line, first_line, p)
        
        if pedges > 0:
            for i in range(pedges):
                Generator.create_random_edge(graph, weight=1.0)

        if exporter is not None:
            exporter.export_graph_to_txt(graph)

        return graph
    
    @staticmethod
    def create_random_graph(m, n, exporter=None):
        """
        Create a graph with m*n nodes and m*x*2 edges created randomly avoiding
        paralel repeated source, destination pairs

        m -> number of nodes in first dimension
        n -> number of nodes in second dimension

        The resulting number of nodes is m*n

        The resulting number of egdes is 2 * m * n - m - n

        returns a random graph
        """
        num_nodes = m * n
        num_edges = 2 * m * n - m - n

        name = Generator.__generate_graph_name("RandomGraph_"+str(m)+"_"+str(n))
        print("Creating " + name)

        graph = Graph(name)

        index_matrix = {}

        repeated_edges_tuples = []

        for j in range(n):  
            index_matrix[j] = {}

            for i in range (0, m):
                id = graph.next_id()
                node = graph.create_node(weight=0, id=id, x=j, y=i, label="sintetic")

                index_matrix[j][i] = node
            
        # print(str(len(graph._nodes)) + " nodes created")

        def get_random_node(index_matrix):
            i = random.randint(0,n - 1)
            j = random.randint(0,m - 1)

            return index_matrix[i][j]
        
        for e in range(num_edges):
            repeated = True

            while repeated:
                src = get_random_node(index_matrix)
                target = get_random_node(index_matrix)

                if((src.id, target.id) in repeated_edges_tuples or (target.id, src.id) in repeated_edges_tuples):
                    continue
                
                repeated = False

            repeated_edges_tuples.append((src.id, target.id))
            graph.create_edge(src, target, weight=1.0, label='sintetic')            

        # print(str(e) + " edges created")
                
        if exporter is not None:
            exporter.export_graph_to_txt(graph)

        return graph    

    @staticmethod
    def create_small_world_graph(m, n, percent, horizontal_loop=True, vertical_loop=True, exporter=None):
        """
        Creates a regular graph and then calls "Generator.transform_into_small_world_graph" with the 
        informed "percent" as the percent of edges changed
        """
        graph = Generator.create_regular_graph(m,n,0,horizontal_loop,vertical_loop)

        name = Generator.__generate_graph_name("SmallWorldGraph_"+str(m)+"_"+str(n)+"_"+str(percent))
        print("Creating " + name)
        graph.name = name

        Generator.transform_into_small_world_graph(graph, percent)

        if exporter is not None:
            exporter.export_graph_to_txt(graph)

        return graph
    
    @staticmethod
    def transform_into_small_world_graph(graph: Graph, percent, exporter=None):
        """
        From a regular graph, remove randomly based on the percent informed on parameters and 
        recreate selecting randomly new sources and targets

        percent -> float between 0 and 1
        """

        num_edges = math.ceil((graph.getEdgesListSize()/2) * percent)
        edge_list = graph.getEdgesList()

        for i in range(num_edges):
            edge = random.choice(edge_list)

            edge_list.remove(edge)
            
            if not edge.is_digraph():
                edge_list.remove(edge.mirror)
            
            graph.remove_edge(edge, kill_from_nodes=True)

            Generator.create_random_edge(graph, weight=1.0, not_repeat=False)

    @staticmethod
    def create_scale_free_graph(qtd_nodes, exporter=None):
        name = Generator.__generate_graph_name("ScaleFreeGraph_"+str(qtd_nodes))
        print("Creating " + name)
        graph = Graph(name)

        n = int(math.sqrt(qtd_nodes))

        positioning_list = [(x,y) for y in range(n) for x in range(n)]
        random.shuffle(positioning_list)

        for id in range(len(positioning_list)):
            x,y = positioning_list.pop()

            sorted_node = Generator.__sort_node(graph)

            node = graph.create_node(weight=0, id=id, x=x, y=y, label="sintetic")

            if id is not 0:
                graph.create_edge(node, sorted_node, weight=1.0, label="sintetic")
            
            # print(str(len(graph._nodes)) + " nodes created")

        if exporter is not None:
            exporter.export_graph_to_txt(graph)
        
        return graph

    @staticmethod
    def __sort_node(graph):
        lottery = Generator.__generate_lottery(graph)

        dice = random.random()
        total = 0
        
        for node, chance in lottery:
            total += chance

            if total >= dice:
                return node

        return None
        
    @staticmethod
    def __generate_lottery(graph: Graph):
        lottery = []

        nodes = graph.getNodesList()

        qtd_nodes = len(nodes)

        for node in nodes:
            edges_per_node = len(node.edges)

            if not graph.is_digraph():
                edges_per_node = edges_per_node/2
            
            if qtd_nodes is 1:
                percent = 1
            else:
                percent = edges_per_node/qtd_nodes

            lottery.append((node, percent))
        
        return lottery

    @staticmethod
    def create_random_edge(g:Graph, weight=0, not_repeat=True):
        nodes = g.getNodesList()

        repeated = True

        if not_repeat:
            while repeated:
                src = random.choice(nodes)
                target = random.choice(nodes)

                repeated = len(g.get_edge_by_source_and_target(src, target)) is not 0
        else:
            src = random.choice(nodes)
            target = random.choice(nodes)
            
        return g.create_edge(src, target, weight)

    @staticmethod
    def __is_sorted(p):
        if p > 0:
            p = math.ceil(p*100)
            return random.randrange(100) < p
        
        return False

    @staticmethod
    def __create_edges_betwen_lines(graph, line1, line2, p):
        """
        Create edges betwen the nodes of the same index on two lines
        """
        pedges = 0
        for i, node1 in enumerate(line1):
            node2 = line2[i]

            if Generator.__is_sorted(p):
                pedges += 1
            else:
                graph.create_edge(node1, node2, weight=1.0, label="sintetic")
        
        return pedges