from services.generator import Generator
from services.exporter import Exporter
# from services.importer import Importer
from services.show_graph import show_sintetic_graph as show_graph
import math


def main():
    """
    A função recebe 4 argumentos

    Argumentos possíveis:    
        1. Tipo de grafo
            - regular |reg
            - random | ran
            - smallworld | sw
            - scalefree | sf
        2. Quantidade de nós
        3. Probabilidade de gerar arestas aleatórias
            - necessário para regular e smallworld, ignorado nos outros
            - varia de 0 a 1.0
        4. Exportar ou não o grafo
            - exporta quando respondido: true, True, t, yes, Yes, y, sim, Sim, s
            - qualquer outro valor não não exporta
        5. Mostrar grafo
            - show

    """
    import sys

    argsSize = len(sys.argv)

    if argsSize < 2:
        print('minimum params: graph_type qtd_nodes')
        return

    graph_type = sys.argv[1]
    qtd_nodes = int(sys.argv[2])

    probability = float(sys.argv[3] if argsSize > 3 else 0)
    m = int(math.sqrt(qtd_nodes))
    n = m

    if argsSize > 4 and sys.argv[4] and sys.argv[4] in ['True', 'true', 't', 'y', 'Y', 'yes', 'Yes', 's', 'S', 'sim', 'Sim']:
        export = Exporter 
    else:
        export = None

    if graph_type == 'regular' or graph_type == 'reg':
        g = Generator.create_regular_graph(m, n, probability, False, False, export)

    elif graph_type == 'random' or graph_type == 'ran':
        g = Generator.create_random_graph(m, n, export)

    elif graph_type == 'smallworld' or graph_type == 'sw':
        g = Generator.create_small_world_graph(m, n, probability, False, False, export)

    elif graph_type == 'scalefree' or graph_type == 'sf':
        g = Generator.create_scale_free_graph(qtd_nodes, export)

    if argsSize > 5 and sys.argv[5]:
        show_graph(g)

if __name__ == '__main__':
    main()

# g = Generator.create_regular_graph(158, 158, exporter=Exporter)
# g = Generator.create_random_graph(158, 158, exporter=Exporter)
# g = Generator.create_scale_free_graph(25000, exporter=Exporter)
# g = Generator.create_small_world_graph(158, 158, 0.05, exporter=Exporter)

# g = Generator.create_regular_graph(22, 22, exporter=Exporter)
# # show_graph(g)

# g = Generator.create_small_world_graph(22, 22, 0.05, exporter=Exporter)
# g = Importer.import_graph_from_txt("/home/daniel/Documentos/LEC/MULTIPLAS od/python_applications/output/SmallWorldGraph_158_158_0.05_2018_6_21_15_42_24.txt")

# show_graph(g)

# g = Generator.create_regular_graph(5, 10)
# g = Generator.create_random_graph(5, 10)
# g = Generator.create_small_world_graph(10, 10, 0.05, False, False)
# g = Generator.create_scale_free_graph(100)

# for i in [50]:    
# Generator.create_random_graph(i, i, exporter=Exporter)
# g = Generator.create_regular_graph(i,i)
# Generator.create_small_world_graph(i,i,0.3, exporter=Exporter)
# g = Generator.create_scale_free_graph(i*i)
# show_graph(g)