from services.GraphicsGenerator import GraphicsGenerator
import numpy as np
import matplotlib.pyplot as plt
from matplotlib.pyplot import figure
"""
corrigir gráfico de tempo

adicionar gráfic de radar das cidades em relação ao dos grafos base

comparar com os sintáticos
"""

save_img = False

def save_or_show(title):
    if save_img:
        # plt.tight_layout()
        # fig_size = plt.rcParams["figure.figsize"]
        # fig_size[0] = 15
        # fig_size[1] = 9
        # plt.rcParams["figure.figsize"] = fig_size
        plt.savefig("./output/img/graphics/" + title+ ".png", format="PNG")
        plt.close()
    else:
        plt.show()
    

def show_time_graphics(title, x, ys, show=True):
    """
    3(a) Comparação de Tempo
        Eixos:
        Raio X Tempo(min)
        Retas:
            Colapsar
            Força Bruta
    """
    title = 'Tempo de ' + title
    plt.title(title)
    plt.xlabel("Raio")
    plt.ylabel("Minutos")
    #plt.yscale('log')
    #plt.ylim(10**1, 10**6)

    gg = GraphicsGenerator(x)
    gg.add_y(ys[0]).add_legend("Estrategia Colapsar")
    gg.add_y(ys[1]).add_legend("Estrategia Força Bruta")
    gg.build_plt(plt)
    #gg.log_y().log_x().build_plt(plt)
    
    plt.legend()

    if show:
        save_or_show(title)

def show_colapsed_nodes_graphics(title, x, ys, show=True):
    """
    4 Indicadores Colapsar
        Eixos:
            Raio X Indicador
        (a)
            Indicador:
                Quantidade de nós colapsados
    """
    title = 'Nós Colapsados em ' + title
    plt.title(title)
    plt.xlabel("Raio")
    plt.ylabel("Nós colapsados")

    gg = GraphicsGenerator(x).add_y(ys[0]).build_plt(plt)

    if show:
        save_or_show(title)


def show_avarage_error(title, x, ys, show=True):
    """
    4 Indicadores Colapsar
        Eixos:
            Raio X Indicador
        (b)
            Indicador:
                Erro médio em metros
    """
    title = 'Erro médio em metros de ' + title
    plt.title(title)
    plt.xlabel("Raio")
    plt.ylabel("Erro médio")

    gg = GraphicsGenerator(x).add_y(ys[0]).build_plt(plt)

    if show:
        save_or_show(title)

def show_maximum_error(title, x, ys, show=True):
    """
    4 Indicadores Colapsar
        Eixos:
            Raio X Indicador
        (c)
            Indicador:
                Erro máximo em metros
    """
    title = 'Erro máximo em metros de ' + title
    plt.title(title)
    plt.xlabel("Raio")
    plt.ylabel("Erro máximo")

    gg = GraphicsGenerator(x).add_y(ys[0]).build_plt(plt)

    if show:
        save_or_show(title)

def show_acuracy(title, x, ys, show=True):
    """
    4 Indicadores Colapsar
        Eixos:
            Raio X Indicador
        (d)
            Indicador:
                accuracy(%)
    """
    title = 'Acurácia (%) de ' + title
    plt.title(title)
    plt.xlabel("Raio")
    plt.ylabel("Acurácia")

    gg = GraphicsGenerator(x).add_y(ys[0]).build_plt(plt)

    if show:
        save_or_show(title)


# {'not_match': 0, 'errors_counter': 114, 'hits_counter': 8550, 'fail_counter': 1336, 'ods_counter': 9886, 'fail_accum_avarage': 13.315723576632085, 'max_fail': 74.76390774849096,  'accuracy': 86.48593971272507}
# {'not_match': 0, 'errors_counter': 105, 'hits_counter': 6813, 'fail_counter': 3082, 'ods_counter': 9895, 'fail_accum_avarage': 25.408246311466765, 'max_fail': 171.43136587433582, 'accuracy': 68.85295603840324}
# {'not_match': 0, 'errors_counter': 97,  'hits_counter': 6419, 'fail_counter': 3484, 'ods_counter': 9903, 'fail_accum_avarage': 32.848994883191935, 'max_fail': 261.7655107865412,  'accuracy': 64.81874179541553}
# {'not_match': 0, 'errors_counter': 88,  'hits_counter': 6323, 'fail_counter': 3589, 'ods_counter': 9912, 'fail_accum_avarage': 38.50775939134173,  'max_fail': 334.76279334406354, 'accuracy': 63.79136400322841}
# {'not_match': 0, 'errors_counter': 91,  'hits_counter': 6025, 'fail_counter': 3884, 'ods_counter': 9909, 'fail_accum_avarage': 39.097013319081874, 'max_fail': 339.26362815394896, 'accuracy': 60.803310122111206}

