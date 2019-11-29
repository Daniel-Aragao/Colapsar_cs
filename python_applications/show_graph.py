from services.importer import Importer
from services.show_graph import show_sintetic_graph as show_graph


# g = Importer.import_graph_from_txt("/home/daniel/Documentos/LEC/MULTIPLAS od/python_applications/output/SmallWorldGraph_158_158_0.05_2018_6_21_15_42_24.txt")

# show_graph(g)

def main():
    import sys
  
    graph_name = sys.argv[1]
    # g = Importer.import_graph_from_txt("/home/danielaragao/Documents/Git/Colapsar/python_applications/output/graphs/" + graph_name + '.txt', show_progess=True)
    g = Importer.import_graph_from_txt( graph_name, show_progess=True)

    show_graph(g)


if __name__ == '__main__':
    main()