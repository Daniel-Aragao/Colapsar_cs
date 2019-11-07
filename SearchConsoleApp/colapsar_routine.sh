#! /bin/bash

if ! [ -f progresso ]
then
    touch progresso
fi

rotina_por_grafo () {
    strategy=$1
    grafo=$2

    dotnet run $strategy $grafo 50 10000 && echo "Success $strategy $grafo 50" >> progresso || echo "Error $strategy $grafo 50" >> progresso
    dotnet run $strategy $grafo 100 10000 && echo "Success $strategy $grafo 100" >> progresso || echo "Error $strategy $grafo 100" >> progresso
    dotnet run $strategy $grafo 150 10000 && echo "Success $strategy $grafo 150" >> progresso || echo "Error $strategy $grafo 150" >> progresso
    dotnet run $strategy $grafo 200 10000 && echo "Success $strategy $grafo 200" >> progresso || echo "Error $strategy $grafo 200" >> progresso
    dotnet run $strategy $grafo 250 10000 && echo "Success $strategy $grafo 250" >> progresso || echo "Error $strategy $grafo 250" >> progresso

}



# rotina_por_grafo BF Fortaleza-network-osm-2019-1_1.txt
# rotina_por_grafo C Fortaleza-network-osm-2019-1_1.txt

# rotina_por_grafo BF Mumbai-network-osm-2019-1_2.txt
# rotina_por_grafo C Mumbai-network-osm-2019-1_2.txt

#rotina_por_grafo C NY-network-osm-2018-1_1.txt
#rotina_por_grafo BF NY-network-osm-2018-1_1.txt

#rotina_por_grafo C Toquio-network-osm-2018-1_1.txt
#rotina_por_grafo BF Toquio-network-osm-2018-1_1.txt

#rotina_por_grafo C Paris-network-osm-2019-1_1.txt
#rotina_por_grafo BF Paris-network-osm-2019-1_1.txt

rotina_por_grafo C RegularGraph_100_100_2019_11_6_23_48_53_1.txt
rotina_por_grafo BF RegularGraph_100_100_2019_11_6_23_48_53_1.txt

rotina_por_grafo C SmallWorldGraph_100_100_0.03_2019_11_6_23_54_41_1.txt
rotina_por_grafo BF SmallWorldGraph_100_100_0.03_2019_11_6_23_54_41_1.txt

rotina_por_grafo C SmallWorldGraph_100_100_0.3_2019_11_6_23_54_17_1.txt
rotina_por_grafo BF SmallWorldGraph_100_100_0.3_2019_11_6_23_54_17_1.txt

rotina_por_grafo C ScaleFreeGraph_10000_2019_11_6_23_55_3_1.txt
rotina_por_grafo BF ScaleFreeGraph_10000_2019_11_6_23_55_3_1.txt

rotina_por_grafo C RandomGraph_100_100_2019_11_6_23_53_14_1.txt
rotina_por_grafo BF RandomGraph_100_100_2019_11_6_23_53_14_1.txt