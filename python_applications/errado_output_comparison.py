import matplotlib
import matplotlib.pyplot as plt
import numpy as np
import os
from services.exporter import Exporter as exporter


def fileLogger(txt):
    print(txt)
    exporter.printToFile(txt, "log")

def logger(txt=""):
    print(txt)

lines_cache = {}

def get_lines(file):
    if file in lines_cache:
        return lines_cache[file]
    
    
    fileObject1 = open(file, 'rb')
    file_lines = fileObject1.readlines()
    fileObject1.close()

    lines_cache[file] = file_lines

    return file_lines

def is_erro(erro_array):
    if len(erro_array) >= 2:
        return True
    
    return False


def find_constrat(file1, file2, showdifferences=False):
    contrast_list = []
    file1_lines = get_lines(file1)
    file2_lines = get_lines(file2)
    error_total = 0
    error_percent_total = 0
    biggest_delta = 0


    for index, file1_line in enumerate(file1_lines):
        if index == 0:
            continue

        file2_line = file2_lines[index]

        striped_file1_line = str(file1_line).strip(r"b|\\n|\n|'|\"").split(',')
        striped_file2_line = str(file2_line).strip(r"b|\\n|\n|'|\"").split(',')

        source1 = striped_file1_line[0]
        source2 = striped_file2_line[0]

        target1 = striped_file1_line[1]
        target2 = striped_file2_line[1]

        erro1 = striped_file1_line[2].split(':')
        erro2 = striped_file2_line[2].split(':')

        distance1 = striped_file1_line[3]
        distance2 = striped_file2_line[3]

        # Caso de erro
        if is_erro(erro1) or is_erro(erro2):
            if not (is_erro(erro1) and is_erro(erro2)):
                if is_erro(erro1):
                    raise Exception("file1 apresentou o erro: "+str(erro1))

                elif is_erro(erro2):
                    raise Exception("file2 apresentou o erro: "+str(erro2))

        elif source1 != source2:
            raise Exception("O source "+source1+" do file1 é diferente do source "+source2+" do file2")
        
        elif target1 != target2:
            raise Exception("O target "+target1+" do file1 é diferente do target "+target2+" do file2")
        
        else:
            d1 = float(distance1)
            d2 = float(distance2)

            delta = abs(d2 - d1)

            if delta > 00.0001:
                if biggest_delta < delta:
                    biggest_delta = delta

                error_total += delta
                error_percent_total += d1/d2 if d1 < d2 else d2/d1
                contrast_list.append("A distância "+distance1+" do file1 é diferente da distância "+distance2+" do file2")
    
    if showdifferences:
        for difference in contrast_list:
            logger(difference)
    
    file_size = len(file1_lines) - 1 # - cabeçalho
    erro_medio = error_total/file_size
    erro_percent_medio = error_percent_total/file_size

    qtd_error = len(contrast_list)
    
    logger('\nTotal de diferenças entre o arquivo (file1) \n' + file1 + '\ne o arquivo (file2)\n' + file2 + '\né de ' + str(qtd_error))
    logger('Erro total: ' + str(error_total))
    logger('Erro máximo: ' + str(biggest_delta))
    logger('Erro médio: ' + str(erro_medio))
    logger('Erro percentual médio: ' + str(erro_percent_medio * 100))
    logger('Acurácia: ' + str((1 - (qtd_error / file_size)) * 100))


