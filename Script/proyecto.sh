#!/bin/bash

#Definiciones de funciones
function Help(){
    echo "uso: proyecto.sh <opcion>"
    echo "Las opciones disponibles son:"
    echo "  run                         Ejecutar el proyecto"
    echo "  report                      Compilar y generar el pdf del proyecto latex relativo al informe"
    echo "  slides                      Compilar y generar el pdf del proyecto latex relativo a la presentación"
    echo "  show_report [tool]          Mostrar el informe usando el comando dado en tool, si no usa uno por defecto"
    echo "  show_slides [tool]          Mostrar la presentacion usando el comando dado en tool,si no usa uno por defecto"
    echo "  clean                       Elimina ficheros generados en la compilacion de la presentacion, el informe, o  "
    echo "                              en la ejecucion del proyecto"
    echo "  help                        Muestra este mensaje"
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
#Fin de definiciones de funciones

# El codigo "$#" devuelve la cantidad de argumentos que se le pasaron al script
# El codigo "$1" devuelve el primer argumento
# El codigo "$2" devuelve el segundo argumento

# cantidadDeArgumentos < 1 || cantidadDeArgumentos > 2
if [[ ($# -lt 1) || ($# -gt 2) ]]
then
    Help;
    #Termina la ejecucion del script, codigo argumento invalido
    exit 128
fi

#A partir de esta linea el programa tiene 1 o 2 argumentos
if [[ $# -eq 1 ]]
then
    #echo "Tiene 1";
    case $1 in
        run)
            echo Run
        ;;
        report)
            echo Report
        ;;
        slides)
            echo Slides
        ;;
        clean)
            echo Clean
        ;;
        help)
            Help
        ;;
        *)
            Help
            exit 128
        ;;
    esac
else
    #echo "Tiene 2";
    case $1 in
        show_report)
            echo Show_report
        ;;
        show_slides)
            echo Show_slides
        ;;
        *)
            Help
            exit 128
        ;;
    esac
fi