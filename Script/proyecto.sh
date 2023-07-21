#!/bin/bash

#Definiciones de funciones

#Imprime variables importantes para este script. Especificamente,la ruta desde donde se esta ejectuando el script,
#la ruta donde esta ubicado el script, la ruta del directorio raiz del proyecto moogle (esta contiene al directorio que contiene al
#script)
function Rutas(){
    echo "Rutas importantes"
    echo "Ruta de llamada       $originalDirectory"
    echo "Ruta al script        $scriptDirectory"
    echo "Ruta al moogle        $moogleDirectory"
}

function Help(){
    echo "uso: proyecto.sh <opcion>"
    echo "Los parametros <arg> son necesarios, los parametros [arg] son opcionales."
    echo ""
    echo "Las opciones disponibles son:"
    echo ""
    echo "  clean                       Elimina ficheros generados en la compilacion de la presentacion, el informe, y  "
    echo "                              en la ejecucion del proyecto"
    echo "  help                        Muestra este mensaje"
    echo "  report                      Compilar y generar el pdf del informe a partir del archivo en latex"
    echo "  run                         Ejecutar el proyecto"
    echo "  rutas                       Muestra rutas importantes usadas en la ejecucion de este script"
    echo "  show_report [tool]          Mostrar el informe usando el comando dado en tool, si no usa uno por defecto"
    echo "  show_slides [tool]          Mostrar la presentacion usando el comando dado en tool,si no usa uno por defecto"
    echo "  slides                      Compilar y generar el pdf de la presentación a partir del archivo en latex"
    echo ""
}

#Determina si un programa existe, es necesario pasarle exactamente un parametro que es el nombre del programa
function Exists(){
    if ! command -v $1 &> /dev/null
    then
        #No existe
        return 1
    else
        #Existe
        return 0
    fi
}

#Comando run
function Run(){
    #echo Run
    if Exists dotnet; then
        #Variable para conocer el estado de ejecucion 
        #0 , OK
        #1 , Error
        status=0
                
        #pwd
        #Cambia al directorio raiz del moogle
        echo "Cambiando al directorio principal del moogle"
        cd $moogleDirectory
        echo "Usando dotnet"
        if dotnet build; then
            #Do nothing
            status=0
        else
            #The build fails
            status=1
        fi
        if dotnet run --project MoogleServer; then
            #do nothing
            status=0
        else
            #The run fails
            status=1
        fi
            #pwd
            #Cambia al directorio original desde donde se invoco el script
            echo "Regresando al directorio original"
            cd $originalDirectory
            #pwd

        if [[ $status -eq 1 ]]; then
            echo "Quizas instalando dotnet 7 se solucionen sus errores."
        fi
        else
            echo "Imposible ejecutar, dotnet no fue encontrado"
            exit
        fi
}
#Fin de definiciones de funciones

#VARIABLES

#Para obtener el directorio donde esta el script, de https://stackoverflow.com/questions/4774054/reliable-way-for-a-bash-script-to-get-the-full-path-to-itself
#Directorio donde esta el script
scriptDirectory="$( cd -- "$(dirname "$0")" >/dev/null 2>&1 ; pwd -P )"

#Directorio principal donde se encuentra el moogle, debe ser el directorio padre del directorio que contiene al script
moogleDirectory="$scriptDirectory/.."
#Ver, https://stackoverflow.com/questions/3915040/how-to-obtain-the-absolute-path-of-a-file-via-shell-bash-zsh-sh
#Convierte  el path relativo a absoluto
moogleDirectory=`realpath $moogleDirectory`

#Directorio desde donde se llamo al script originalmente
originalDirectory=$PWD

#FIN VARIABLES

#INICIO SCRIPT

# El codigo "$#" devuelve la cantidad de argumentos que se le pasaron al script
# El codigo "$1" devuelve el primer argumento
# El codigo "$2" devuelve el segundo argumento

case $1 in
    help)
        Help
    ;;
esac

#FIN SCRIPT