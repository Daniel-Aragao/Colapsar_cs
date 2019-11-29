from Entities.graph import Graph
from Entities.node import Node
import math

class Utils:
    @staticmethod
    def distance_betweenNodes(n1: Node, n2: Node):
        x1 = n1.get_attribute_x()
        y1 = n1.get_attribute_y()

        x2 = n2.get_attribute_x()
        y2 = n2.get_attribute_y()

        return math.sqrt((x1 - x2) ** 2 + (y1 - y2) ** 2)