# plt.subplot(2,3,1)
# show_time_graphics('Fortaleza', x_raio, [[88.805, 98.985, 112.125, 120.305, 129.317],[715.789, 5042.824, 20757.561, 46978.528, 104203.919]], show=False)
# # save_or_show("Painel Raio X Log(Tempo)")

# plt.subplot(2,3,2)
# show_colapsed_nodes_graphics("Fortaleza", x_raio, [[2, 8, 18, 30, 44]], show=False)
# # save_or_show("Painel Quantidade de nós colapsados")

# plt.subplot(2,3,3)
# show_avarage_error("Fortaleza", x_raio, [[13.315, 25.408, 32.848, 38.507, 39.097]], show=False)
# # save_or_show("Painel Erro médio em metros")

# plt.subplot(2,3,4)
# show_maximum_error("Fortaleza", x_raio, [[74.763, 171.431, 261.765, 334.762, 339.263]], show=False)
# # save_or_show("Painel Erro máximo em metros")

# plt.subplot(2,3,5)
# show_acuracy("Fortaleza", x_raio, [[86.485, 68.852, 64.818, 63.791, 60.803]], show=False)
# save_or_show("Painel accuracy(%)")

######################################################## grafos errados ########################################################
# ###################  3(a) Tempo ###################

# plt.subplot(2,3,1)
# show_time_graphics('Mumbai', x_raio, [[50.16,50.59,54.98,55.16,49],[478.8,1864.2,5047.8,10545.6,21024]], show=False)
# plt.subplot(2,3,2)
# show_time_graphics('Nova York', x_raio, [[451.072,316.035, 442.66, 345.137, 495.907],[22409.685, 126526.102, 357004.956, 860169.588, 1700298.174]], show=False)
# plt.subplot(2,3,3)
# show_time_graphics('Tóquio', x_raio, [[327.524, 217.549, 300.652, 326.709, 234.459],[23234.236, 99260.318, 158055, 319153.845, 572276.631]], show=False)
# plt.subplot(2,3,4)
# show_time_graphics('Paris', x_raio, [[121.913, 102.943, 119.482, 106.272, 115.947],[6805.139, 38582.932, 131713.944, 490877.749, 1283322.288]], show=False)
# plt.subplot(2,3,5)
# show_time_graphics('Fortaleza', x_raio, [[101.849, 98.144, 108.013, 105.552, 103.327], [1921.430, 10761.027, 37237.049, 93756.209, 202883.298]], show=False)
# save_or_show("Painel Raio X Log(Tempo)")

# ###################  4(a) Quantidade de nós colapsados ###################

# plt.subplot(2,3,1)
# show_colapsed_nodes_graphics("Mumbai", x_raio, [[3, 8, 14, 22, 31]], show=False)
# plt.subplot(2,3,2)
# show_colapsed_nodes_graphics("NY", x_raio, [[6, 15, 27, 44, 63]], show=False)
# plt.subplot(2,3,3)
# show_colapsed_nodes_graphics("Tóquio", x_raio, [[5, 13, 14, 31, 43]], show=False)
# plt.subplot(2,3,4)
# show_colapsed_nodes_graphics("Paris", x_raio, [[9, 24, 46, 75, 112]], show=False)
# plt.subplot(2,3,5)
# show_colapsed_nodes_graphics("Fortaleza", x_raio, [[2, 8, 18, 30, 44]], show=False)
# save_or_show("Painel Quantidade de nós colapsados")

# # ###################  4(b) Erro médio em metros ###################

# plt.subplot(2,3,1)
# show_avarage_error("Mumbai", x_raio, [[15.235819299009922, 27.04257169011056, 33.3248917171415, 37.91942956740938, 38.79603620677259]], show=False)
# plt.subplot(2,3,2)
# show_avarage_error("NY", x_raio, [[15.625560166218628, 31.62699306791572, 41.108753169825526, 49.61890660207496, 56.45227697453647]], show=False)
# plt.subplot(2,3,3)
# show_avarage_error("Tóquio", x_raio, [[19.277359922333783, 88.48101997073694, 42.990977152494395, 47.8648879505157, 47.01677643895114]], show=False)
# plt.subplot(2,3,4)
# show_avarage_error("Paris", x_raio, [[14.740349807577003, 27.73187183427684, 36.0254347187759, 37.62301465484064, 40.61796513482687]], show=False)
# plt.subplot(2,3,5)
# show_avarage_error("Fortaleza", x_raio, [[15.214554306052726, 29.125933274303367, 33.36929376647188, 39.02207549970293, 40.08146793919004]], show=False)
# save_or_show("Painel Erro médio em metros")

