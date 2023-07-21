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
    echo "  troubleshoot                Muestra informacion que puede servir para prevenir y corregir errores en la"
    echo "                              ejecucion del script"
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

    local status=0

    if Exists dotnet; then
        #Variable para conocer el estado de ejecucion 
        #0 , OK
        #1 , Error
        status=0
                
        #Cambia al directorio raiz del moogle
        echo "En: $PWD"
        echo "Cambiando al directorio del moogle"
        cd $moogleDirectory
        echo "En: $PWD"
        echo ""


        echo "Usando dotnet"
        #Compilacion y Ejecucion
        if dotnet run --project MoogleServer; then
            #do nothing
            status=0
        else
            #The run fails
            status=1
        fi


        #Cambia al directorio original desde donde se invoco el script
        echo ""
        echo "En: $PWD"
        echo "Regresando al directorio original"
        cd $originalDirectory
        echo "En: $PWD"
        echo ""

        if [[ $status -eq 1 ]]; then
            echo "Este proyecto ha sido creado y probado en dotent 7. Quizas instalando dotnet 7 se solucionen sus errores."
        fi
    else
        echo "Imposible ejecutar, dotnet no fue encontrado"
        exit
    fi
}

#Muestra informacion relevante para la ejecucion del script
function Troubleshoot(){
    echo "  Este script requiere de los siguientes comandos para funcionar: "
    echo "  cd            Para establecer las rutas que usaran otros comandos"
    echo "  command       Para determinar la existencia o no de los comandos"
    echo "  dirname       Para establecer las rutas que usaran otros comandos"
    echo "  dotnet        Para compilar y ejecutar el proyecto. Se recomienda la version 7.0"
    echo "  echo          Para escribir esto (^_^)"
    echo "  pwd           Para establecer las rutas que usaran otros comandos"
    echo "  realpath      Para establecer las rutas que usaran otros comandos"
    echo ""
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
    "")
        #Continua ejecutando el proximo caso
    ;&
    "help")
        Help
    ;;

    "hidden")
        echo "Este es un comando oculto que probablemente solo descubriran los que lean el codigo fuente del script ;)"
    ;;

    "run")
        Run
    ;;
    
    "rutas")
        Rutas
    ;;

    "troubleshoot")
        Troubleshoot
    ;;

    *)
        #Comando desconocido
        echo "Comando desconocido. Mostrando la ayuda. "
        echo ""
        Help
    ;;
esac

#FIN SCRIPT