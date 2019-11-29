import re
import codecs

# def total_time(text, metros, total_de_ods=50000):
#     textsplit = text.split(' - ')
    
#     nodes = textsplit[0]
#     time = textsplit[1]

#     percent_of_nodes = float(nodes)/total_de_ods

#     time_minutes = float(time.split(' ')[0])
#     hours = time_minutes/60
#     days = hours/24

#     hours_estimative = hours / percent_of_nodes
#     days_estimative = hours_estimative / 24

#     formatOpt = '{0:.2f}'

#     view = '{0:.0f}'.format(percent_of_nodes * 100) + '% '
#     view += str(metros) + 'm '
#     view += formatOpt.format(hours) + 'h ('+ formatOpt.format(days) +' dias) '
#     view += 'estimativa ' + formatOpt.format(hours_estimative) + 'h (' + formatOpt.format(days_estimative) + ' dias)'
#     view += ' restam ' + formatOpt.format(days_estimative - days) + " dias"

#     print(view)


# if __name__ == '__main__':
#     import sys
#     # "19000 - 36495 minutos" 50 50000
#     if len(sys.argv) < 4:
#         total_time(sys.argv[1], sys.argv[2])
#     else:
#         total_time(sys.argv[1], sys.argv[2], float(sys.argv[3]))

def getEstimative(path):
    with codecs.open(path, 'r',encoding='utf-8', errors='ignore') as file_obj:
        # Armazenando dados do arquivo

        re_num_threads = re.compile(r'\s*Threads:\s(\d+)\s*')
        re_thread_declaration = re.compile(r'Thread:\s*(\d+)\s+ODs:\s*(\d+)\s*')
        re_thread_progress = re.compile(r'Progresso\s*\(T(\d+)\)\s*:\s*(\d+)%\s*-\s*(\d+)\s*-\s*(\d+)\s*minutos')
        re_thread_end = re.compile(r'Fim:\s*T(\d+)')
        re_arquivo = re.compile(r'\s*Arquivo:\s*(.+)')
        re_raio = re.compile(r'\s*Raio:\s*(.+)')
        re_ods = re.compile(r'\s*ODs:\s*(.+)')

        num_threads = 1
        threads = {}
        last_qtd = 0

        for line in file_obj:
            if re_thread_progress.match(line):
                match = re_thread_progress.match(line)

                thread_id = match.group(1)
                # program_progress_percent = int(match.group(2))
                program_progress_qtd = int(match.group(3))
                program_minutes = int(match.group(4))

                append_qtd = program_progress_qtd - last_qtd

                threads[thread_id]['progress_qtd'] += append_qtd
                threads[thread_id]['time_checkpoint'] = program_minutes

                last_qtd = program_progress_qtd
            
            elif re_thread_end.match(line):
                thread_id = re_thread_end.match(line).group(1)

                threads[thread_id]['ended'] = True

            elif re_thread_declaration.match(line):
                match = re_thread_declaration.match(line)

                thread_id = match.group(1)
                thread_size = match.group(2)

                threads[thread_id] = {'thread_size': int(thread_size), 'ended': False, 'progress_qtd': 0, 'time_checkpoint': 0}

            elif re_num_threads.match(line):
                num_threads = re_num_threads.match(line).group(1)
            
            elif re_arquivo.match(line):
                arquivo = re_arquivo.match(line).group(1)
            elif re_raio.match(line):
                raio = re_raio.match(line).group(1)
            elif re_ods.match(line):
                ods = re_ods.match(line).group(1)
        
        ##### Tempo

        total_time = 0
        average = 0
        count = 0
        zero_progress = 0

        for thread_id in threads:
            thread = threads[thread_id]
            count += 1

            if not thread["ended"]:
                progress_percent = thread['progress_qtd']/thread['thread_size']

                if progress_percent:
                    estimative = thread['time_checkpoint']/progress_percent
                    total_time += estimative
                else:
                    zero_progress += 1
            else:
                total_time += thread['time_checkpoint']
                average += thread['time_checkpoint']
                
        average = total_time/count
        for i in range(zero_progress):
            total_time += average

        return {'total_time': total_time, 'arquivo': arquivo, 'raio': raio, 'ods': ods}
        
    

if __name__ == '__main__':
    import sys
    default_path = '/home/daniel/Documentos/Git/Colapsar/caracterizacao-dados-reviews/Logs/'
    #aws96/LOG-\(2018_10_06_19_01_42\)-MultithreadManager-.txt

    info = getEstimative(default_path+sys.argv[1])

    print("Arquivo: " + str(info['arquivo']))
    print("Raio: " + str(info['raio']))
    print("Qtd de ODs: " + str(info['ods']))
    print("Tempo estimado: " + str(info['total_time']) + ' minutos')