# ###################  4(c) Erro máximo em metros ###################

# plt.subplot(2,3,1)
# show_maximum_error("Mumbai", x_raio, [[85.24237522231488, 153.74876612962362, 230.46811020138193, 285.339823848939, 310.94615046111926]], show=False)
# plt.subplot(2,3,2)
# show_maximum_error("NY", x_raio, [[81.90734720932232, 184.79595645034715, 256.661655154594, 398.3472448297507, 448.3190085818642]], show=False)
# plt.subplot(2,3,3)
# show_maximum_error("Tóquio", x_raio, [[90.48188667815703, 17000.010000000002, 284.3740990017068, 390.30890629388523, 435.726160918568]], show=False)
# plt.subplot(2,3,4)
# show_maximum_error("Paris", x_raio, [[77.42669244302488, 171.25889534370253, 226.32904061396675, 261.38861714422205, 257.7793026421623]], show=False)
# plt.subplot(2,3,5)
# show_maximum_error("Fortaleza", x_raio, [[73.06571936807632, 204.00468334135167, 245.97652388323513, 338.64287880026495, 400.4723079268392]], show=False)
# save_or_show("Painel Erro máximo em metros")

# # ###################  4(d) accuracy(%) ###################

# plt.subplot(2,3,1)
# show_acuracy("Mumbai", x_raio, [[96.0198009900495, 94.30971548577429, 93.10965548277413, 92.15960798039902, 91.14455722786138]], show=False)
# plt.subplot(2,3,2)
# show_acuracy("NY", x_raio, [[94.96474823741187, 88.03940197009851, 83.69918495924796, 79.49397469873493, 75.50758159856078]], show=False)
# plt.subplot(2,3,3)
# show_acuracy("Tóquio", x_raio, [[97.27486374318715, 95.78059071729957, 95.1347567378369, 94.92474623731187, 94.23971198559929]], show=False)
# plt.subplot(2,3,4)
# show_acuracy("Paris", x_raio, [[94.4197209860493, 87.964398219911, 83.38422391857506, 81.16905845292266, 79.73831300813008]], show=False)
# plt.subplot(2,3,5)
# show_acuracy("Fortaleza", x_raio, [[96.41482074103706, 90.4695234761738, 87.33936696834841, 85.5642782139107, 83.77918895944796]], show=False)
# save_or_show("Painel accuracy(%)")




