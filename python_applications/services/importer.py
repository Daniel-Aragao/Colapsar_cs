import os

from Entities.graph import Graph
from Entities.node import Node
from Entities.node import Edge

class Importer:
    def __init__(self):
        pass
    
    @staticmethod
    def import_graph_from_txt(path, show_progess=True):

        if not os.path.exists(path):
            raise Exception('Can\'t find the informed path: ' + str(path) + " \n From " + str(os.path.realpath('.')))

        name = os.path.basename(path).replace('.txt','')

        graph = Graph(name)

        with open(path, 'r') as file_obj:
            mode = ''
            count = 0
            source = None
            target = None

            for i, line in enumerate(file_obj):
                line = line.strip()

                if line == 'nodes':
                    mode = line
                    count = 0
                    continue
                elif line == 'edges':
                    mode = line
                    count = 0
                    continue

                if mode == 'nodes':
                    splited_line = line.split(',')

                    ide = splited_line[0]
                    label = splited_line[1]
                    weight = splited_line[2]
                    y = int(splited_line[4])
                    x = int(splited_line[6])

                    node = graph.create_node(weight, id=ide, x=x, y=y, label=label)

                    if not Importer.__add_other_attributes(splited_line[7::], node.other_attributes):
                        raise Exception('The pair of values is malformed for the node in line ' + str(i) + " : " + line)
                    count += 1
                    if show_progess and count%1000 is 0:
                        print(str(count) + ' nodes imported')
                        
                elif mode == 'edges':
                    splited_line = line.split(',')

                    source_id = splited_line[0]
                    weight = splited_line[1]
                    target_id = splited_line[2]
                    # y = splited_line[4]
                    # x = splited_line[6]

                    if not (source != None and source.id == source_id):
                        source = graph.get_node_by_id(source_id)
                        
                    target = graph.get_node_by_id(target_id)

                    edge = graph.create_edge(source, target, weight=weight, addNodes=False,is_mirror_recursion=True) #(weight, id=ide, x=x, y=y, label=label)

                    if not Importer.__add_other_attributes(splited_line[3::], edge.other_attributes):
                        raise Exception('The pair of values is malformed for the edge in line ' + str(i) + " : " + line)
                    
                    count += 1
                    if show_progess and count%1000 is 0:
                        print(str(count) + ' edges imported')

        return graph

    @staticmethod
    def __add_other_attributes(source_array, target_dict):
        line_size = len(source_array)
        
        if line_size > 0:
            if line_size % 2 is 1:
                return False

            for i in range(0, line_size, 2):
                target_dict[source_array[i]] = source_array[i+1]
        
        return True
    
    @staticmethod
    def import_file(path, show_progress=False):
        lines = []
        with open(path, 'r') as file_obj:
            for i, line in enumerate(file_obj):
                lines.append((i, line))
            
        return lines

    #export_graph_to_txt