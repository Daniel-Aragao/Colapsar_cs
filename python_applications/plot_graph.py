import pandas as pd
import matplotlib.pyplot as plt

files = ["Fortaleza-network-osm-2019-1_1.txt", "Mumbai-network-osm-2019-1_2.txt", "Toquio-network-osm-2018-1_1.txt"]# "NY-network-osm-2018-1_1.txt", "Paris-network-osm-2019-1_1.txt"]
cities = ["Fortaleza", "Mumbai", "Tóquio"]#, "NY", "Paris"]
path = "D:/daniel/Documents/Git/Colapsar_cs/SearchConsoleApp/graphs/"

#Esquerda, direita, baixo, cima
borders = [(-38.6860, -38.3550, -3.8906, -3.6786), (72.7700, 73.0000, 18.9304, 19.2818), (138.9391, 140.0000, 35.4491, 35.9000)]#, (-74.2331, -73.6322, 40.5484, 40.9500), (2.2000, 2.4800, 48.8000, 48.9200)]

def plot(borders, df_file, city, i):
    fig, ax = plt.subplots(figsize=(15, 10))
    # ax = plt.subplot(2, 2, i+1) if i != 2 else plt.subplot(2, 1, 2)

    plt.title(city) 
    ax.scatter(df_file.longitude, df_file.latitude, zorder=1, alpha=0.2, c='b', s=1)
    ax.set_xlim(borders[0], borders[1])
    ax.set_ylim(borders[2], borders[3])
    # plt.savefig(city + ".png")
    # plt.show()


for i, file in enumerate(files):
    txt = open(path + file, "r")
    city = cities[i]
    border = borders[i]
    latitudes = []
    longitudes = []

    for line in txt:
        line = line.strip()

        if line == "edges":
            break

        if line != "nodes":
            campos = line.split(",")
            latitudes.append(float(campos[6]))
            longitudes.append(float(campos[8]))

    df_file = pd.DataFrame()
    df_file["latitude"] = latitudes
    df_file["longitude"] = longitudes

    plot(border, df_file, city, i)

plt.legend()
plt.show()