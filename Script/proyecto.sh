#!/bin/bash

#Definiciones de funciones
function Help(){
    echo "uso: proyecto.sh <opcion>"
    echo "Las opciones disponibles son:"
    echo "  run                         Ejecutar el proyecto"
    echo "  report                      Compilar y generar el pdf del informe a partir del archivo en latex"
    echo "  slides                      Compilar y generar el pdf de la presentación a partir del archivo en latex"
    echo "  show_report [tool]          Mostrar el informe usando el comando dado en tool, si no usa uno por defecto"
    echo "  show_slides [tool]          Mostrar la presentacion usando el comando dado en tool,si no usa uno por defecto"
    echo "  clean                       Elimina ficheros generados en la compilacion de la presentacion, el informe, y  "
    echo "                              en la ejecucion del proyecto"
    echo "  help                        Muestra este mensaje"
    echo ""
    echo "Los parametros <arg> son necesarios, los parametros [arg] son opcionales."
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

#Para obtener el directorio donde esta el script, de https://stackoverflow.com/questions/4774054/reliable-way-for-a-bash-script-to-get-the-full-path-to-itself
#Directorio donde esta el script
scriptDirectory="$( cd -- "$(dirname "$0")" >/dev/null 2>&1 ; pwd -P )"

#Directorio principal donde se encuentra el moogle, debe ser el directorio que contiene a SCRIPTPATH
moogleDirectory="$scriptDirectory/.."
#Ver, https://stackoverflow.com/questions/3915040/how-to-obtain-the-absolute-path-of-a-file-via-shell-bash-zsh-sh
moogleDirectory=`realpath $moogleDirectory`

#Directorio desde donde se llamo al script originalmente
originalDirectory=$PWD

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
        ;;
        show_report)
            echo Report
        ;;
        show_slides)
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