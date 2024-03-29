#!/bin/bash

#Definiciones de funciones

#Imprime variables importantes para este script. Especificamente,la ruta desde donde se esta ejectuando el script,
#la ruta donde esta ubicado el script, la ruta del directorio raiz del proyecto moogle (esta contiene al directorio que contiene al
#script)
function Rutas(){
    echo "Rutas importantes"
    echo "Ruta de llamada               $originalDirectory"
    echo "Ruta al script                $scriptDirectory"
    echo "Ruta al moogle                $moogleDirectory"
    echo "Ruta a MoogleServer.csproj    $projectFile"
    echo "Ruta al Informe               $reportDirectory"
    echo "Informe                       $reportFile"
    echo "Ruta a la Presentacion        $slideDirectory"
    echo "Presentacion                  $slideFile"
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

#Comando report
function Report(){
    #Cambiar al directorio donde esta el informe, no encontre opcion para decirle a pdflatex donde buscar las imagenes
    echo "En: $PWD"
    echo "Cambiando al directorio donde se encuentra el informe"
    cd $reportDirectory
    echo "En: $PWD"
    echo ""

    if Exists pdflatex; then
        pdflatex -interaction=nonstopmode -output-directory=$reportDirectory -shell-escape $reportFile
        pdflatex -interaction=nonstopmode -output-directory=$reportDirectory -shell-escape $reportFile
    else
        echo "No se encontro pdflatex"
    fi

    echo ""
    echo "En: $PWD"
    echo "Cambiando al directorio original"
    cd $originalDirectory
    echo "En: $PWD"
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

        echo "Usando dotnet"
        #Compilacion y Ejecucion
        if dotnet run --project $projectFile; then
            #do nothing
            status=0
        else
            #The run fails
            status=1
        fi

        if [[ $status -eq 1 ]]; then
            echo "Este proyecto ha sido creado y probado en dotent 7. Quizas instalando dotnet 7 se solucionen sus errores."
        fi

    else
        echo "Imposible ejecutar, dotnet no fue encontrado"
        exit
    fi
}

#Comando auxiliar para abrir un archivo  o url utilizando la aplicacion por defecto del sistema, recibe como parametro la ruta del archivo
#Basado en: https://stackoverflow.com/questions/394230/how-to-detect-the-os-from-a-bash-script
#           https://www.enmimaquinafunciona.com/pregunta/25334/comando-equivalente-a-linux-para-el-comando-quotabrirquot-en-macwindows
function OpenDefault(){
    echo "Abriendo con aplicacion por defecto"
    case "$OSTYPE" in
        "darwin"*)
            open $1
        ;; 
        
        "linux-gnu"*)
            xdg-open $1
        ;;
    esac
}

#Comando show_report
function ShowReport(){
    #Si no existe compilalo
    if [[ ! -f $reportPdf ]]; then
        Report
    fi

    #Muestralo
    if [[ ! $1 = "" ]]; then
        #Mostrando con la aplicacion proporcionada
        echo "Mostrando pdf con $1"
        $1 $reportPdf
    else
        #Mostrando con la aplicacion por defecto
        OpenDefault $reportPdf
    fi
}

#Comando show_slides
function ShowSlides(){
    #Si no existe compilalo
    if [[ ! -f $slidePdf ]]; then
        Slides
    fi

    #Muestralo
    if [[ ! $1 = "" ]]; then
        #Mostrando con la aplicacion proporcionada
        echo "Mostrando pdf con $1"
        $1 $slidePdf
    else
        #Mostrando con la aplicacion por defecto
        OpenDefault $slidePdf
    fi
}

#Comando slides
function Slides(){
    #Cambiar al directorio donde esta la presentacion, no encontre opcion para decirle a pdflatex donde buscar las imagenes
    echo "En: $PWD"
    echo "Cambiando al directorio donde se encuentra la presentacion"
    cd $slideDirectory
    echo "En: $PWD"
    echo ""

    if Exists pdflatex; then
        pdflatex -interaction=nonstopmode -output-directory=$slideDirectory -shell-escape $slideFile
        pdflatex -interaction=nonstopmode -output-directory=$slideDirectory -shell-escape $slideFile
    else
        echo "No se encontro pdflatex"
    fi

    echo ""
    echo "En: $PWD"
    echo "Cambiando al directorio original"
    cd $originalDirectory
    echo "En: $PWD"
}

