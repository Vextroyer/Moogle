#!/bin/bash

echo "uso: proyecto.sh <opcion>"
echo "Las opciones disponibles son:"
echo "  run                         Ejecutar el proyecto"
echo "  report                      Compilar y generar el pdf del proyecto latex relativo al informe"
echo "  slides                      Compilar y generar el pdf del proyecto latex relativo a la presentación"
echo "  show_report [tool]          Mostrar el informe usando el comando dado en tool, si no usa uno por defecto"
echo "  show_slides [tool]          Mostrar la presentacion usando el comando dado en tool,si no usa uno por defecto"
echo "  clean                       Elimina ficheros generados en la compilacion de la presentacion, el informe, o  "
echo "                              en la ejecucion del proyecto"