def compare_erros(file1, file2):
    file1_lines = get_lines(file1)
    error_list1 = []
    counter1 = 0

    file2_lines = get_lines(file2)
    error_list2 = []
    counter2 = 0

    def test_erro(erro_array1, erro_array2):
        erro1 = None
        erro2 = None

        if len(erro_array1) >= 2:
            erro1 = erro_array1[1]

        if len(erro_array2) >= 2:
            erro2 = erro_array2[1]

        if erro1 and erro2 is None:
            return True

    for index, file1_line in enumerate(file1_lines):
        file2_line = file2_lines[index]

        striped_file1_line = str(file1_line).strip(r"b|\\n|\n|'|\"").split(',')
        striped_file2_line = str(file2_line).strip(r"b|\\n|\n|'|\"").split(',')

        file1_erro_array = striped_file1_line[2].split(':')
        file2_erro_array = striped_file2_line[2].split(':')

        if test_erro(file1_erro_array, file2_erro_array):
            counter1 +=1
            error_list1.append((file1_line, file2_line))

        if test_erro(file2_erro_array, file1_erro_array):
            counter2 +=1
            error_list2.append((file2_line, file1_line))


    def show_result(file1, file2, counter, error_list):
        logger(file2+' \npossui ' + str(counter) + ' erros a mais que \n'+file1 + '\n')
        if counter:
            logger('Erros: ')
        for item in error_list:
            logger(item[0])
            logger(item[1])
            logger('')
    
    show_result(file2, file1, counter1, error_list1)
    logger('------------------------------------------------------------------------------------------------\n')
    show_result(file1, file2, counter2, error_list2)
    

def calcular_erros_percent(filename):
    with open(filename, 'rb') as fileObject:
        counter = 0
        error_count = 0
        error_dict = {}

        for line in fileObject.readlines():
            striped_line = str(line).strip(r"b|\\n|\n|'|\"").split(',')
            counter += 1

            if len(striped_line) >= 3:
                erro_array = striped_line[2].split(':')

                if len(erro_array) >= 2:
                    error_count += 1
                    error_number = erro_array[1]

                    if  error_number not in error_dict:
                        error_dict[error_number] = 0
                    
                    error_dict[error_number] += 1

        logger(filename)
        logger('Total de linhas: ' + str(counter))
        logger('Total de erros: ' + str(error_count))
        logger('Normal execution: ' + str((counter-error_count)/counter*100) + '%')
        
        for error_code in error_dict:
            error_qtd = float(error_dict[error_code])
            error_percent = error_qtd/counter

            logger('Error: ' + str(error_code) + ' => '+str(error_qtd) +' * 100 / ' + str(counter) + ' = ' + str(error_percent * 100) + '%' )