# MUMBAI
# {'errors_counter': 0, 'hits_counter': 19203, 'fail_counter': 796, 'ods_counter': 19999, 'fail_accum_avarage':  15.235819299009922, 'max_fail':85.24237522231488, 'accuracy': 96.0198009900495}
# {'errors_counter': 0, 'hits_counter': 18861, 'fail_counter': 1138, 'ods_counter': 19999, 'fail_accum_avarage': 27.04257169011056, 'max_fail': 153.74876612962362, 'accuracy': 94.30971548577429}
# {'errors_counter': 0, 'hits_counter': 18621, 'fail_counter': 1378, 'ods_counter': 19999, 'fail_accum_avarage': 33.3248917171415, 'max_fail':  230.46811020138193, 'accuracy': 93.10965548277413}
# {'errors_counter': 0, 'hits_counter': 18431, 'fail_counter': 1568, 'ods_counter': 19999, 'fail_accum_avarage': 37.91942956740938, 'max_fail': 285.339823848939, 'accuracy': 92.15960798039902}
# {'errors_counter': 0, 'hits_counter': 18228, 'fail_counter': 1771, 'ods_counter': 19999, 'fail_accum_avarage': 38.79603620677259, 'max_fail': 310.94615046111926, 'accuracy': 91.14455722786138}
# NY
# {'errors_counter': 0, 'hits_counter': 18992, 'fail_counter': 1007, 'ods_counter': 19999, 'fail_accum_avarage': 15.625560166218628, 'max_fail': 81.90734720932232, 'accuracy': 94.96474823741187}
# {'errors_counter': 0, 'hits_counter': 17607, 'fail_counter': 2392, 'ods_counter': 19999, 'fail_accum_avarage': 31.62699306791572, 'max_fail': 184.79595645034715, 'accuracy': 88.03940197009851}
# {'errors_counter': 0, 'hits_counter': 16739, 'fail_counter': 3260, 'ods_counter': 19999, 'fail_accum_avarage': 41.108753169825526, 'max_fail': 256.661655154594, 'accuracy': 83.69918495924796}
# {'errors_counter': 0, 'hits_counter': 15898, 'fail_counter': 4101, 'ods_counter': 19999, 'fail_accum_avarage': 49.61890660207496, 'max_fail': 398.3472448297507, 'accuracy': 79.49397469873493}
# {'not_match': 544, 'errors_counter': 0, 'hits_counter': 14690, 'fail_counter': 4765, 'ods_counter': 19455, 'fail_accum_avarage': 56.45227697453647, 'max_fail': 448.3190085818642, 'accuracy': 75.50758159856078}
# TOQUIO
# {'not_match': 0, 'errors_counter': 0, 'hits_counter': 19454, 'fail_counter': 545, 'ods_counter': 19999, 'fail_accum_avarage': 19.277359922333783, 'max_fail':  90.48188667815703, 'accuracy': 97.27486374318715}
# {'not_match': 9802, 'errors_counter': 0, 'hits_counter': 9761, 'fail_counter': 430, 'ods_counter': 10191, 'fail_accum_avarage': 88.48101997073694, 'max_fail': 17000.010000000002, 'accuracy': 95.78059071729957}
# {'not_match': 0, 'errors_counter': 0, 'hits_counter': 19026, 'fail_counter': 973, 'ods_counter': 19999, 'fail_accum_avarage': 42.990977152494395, 'max_fail':  284.3740990017068, 'accuracy': 95.1347567378369}
# {'not_match': 0, 'errors_counter': 0, 'hits_counter': 18984, 'fail_counter': 1015, 'ods_counter': 19999, 'fail_accum_avarage': 47.8648879505157, 'max_fail':   390.30890629388523, 'accuracy': 94.92474623731187}
# {'not_match': 0, 'errors_counter': 0, 'hits_counter': 18847, 'fail_counter': 1152, 'ods_counter': 19999, 'fail_accum_avarage': 47.01677643895114, 'max_fail':  435.726160918568, 'accuracy': 94.23971198559929}
# PARIS
# {'not_match': 0, 'errors_counter': 0, 'hits_counter': 18883, 'fail_counter': 1116, 'ods_counter': 19999, 'fail_accum_avarage': 14.740349807577003, 'max_fail':  77.42669244302488, 'accuracy': 94.4197209860493}
# {'not_match': 0, 'errors_counter': 0, 'hits_counter': 17592, 'fail_counter': 2407, 'ods_counter': 19999, 'fail_accum_avarage': 27.73187183427684, 'max_fail':   171.25889534370253, 'accuracy': 87.964398219911}
# {'not_match': 8209, 'errors_counter': 0, 'hits_counter': 9831, 'fail_counter': 1959, 'ods_counter': 11790, 'fail_accum_avarage': 36.0254347187759, 'max_fail':  226.32904061396675, 'accuracy': 83.38422391857506}
# {'not_match': 0, 'errors_counter': 0, 'hits_counter': 16233, 'fail_counter': 3766, 'ods_counter': 19999, 'fail_accum_avarage': 37.62301465484064, 'max_fail':   261.38861714422205, 'accuracy': 81.16905845292266}
# {'not_match': 12127, 'errors_counter': 0, 'hits_counter': 6277, 'fail_counter': 1595, 'ods_counter': 7872, 'fail_accum_avarage': 40.61796513482687, 'max_fail': 257.7793026421623, 'accuracy': 79.73831300813008}
# FORTALEZA
# {'not_match': 0, 'errors_counter': 0, 'hits_counter': 19282, 'fail_counter': 717, 'ods_counter': 19999, 'fail_accum_avarage': 15.214554306052726, 'max_fail': 73.06571936807632, 'accuracy': 96.41482074103706}
# {'not_match': 0, 'errors_counter': 0, 'hits_counter': 18093, 'fail_counter': 1906, 'ods_counter': 19999, 'fail_accum_avarage': 29.125933274303367, 'max_fail':204.00468334135167, 'accuracy': 90.4695234761738}
# {'not_match': 0, 'errors_counter': 0, 'hits_counter': 17467, 'fail_counter': 2532, 'ods_counter': 19999, 'fail_accum_avarage': 33.36929376647188, 'max_fail': 245.97652388323513, 'accuracy': 87.33936696834841}
# {'not_match': 0, 'errors_counter': 0, 'hits_counter': 17112, 'fail_counter': 2887, 'ods_counter': 19999, 'fail_accum_avarage': 39.02207549970293, 'max_fail': 338.64287880026495, 'accuracy': 85.5642782139107}
# {'not_match': 0, 'errors_counter': 0, 'hits_counter': 16755, 'fail_counter': 3244, 'ods_counter': 19999, 'fail_accum_avarage': 40.08146793919004, 'max_fail': 400.4723079268392, 'accuracy': 83.77918895944796}


