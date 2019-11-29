class Node:
    def __init__(self, id, label, weight, latitude, longitude):
        self.id = id
        self.label = label
        self.weight = weight
        self.latitude = latitude
        self.longitude = longitude

class Edge:
    def __init__(self, source, destination, distance):
        self.source = source
        self.destination = destination
        self.distance = distance