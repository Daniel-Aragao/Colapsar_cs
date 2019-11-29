class Node:
    def __init__(self, weight, id=None, x=None, y=None, label=''):
        self.id = id
        self.label = label
        self.weight = weight
        self.x = x
        self.y = y
        self.edges = []
        self.other_attributes = {}
    
    def getDegree(self):
        return len(self.edges)
    
    def get_out_degree(self):
        return len([edge for edge in self.edges if edge.source == self])

    def get_in_degree(self):
        return len([edge for edge in self.edges if edge.target == self])
    
    def get_attribute_x(self):
        keys = self.other_attributes.keys()
        x = None

        if 'x' in keys:
            x = self.other_attributes['x']
        elif 'longitude' in keys:
            x = self.other_attributes['longitude']

        return x

    def get_attribute_y(self):
        keys = self.other_attributes.keys()
        y = None

        if 'y' in keys:
            y = self.other_attributes['y']
        elif 'latitude' in keys:
            y = self.other_attributes['latitude']

        return y
    
    def __str__(self):
        result = str(self.id).replace(',','') + ","
        result += str(self.label).replace(',','') + ","
        result += str(self.weight).replace(',','') + ","
        result += 'y' + ','
        result += str(self.y).replace(',','') + ','
        result += 'x,'
        result += str(self.x).replace(',','')

        # other attributes

        return result
    
    def __repr__(self):
        return str(self.id)

class Edge:
    def __init__(self, source, target, weight=0, label=None, mirror=None):
        self.label = label
        self.source = source
        self.target = target
        self.weight = weight
        self.other_attributes = {}
        self.mirror = mirror

    def is_digraph(self):
        return not bool(self.mirror)
    
    def isIncident(self, node: Node):
        return self.source == node or self.target == node

    def __str__(self):
        result = str(self.source.id).replace(',','') + ","
        result += str(self.weight).replace(',','') + ","
        result += str(self.target.id).replace(',','') # + ","
        # result += 'distance,'
        # result += str(self.weight).replace(',','')
        
        # other attributes

        return result
    
    # def __eq__(self, otherEdge):
    #     if self is otherEdge or (otherEdge.source is self.source and otherEdge.destination is self.destination and otherEdge.distance == self.distance):
    #         return True
        
    #     return False