def show_accuracy(file1, file2, show_graphic=True, not_save=False, logger=logger):
    contrast_list = []
    file1_lines = get_lines(file1)
    file_size = len(file1_lines) - 1 # - cabeçalho

    file2_lines = get_lines(file2)
    error_total = 0
    error_percent_total = 0
    biggest_delta = 0

    total_lines = 1

    progressive_data = []
    progress = 10


    for index, file1_line in enumerate(file1_lines):
        if index == 0:
            continue

        file2_line = file2_lines[index]

        striped_file1_line = str(file1_line).strip(r"b|\\n|\n|'|\"").split(',')
        striped_file2_line = str(file2_line).strip(r"b|\\n|\n|'|\"").split(',')

        source1 = striped_file1_line[0]
        source2 = striped_file2_line[0]

        target1 = striped_file1_line[1]
        target2 = striped_file2_line[1]

        erro1 = striped_file1_line[2].split(':')
        erro2 = striped_file2_line[2].split(':')

        distance1 = striped_file1_line[3]
        distance2 = striped_file2_line[3]

        # Caso de erro
        if is_erro(erro1) or is_erro(erro2):
            if not (is_erro(erro1) and is_erro(erro2)):
                if is_erro(erro1):
                    raise Exception("file1 apresentou o erro: "+str(erro1))

                elif is_erro(erro2):
                    raise Exception("file2 apresentou o erro: "+str(erro2))

        elif source1 != source2:
            raise Exception("O source "+source1+" do file1 é diferente do source "+source2+" do file2")
        
        elif target1 != target2:
            raise Exception("O target "+target1+" do file1 é diferente do target "+target2+" do file2")
        
        else:
            d1 = float(distance1)
            d2 = float(distance2)

            delta = abs(d2 - d1)

            if delta > 00.0001:
                if biggest_delta < delta:
                    biggest_delta = delta

                error_total += delta
                error_percent_total += d1/d2 if d1 < d2 else d2/d1
                contrast_list.append("A distância "+distance1+" do file1 é diferente da distância "+distance2+" do file2")
        
        if index % progress == 0:
            erro_medio = error_total/total_lines
            erro_percent_medio = error_percent_total/total_lines

            qtd_error = len(contrast_list)
            total_acc = (1 - (qtd_error / total_lines)) * 100

            progressive_data.append((erro_medio, erro_percent_medio, total_acc, total_lines))
            
        total_lines += 1
    
    file_size = len(file1_lines) - 1 # - cabeçalho
    erro_medio = error_total/file_size
    erro_percent_medio = error_percent_total/file_size

    qtd_error = len(contrast_list)
    total_acc = (1 - (qtd_error / file_size)) * 100

    logger('\nTotal de diferenças entre o arquivo (file1) \n' + file1 + '\ne o arquivo (file2)\n' + file2 + '\né de ' + str(qtd_error))
    logger('Acurácia Total: ' + str(total_acc))
    logger('Erro total: ' + str(error_total))
    logger('Erro máximo: ' + str(biggest_delta))
    logger('Erro médio: ' + str(erro_medio))
    logger('Erro percentual médio Total: ' + str(erro_percent_medio * 100))
    logger('Total de Linhas Percorridas:' + str(total_lines))

    erros_medios = []
    erros_percents_medios = []
    totais_acc = []
    totais_lines = []

    for erro_medio, erro_percent_medio, total_acc, total_lines in progressive_data:
        erros_medios.append(erro_medio)
        erros_percents_medios.append(erro_percent_medio * 100)
        totais_acc.append(total_acc)

        totais_lines.append(total_lines)

    plt.subplot(3, 1, 1)
    plt.plot(totais_lines, erros_medios)
    plt.ylabel('Erros Médios')

    plt.subplot(3, 1, 2)
    plt.plot(totais_lines, erros_percents_medios)
    plt.ylabel('Erros Percentuais Médios')

    plt.subplot(3, 1, 3)
    plt.plot(totais_lines, totais_acc)
    plt.ylabel('Acurácia')

    plt.xlabel('Total de Linhas Percorridas')
    
    file2_array_name = os.path.basename(file2).split('-network')

    if not not_save:
        plt.savefig('python_applications/output/img/' + file2_array_name[0] + ".png")

    if show_graphic:
        plt.show()
    
def accuracyInfo():
    raios = ["50", "100", "250", "500"]
    cidades = ["Fortaleza", "Paris", "Toquio", 'NY', 'Mumbai']

    for cidade in cidades:
        for raio in raios:
            file1 = 'caracterizacao-dados-reviews/saida/250/BruteForce-20000-'+raio+'.0-'+cidade+'-network-osm-2018-1.txt'
            file2 = 'caracterizacao-dados-reviews/saida/250/20000-'+raio+'.0-'+cidade+'-network-osm-2018-1.txt'

            fileLogger("======================Montar Gráficos======================")
            fileLogger("======================"+raio+"="+cidade+"======================")
            try:
                show_accuracy(file1, file2, show_graphic=False, not_save=True, logger=fileLogger)
            except Exception:
                fileLogger("Não iniciado")

def specificInfo():
    folder = "caracterizacao-dados-reviews/saida/250/"
    ods = "20000"
    radius = "100.0"
    city = "Paris"

    filename = folder + 'BruteForce-'+ods+'-'+radius+'-'+city+'-network-osm-2018-1.txt'
    filename2 = folder + ods+'-'+radius+'-'+city+'-network-osm-2018-1.txt'
    # filename2 = 'caracterizacao-dados-reviews/saida/Admissible-50000-50.0-Fortaleza-network-osm-2018-1.txt'

    try:
        # logger("=====================Calcular erros(%)=====================")
        # calcular_erros_percent(filename)
        # logger()
        # calcular_erros_percent(filename2)

        # logger()
        # logger("======================Comparar erros=======================")
        # compare_erros(filename, filename2)

        # logger()
        # logger("===================Localizar diferenças====================")
        # find_constrat(filename, filename2, True)

        logger()
        logger("======================Montar Gráficos======================")
        show_accuracy(filename, filename2, show_graphic=True, not_save=True)

    except ZeroDivisionError:
        logger("Não iniciado")

specificInfo()
# accuracyInfo()
