import re
import codecs
import math
from services.exporter import Exporter

# re_line_format = re.compile(r'(\d+),(\d+),(?:Erro:|null),?(\d+.?\d*),([^,]+)')
# re_line_format = re.compile(r'(\d+),(\d+),(?:Erro:|\d+,\d+,\d+),?(\d+.?\d*),([^,\n]+)')
re_line_format = re.compile(r'(\d+),(\d+),(Found|NotFound|FailOnRouteBuilding|SourceOrTargetDoNotExist|SourceAndTargetAreToCloseToCollapse|SourceAndTargetAreEqual|UnexpectedException)(?:,(\d+))?(?:,(\d+))?(?:,(\d+.?\d*))?(?:,(\d+.?\d*))?,([^\n]+)')

def __get_pairs_output(path):
    dict_od = {}
    counter = 0
    lines = -1

    with codecs.open(path, 'r',encoding='utf-8', errors='ignore') as file_obj_fb:
        for line in file_obj_fb:
            lines += 1
            match_fb = re_line_format.match(line)

            if match_fb:
                src = match_fb.group(1)
                dest = match_fb.group(2)
                status = match_fb.group(3)
                saltos = match_fb.group(4)
                expansoes = match_fb.group(5)
                tempo = match_fb.group(6)
                distance = match_fb.group(7)
                err_name_or_path = match_fb.group(8)

                line_obj = {
                    'distance': distance, 
                    'err_or_path': err_name_or_path, 
                    'status': status,
                    'saltos': saltos,
                    'expansoes': expansoes,
                    'tempo': tempo
                }

                if src in dict_od:
                    dict_od[src][dest] = line_obj
                else:
                    dict_od[src] = {dest: line_obj}
                counter += 1
            # else:
                # print("Didn't match: " + line)

    # print('matched ' + str(counter) + "/" + str(lines))
    return dict_od

def __is_float(string: str):
    try:
        float(string)
        return True
    except Exception as e:
        return False

def __is_error(string):
    if string == 'Found':
        return False
    # if __is_float(string):
    #     return False
    
    return True

def get_pair_value(src, tgt, dict_ods):
    if src in dict_ods:
        if tgt in dict_ods[src]:
            return dict_ods[src][tgt]['distance']
    
    return None

def compare(colapsar_path, brute_force_path):
    bruteForce = __get_pairs_output(brute_force_path)
    colapsar = __get_pairs_output(colapsar_path)

    errors_counter = 0
    hits_counter = 0
    fail_counter = 0
    ods_counter = 0
    fail_accum = 0
    max_fail = 0
    not_match = 0
    tempo_Collapse = 0
    tempo_FB = 0
    hist = []

    for colapsar_src in colapsar:
        colapsar_targets = colapsar[colapsar_src]

        for colapsar_target in colapsar_targets:
            colapsar_distance = get_pair_value(colapsar_src, colapsar_target, colapsar)
            
            fb_distance = get_pair_value(colapsar_src, colapsar_target, bruteForce)

            if fb_distance:
                ods_counter += 1

                if not __is_error(bruteForce[colapsar_src][colapsar_target]['status']): 
                    if not __is_error(colapsar[colapsar_src][colapsar_target]['status']):
                        fb_distance = float(fb_distance)
                        colapsar_distance = float(colapsar_distance)

                        abs_difference = math.fabs(fb_distance - colapsar_distance)
                        tempo_Collapse += float(colapsar[colapsar_src][colapsar_target]['tempo'])
                        tempo_FB += float(bruteForce[colapsar_src][colapsar_target]['tempo'])

                        if abs_difference < 0.0001:
                            hits_counter += 1
                        else:
                            fail_counter += 1
                            fail_accum += abs_difference

                            hist.append(abs_difference)

                            if max_fail < abs_difference:
                                max_fail = abs_difference
                                # print(colapsar_src, colapsar_target)
                    else:
                        # fail_counter += 1
                        ods_counter -= 1
                        errors_counter += 1

                else:
                    ods_counter -= 1
                    errors_counter += 1

                    # if not __is_error(colapsar_value):
                    #     print("ERRO: Brute fource founded an error, but colapsar don't")
            else:
                # print("ERRO: ", colapsar_src, colapsar_target, "not matching to bruteforce")
                not_match+=1
    
    return ({
        'not_match': not_match,
        'errors_counter': errors_counter,
        'hits_counter': hits_counter,
        'fail_counter': fail_counter,
        'ods_counter': ods_counter,
        'fail_accum_avarage': fail_accum/fail_counter if fail_counter else 0,
        'max_fail': max_fail,
        'Acurácia': (1 - (fail_counter/ods_counter)) * 100 if ods_counter else 0,
        'tempo_fb': tempo_FB,
        'tempo_colapsar': tempo_Collapse
    }, hist)


