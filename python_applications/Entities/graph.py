from .node import Node
from .node import Edge

class Graph:
    def __init__(self, name="", nodes=None, edges=None, digraph=False):
        self.name = name
        self.id_counter = 0
        self._nodes = set()
        self._edges = set()
        self._digraph = digraph

        if not (edges is None):
            self.add_edges(edges)
            
        if not (nodes is None):
            self.add_nodes(nodes)
    
    def is_digraph(self):
        return self._digraph

    def next_id(self):
        id = self.id_counter
        self.id_counter += 1
        return id
    
    def getNodesList(self):
        """
        Transform the nodes set in a list and returns it
        """
        return list(self._nodes)
    
    def getNodesListSize(self):
        return len(self._nodes)
    
    def getEdgesList(self):
        """
        Transform the edges set in a list and returns it
        """
        return list(self._edges)
    
    def getEdgesListSize(self):
        return len(self._edges)
    
    def add_edges(self, edges, addNodes=True):
        for edge in edges:
            if edge not in self._edges:
                self.add_edge(edge, addNodes)
    
    def add_edge(self, edge, addNodes=True):
        """
        add the edge on list and it source and target nodes if they reference don't exist yet
        (Default parameter addNodes=False)
        """
        if not (edge in self._edges):
            self._edges.add(edge)

        if addNodes:
            self.__buildNodesFromEdge(edge)
    
    def __buildNodesFromEdge(self, edge):
        """
        Calls the add_node function for the source and the target of the informed edge
        """
        source = edge.source
        destination = edge.target

        self.add_node(source, addEdges=False)

        self.add_node(destination, addEdges=False)
        
    def add_nodes(self, nodes):
        for node in nodes:
            if node not in self._nodes:
                self.add_node(node)

    def add_node(self, node, addEdges=False):
        """
        add the node and then add it's edges if it have any edge reference that don't belong to the list yet
        """
        new_edges = node.edges

        if not (node in self._nodes):
            self._nodes.add(node)

            if addEdges and len(new_edges) > 0:
                self.add_edges(new_edges, addNodes=False)
    
    def remove_node(self, node, kill_edges=True):
        """
        Remove and return the node from the list. If default parameter were not altered all the edges that contains this node
        are also removed.
        """
        if node in self._nodes:
            self._nodes.remove(node)

            if(kill_edges):
                discard_edges = [edge for edge in self._edges if edge.source == node or edge.target == node]

                for edge in discard_edges:
                    self._edges.remove(edge)

            return node
    
    def remove_edge(self, edge, kill_from_nodes=True):
        """
        Remove and return edge from the list. If default parameter were not altered the edge is also removed from any node
        that has the reference to this edge.
        """
        if edge in self._edges:
            self._edges.remove(edge)

            if not edge.is_digraph():
                mirror = edge.mirror

                edge.mirror = None
                mirror.mirror = None

                self.remove_edge(mirror, kill_from_nodes=kill_from_nodes)

            if(kill_from_nodes):
                edge.source.edges.remove(edge)
                edge.target.edges.remove(edge)

            return edge

    def get_node_by_id(self, id):        
        return next((node for node in self._nodes if id == node.id), None)
    
    def get_node_by_weight(self, value):        
        return next((node for node in self._nodes if value == node.weight), None)
    
    def get_nodes_by_special_attr(self, name, value):
        """
        get a list of nodes with the attribute name and the attribute value passed, or empty list
        """
        return [node for node in self._nodes if value == node.other_attributes[name]]
    
    def get_node_with_iterator(self, iterator):
        """
        Return a node that matches the given iterator, or None otherwise
        """
        return next(iterator, None)

    def get_edge_by_weight(self, weight):
        return next((edge for edge in self._edges if weight == edge.weight), None)

    def get_edge_by_source_and_target(self, source, target):
        """
        Return a list of edges that has the given node on source and target attributes. 
        Return empty list in case if can't find any
        """
        return [edge for edge in self._edges if source == edge.source and target == edge.target]

    def get_edges_by_source_or_target(self, node):
        """
        Return a list of edges that has the given node on source or target attributes. 
        Return empty list in case if can't find any
        """
        return [edge for edge in self._edges if node == edge.source or node == edge.target]
        
    
    def get_edge_by_special_attr(self, name, value):
        """
        get a list of edge with the attribute name and the attribute value passed, or empty list
        """
        return [edge for edge in self._edges if value == edge.other_attributes[name]]
    
    def get_edge_with_iterator(self, iterator):
        """
        Return a edge that matches the given iterator, or None otherwise
        """
        return next(iterator, None)
    
    def create_edge(self, source, target, weight=0, label=None, addToList=True, addNodes=True, is_mirror_recursion=False):
        """
        Create and return an edge after adding to the edges list (default attr addToList=True)
        """

        if weight == "calcule":
            import math
            weight = math.sqrt(((target.x - source.x) ** 2) + ((target.y - source.y) ** 2))

        edge = Edge(source, target, weight=weight, label=label)
        
        if not self.is_digraph() and not is_mirror_recursion:
            mirror_edge = self.create_edge(target, source, weight, label, addToList, addNodes, True)

            edge.mirror = mirror_edge
            mirror_edge.mirror = edge
        
        source.edges.append(edge)
        target.edges.append(edge)

        if addToList:
            self.add_edge(edge, addNodes=addNodes)

        return edge

    def create_node(self, weight, id=None, x=None, y=None, label=None, addToList=True):
        """
        Create and return a node after adding to the nodes list (default attr addToList=True)
        """
        node = Node(weight, id=id, x=x, y=y, label=label)

        if addToList:
            self.add_node(node, addEdges=False)

        return node

    def getDensity(self):
        E = self.getEdgesListSize()
        V = self.getNodesListSize()

        if self.is_digraph():
            return E/(V*(V-1))

        return 2*E/(V*(V-1))

    def getHubDegree(self):
        biggest = 0

        for node in self._nodes:
            degree = node.getDegree()

            if biggest < degree:
                biggest = degree
        
        return biggest
    
    def get_branching_factor(self):
        total = 0

        for node in self._nodes:
            total += node.get_out_degree()
        
        return total/self.getNodesListSize()

            

class Digraph(Graph):
    def __init__(self, name="", nodes=None, edges=None):
        Graph.__init__(self, name, nodes, edges, digraph=True)