#Muestra informacion relevante para la ejecucion del script
function Troubleshoot(){
    echo "  Este script requiere de los siguientes comandos para funcionar: "
    echo "  cd            Para establecer las rutas que usaran otros comandos"
    echo "  command       Para determinar la existencia o no de los comandos"
    echo "  dirname       Para establecer las rutas que usaran otros comandos"
    echo "  dotnet        Para compilar y ejecutar el proyecto. Se recomienda la version 7.0"
    echo "  echo          Para escribir esto (^_^)"
    echo "  pdflatex      Para compilar y generar pdf a partir de archivos tex escritos en"
    echo "                latex"
    echo "  pwd           Para establecer las rutas que usaran otros comandos"
    echo "  realpath      Para establecer las rutas que usaran otros comandos"
    echo "  rm            Para remover archivos y directorios"
    echo "  xdg-open      Para abrir archivos con la aplicacion por defecto"
    echo ""
}

#Funcion clean
#Debe borrar lo siguiente
#Generado por la compilacion del moogle: -MoogleEngine/obj
#                                        -MoogleEngine/bin
#                                        -MoogleServer/obj
#                                        -MoogleServer/bin
#Todo archivo en el directorio Informe que no sea Informe.tex y el directorio Imagenes
#Todo archivo en el directorio Presentacion que no sea Presentacion.tex y el directorio Imagenes
#Ayuda de https://stackoverflow.com/questions/2437452/how-to-get-the-list-of-files-in-a-directory-in-a-shell-script
function Clean(){
    #Compilador C#
    echo "Borrando archivos generados por el compilador de C# ..."
    echo ""
    rm -r -f -d -v "$moogleDirectory/MoogleEngine/bin"
    rm -r -f -d -v "$moogleDirectory/MoogleEngine/obj"
    rm -r -f -d -v "$moogleDirectory/MoogleServer/bin"
    rm -r -f -d -v "$moogleDirectory/MoogleServer/obj"
    echo ""
    echo "Hecho."
    echo ""

    #Informe
    echo ""
    echo "Borrando archivos generados en la compilacion del informe"
    echo ""
    #Borra todos los archivos excepto el directorio Imagenes y el archivo Informe.tex
    for file in "$reportDirectory/"*
    do
        case $file in
            *"Informe.tex")
            ;&
            *"Imagenes")
                #Do nothing
            ;;
            *)
                rm -f -v $file
            ;;
        esac
    done
    echo ""
    echo "Hecho"
    echo ""

    #Presentacion
    echo ""
    echo "Borrando archivos generados en la compilacion de la presentacion"
    echo ""
    #Borra todos los archivos excepto el directorio Imagenes y el archivo Presentacion.tex
    for file in "$slideDirectory/"*
    do
        case $file in
            *"Presentacion.tex")
            ;&
            *"Imagenes")
                #Do nothing
            ;;
            *)
                rm -f -v $file
            ;;
        esac
    done
    echo ""
    echo "Hecho"
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

#Archivo que contiene el proyecto a compilar y ejecutar por dotnet
projectFile="$moogleDirectory/MoogleServer/MoogleServer.csproj"

#Directorio desde donde se llamo al script originalmente
originalDirectory=$PWD

#Directorio que contiene el informe, debe estar dentro del directorio principal del moogle
reportDirectory="$moogleDirectory/Informe"
#Archivo tex del informe
reportFile="$reportDirectory/Informe.tex"
#Pdf del informe
reportPdf="$reportDirectory/Informe.pdf"
#Directorio que contiene la presentacion, debe estar dentro del directorio principal del moogle
slideDirectory="$moogleDirectory/Presentacion"
#Archivo tex de la presentacion
slideFile="$slideDirectory/Presentacion.tex"
#Pdf de la presentacion
slidePdf="$slideDirectory/Presentacion.pdf"

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

    "clean")
        Clean
    ;;

    "hidden")
        echo "Este es un comando oculto que probablemente solo descubriran los que lean el codigo fuente del script ;)"
    ;;

    "report")
        Report
    ;;

    "run")
        Run
    ;;
    
    "rutas")
        Rutas
    ;;

    "show_report")
        #El segundo argumento de debe ser el programa usado para abrir el pdf
        ShowReport $2
    ;;

    "show_slides")
        #El segundo argumento de debe ser el programa usado para abrir el pdf
        ShowSlides $2
    ;;

    "slides")
        Slides
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