graphs_sources = [
    "Fortaleza-network-osm-2019-1_1-Collapse-10000-50-8.txt",
    "Fortaleza-network-osm-2019-1_1-Collapse-10000-100-8.txt",
    "Fortaleza-network-osm-2019-1_1-Collapse-10000-150-8.txt",
    "Fortaleza-network-osm-2019-1_1-Collapse-10000-200-8.txt",
    "Fortaleza-network-osm-2019-1_1-Collapse-10000-250-8.txt"
]
default_path_city = 'Fortaleza-output'
graphs_targets = [
    "Fortaleza-network-osm-2019-1_1-BruteForce-10000-50-8.txt",
    "Fortaleza-network-osm-2019-1_1-BruteForce-10000-100-8.txt",
    "Fortaleza-network-osm-2019-1_1-BruteForce-10000-150-8.txt",
    "Fortaleza-network-osm-2019-1_1-BruteForce-10000-200-8.txt",
    "Fortaleza-network-osm-2019-1_1-BruteForce-10000-250-8.txt"
]

#graphs_sources = [
#    "Mumbai-network-osm-2019-1_2-Collapse-10000-50-8.txt",
#    "Mumbai-network-osm-2019-1_2-Collapse-10000-100-8.txt",
#   "Mumbai-network-osm-2019-1_2-Collapse-10000-150-8.txt",
#    "Mumbai-network-osm-2019-1_2-Collapse-10000-200-8.txt",
#    "Mumbai-network-osm-2019-1_2-Collapse-10000-250-8.txt"
#]
#default_path_city = 'Mumbai-output'
#graphs_targets = [
#    "Mumbai-network-osm-2019-1_2-BruteForce-10000-50-8.txt",
#    "Mumbai-network-osm-2019-1_2-BruteForce-10000-100-8.txt",
#    "Mumbai-network-osm-2019-1_2-BruteForce-10000-150-8.txt",
#    "Mumbai-network-osm-2019-1_2-BruteForce-10000-200-8.txt",
#    "Mumbai-network-osm-2019-1_2-BruteForce-10000-250-8.txt"
#]

#graphs_sources = [
#    "Toquio-network-osm-2018-1_1-Collapse-10000-50-8.txt",
#    "Toquio-network-osm-2018-1_1-Collapse-10000-100-8.txt",
#    "Toquio-network-osm-2018-1_1-Collapse-10000-150-8.txt",
#    "Toquio-network-osm-2018-1_1-Collapse-10000-200-8.txt",
#    "Toquio-network-osm-2018-1_1-Collapse-10000-250-8.txt"
#]
#default_path_city = 'Toquio-output'
#graphs_targets = [
#    "Toquio-network-osm-2018-1_1-BruteForce-10000-50-8.txt",
#    "Toquio-network-osm-2018-1_1-BruteForce-10000-100-8.txt",
#    "Toquio-network-osm-2018-1_1-BruteForce-10000-150-8.txt",
#    "Toquio-network-osm-2018-1_1-BruteForce-10000-200-8.txt",
#    "Toquio-network-osm-2018-1_1-BruteForce-10000-250-8.txt"
#]

if __name__ == '__main__':
    import sys
    #default_path = '/media/Storage/Documentos/colapsar/colapsar_2019/saida/'
    #aws96/LOG-\(2018_10_06_19_01_42\)-MultithreadManager-.txt
    # python3 error_comparison.py aws96/20000-100.0-Toquio-network-osm-2018-1.txt aws96/BruteForce-20000-100.0-Toquio-network-osm-2018-1.txt
    # info, hist = compare(default_path+sys.argv[1], default_path+sys.argv[2])

    #default_path = 'C:\\Users\\danie\\Desktop\\mumbai-toquio-fortaleza-logs\\' + default_path_city
    default_path = '/home/danielfilhoce/mumbai-toquio-fortaleza-logs/' #+ default_path_city + '/'

    for i in range(5):
        info, hist = compare(default_path + graphs_sources[i], default_path + graphs_targets[i])
        print(info)

    # Exporter.printToFile(",".join([str(i) for i in hist]), "distances_diff")


# matched 19456
# matched 19999