FORTALEZA = [
    {'collapse': 2, 'fail_accum_avarage': 12.426113973237934, 'tempo_fb': 517.85527306, 'not_match': 0, 'fail_counter': 685, 'tempo_colapsar': 181.56331007166685, 'max_fail': 48.95949047866998, 'accuracy': 93.15, 'hits_counter': 9315, 'errors_counter': 0, 'ods_counter': 10000},
    {'collapse': 8, 'fail_accum_avarage': 21.13163395761892, 'tempo_fb': 3081.7144019450084, 'not_match': 0, 'fail_counter': 1682, 'tempo_colapsar': 366.81669978833196, 'max_fail': 93.39913323600013, 'accuracy': 83.17831783178318, 'hits_counter': 8317, 'errors_counter': 1, 'ods_counter': 9999},
    {'collapse': 18, 'fail_accum_avarage': 27.13345291171151, 'tempo_fb': 10966.311710078322, 'not_match': 0, 'fail_counter': 1948, 'tempo_colapsar': 661.5580813400021, 'max_fail': 144.15331791249992, 'accuracy': 80.51220488195278, 'hits_counter': 8048, 'errors_counter': 4, 'ods_counter': 9996},
    {'collapse': 30, 'fail_accum_avarage': 31.462662885016442, 'tempo_fb': 28726.287438643383, 'not_match': 0, 'fail_counter': 2054, 'tempo_colapsar': 1063.76529496833, 'max_fail': 170.98066947400002, 'accuracy': 79.44561192834985, 'hits_counter': 7939, 'errors_counter': 7, 'ods_counter': 9993},
    {'collapse': 44, 'fail_accum_avarage': 33.129955552810586, 'tempo_fb': 61540.12819426688, 'not_match': 0, 'fail_counter': 2237, 'tempo_colapsar': 1573.8171022100025, 'max_fail': 243.71476309040008, 'accuracy': 77.60984886397758, 'hits_counter': 7754, 'errors_counter': 9, 'ods_counter': 9991}

]

MUMBAI = [
    {'collapse': 3, 'hits_counter': 9731, 'fail_counter': 269, 'errors_counter': 0, 'ods_counter': 10000, 'accuracy': 97.31, 'tempo_fb': 240.16042855666643, 'max_fail': 48.077174488870014, 'not_match': 0, 'fail_accum_avarage': 12.949941976353388, 'tempo_colapsar': 78.37049173166659},
    {'collapse': 8, 'hits_counter': 9468, 'fail_counter': 532, 'errors_counter': 0, 'ods_counter': 10000, 'accuracy': 94.67999999999999, 'tempo_fb': 1036.31562541333, 'max_fail': 89.73649213309909, 'not_match': 0, 'fail_accum_avarage': 23.152410478951413, 'tempo_colapsar': 136.84106322166664},
    {'collapse': 14, 'hits_counter': 9252, 'fail_counter': 741, 'errors_counter': 7, 'ods_counter': 9993, 'accuracy': 92.58480936655658, 'tempo_fb': 2730.5924492916743, 'max_fail': 142.13799034807016, 'not_match': 0, 'fail_accum_avarage': 32.82263266270961, 'tempo_colapsar': 213.5744917950001},
    {'collapse': 22, 'hits_counter': 9113, 'fail_counter': 874, 'errors_counter': 13, 'ods_counter': 9987, 'accuracy': 91.24862321017324, 'tempo_fb': 5908.523805193329, 'max_fail': 194.53632354799015, 'not_match': 0, 'fail_accum_avarage': 39.65482481534233, 'tempo_colapsar': 302.5743641149998},
    {'collapse': 31, 'hits_counter': 8998, 'fail_counter': 986, 'errors_counter': 16, 'ods_counter': 9984, 'accuracy': 90.12419871794873, 'tempo_fb': 11219.929323826638, 'max_fail': 230.46811020137966, 'not_match': 0, 'fail_accum_avarage': 45.43544022961886, 'tempo_colapsar': 415.190224468334}
]

