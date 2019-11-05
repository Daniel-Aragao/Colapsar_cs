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

rotina_por_grafo C RandomGraph_158_158_2019_10_8_0_5_32_1.txt
rotina_por_grafo BF RandomGraph_158_158_2019_10_8_0_5_32_1.txt

rotina_por_grafo C RegularGraph_158_158_2019_10_8_0_4_28_1.txt
rotina_por_grafo BF RegularGraph_158_158_2019_10_8_0_4_28_1.txt

rotina_por_grafo C ScaleFreeGraph_25000_2019_10_8_0_7_34_1.txt
rotina_por_grafo BF ScaleFreeGraph_25000_2019_10_8_0_7_34_1.txt

rotina_por_grafo C SmallWorldGraph_158_158_0.03_2019_10_8_0_6_44_1.txt
rotina_por_grafo BF SmallWorldGraph_158_158_0.03_2019_10_8_0_6_44_1.txt

rotina_por_grafo C SmallWorldGraph_158_158_0.3_2019_10_8_0_6_59_1.txt
rotina_por_grafo BF SmallWorldGraph_158_158_0.3_2019_10_8_0_6_59_1.txt