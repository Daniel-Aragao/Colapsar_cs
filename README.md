# Colapsar_cs

People most commonly use searchs between two points in the graph, for example get home from work by car have two points 
that should be the origin, work, and the destination, home.

This algorithmns uses the principles of searchs but between two regions, a set of points and not just one, as origin 
and destination. The pratical example is a bus network where an user should chose one bus stop to get in the bus and 
another to leave the bus, these two are origin and destination respectively using regions when the user don't know where 
is the best bus stop to use in a radius from where he is and how far he is willing to walk until he get to a bus stop. 
The same is valid when he don't know where to leave the bus trying to go home and have some specific disposition to 
walk there.

This project can be associated as a research project provided by LEC (Laboratório de Engenharia do Conhecimento in 
portuguese that means Knowledge Engineering Laboratory) and it's being developed as my monography. When the project is 
finalized the results will be presented here.

# Using
install the right version of dotnet core and runs from "SearchConsoleApp" folder

> $dotnet run [params]

where [params] are explained in the program description below:

### Description 
        This program it's for research purpose and search the best path between two informed regions (set of nodes)
        using the informed strategy and the graph also informed. It is also needed the radius to build the region and the number of
        pairs of origin and destinations to make data to the research. May also be informed the number of threads of execution
        to run the program (the default depends on the machine that is running this script)

### Legend 
        1. None "< or >" must be used, they are just to delimiter parameters
        2. Arguments
                (*) <mandatory arguments>
                (#) <default arguments> (also optional) #default value
                ( ) <optional arguments>
                    <any kind of argument> :type
                    <any kind of argument> (description)
        3. The order of the parameters must be followed (you can
        use "i" to avoid change default values or to ignore optional arguments)

### Arguments 
        (*) <Strategy> : string (<C> to Collapse, <BF> to BruteForce)
        (*) <File name> : string #./graphs/<File name>
        (*) <Distance> :double (radius to search)
        (*) <OD size> :int (number of Origin and Destination to run) #./ODs/<OD size>
        (#) <Number of threads to use> :int #4 (The number of logical processors in this machine)
        (#) <Log to file> :bool #True (<!> to negate the default value) (Whether the log should be written only to the console or to a file as well)
        ( ) <don't use default paths> :bool(<t> to true) (to use just the path on the <File path> argument when searching for graphs)