TOQUIO = [
    {'collapse': 5,'not_match': 0, 'errors_counter': 0, 'max_fail': 46.47022054449917, 'tempo_fb': 3821.3713241183245, 'hits_counter': 9869, 'fail_accum_avarage': 15.567177430797704, 'ods_counter': 10000, 'accuracy': 98.69, 'fail_counter': 131, 'tempo_colapsar': 386.98288287833356},
    {'collapse': 13,'not_match': 0, 'errors_counter': 1, 'max_fail': 93.2159635768985, 'tempo_fb': 16490.809682573334, 'hits_counter': 9786, 'fail_accum_avarage': 24.85103937882513, 'ods_counter': 9999, 'accuracy': 97.86978697869787, 'fail_counter': 213, 'tempo_colapsar': 635.3481389100003},
    {'collapse': 14,'not_match': 0, 'errors_counter': 3, 'max_fail': 131.0043007919012, 'tempo_fb': 42427.80495005489, 'hits_counter': 9718, 'fail_accum_avarage': 34.6504897195566, 'ods_counter': 9997, 'accuracy': 97.20916274882465, 'fail_counter': 279, 'tempo_colapsar': 915.5648975883321},
    {'collapse': 31,'not_match': 0, 'errors_counter': 4, 'max_fail': 185.9203764082995, 'tempo_fb': 87063.29515659009, 'hits_counter': 9716, 'fail_accum_avarage': 38.93959929014586, 'ods_counter': 9996, 'accuracy': 97.19887955182072, 'fail_counter': 280, 'tempo_colapsar': 1235.8727759549924},
    {'collapse': 43,'not_match': 0, 'errors_counter': 6, 'max_fail': 200.90280422159776, 'tempo_fb': 167587.4816306259, 'hits_counter': 9628, 'fail_accum_avarage': 37.50007116010323, 'ods_counter': 9994, 'accuracy': 96.33780268160896, 'fail_counter': 366, 'tempo_colapsar': 1629.5005396833349}
]

x_raio = [50,100,150,200,250]
CITIES = [FORTALEZA, MUMBAI, TOQUIO]
CITIES_NAMES = [
    "Fortaleza",
    "Mumbai",
    "Tóquio"
]
FUNCTIONS = {
    #"tempo_colapsar,tempo_fb": show_time_graphics,
    #"collapse": show_colapsed_nodes_graphics,
    #"fail_accum_avarage": show_avarage_error,
    #"max_fail": show_maximum_error,
    "accuracy": show_acuracy
}
FUNCTIONS_NAMES = [
    #"Painel Raio X Tempo",
    #"Painel Quantidade de nós colapsados",
    #"Painel Erro médio em metros",
    #"Painel Erro máximo em metros",
    "Painel Acurácia(%)"
]

funcs = len(FUNCTIONS)
rows = 1
columns = 3

def printByCity():
    for j, city in enumerate(CITIES):
        for i, func in enumerate(FUNCTIONS):
            plt.subplot(rows,columns, i + 1)
        
            if ',' in func:
                func1,func2 = func.split(',')
                FUNCTIONS[func](CITIES_NAMES[j], x_raio, [[k[func1] for k in city], [k[func2] for k in city]], show=False)
            else:
                FUNCTIONS[func](CITIES_NAMES[j], x_raio, [[k[func] for k in city]], show=False)

        save_or_show('Painel de ' + CITIES_NAMES[j])


def printByFunction():
    for i, func in enumerate(FUNCTIONS):
        for j, city in enumerate(CITIES):
            plt.subplot(rows, columns, j + 1)
        
            if ',' in func:
                func1,func2 = func.split(',')
                FUNCTIONS[func](CITIES_NAMES[j], x_raio, [[k[func1] for k in city], [k[func2] for k in city]], show=False)
            else:
                FUNCTIONS[func](CITIES_NAMES[j], x_raio, [[k[func] for k in city]], show=False)
        
        save_or_show(FUNCTIONS_NAMES[i])

#printByCity()
printByFunction()


#show_acuracy("Fortaleza", x_raio, [[86.485, 68.852, 64.818, 63.791, 60.803]], show=False)
