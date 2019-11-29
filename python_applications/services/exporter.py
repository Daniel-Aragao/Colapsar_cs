class Exporter:
    outputfolder="./output/"

    def __init__(self):
        pass
    
    @staticmethod
    def export_graph_to_txt(graph, output=outputfolder+"/graphs/"):
        # nodes
        # 499842466,cross,0.0,latitude,-3.777314,longitude,-38.6260466
        # edges
        # 499842466,1.0,1703311120,distance,4.978396219401326,name-street,Rua B,type_route,driving

        path = output + graph.name

        with open(str(path) + ".txt", 'w') as file_obj:
            file_obj.write('nodes\n')
            nodes = graph.getNodesList()

            for i, node in enumerate(nodes):
                file_obj.write(str(node)+'\n')

                if i % 10000:
                    file_obj.flush()

            file_obj.flush()

            file_obj.write('edges\n')
            edges = graph.getEdgesList()

            for i, edge in enumerate(edges):
                file_obj.write(str(edge)+'\n')

                if i % 10000:
                    file_obj.flush()
    
    @staticmethod
    def printToFile(string, filename, output=outputfolder):
        path = output + filename

        with open(path + ".txt", 'w+') as file_obj:
            # file_obj.write('\n')
            file_obj.write(string)
