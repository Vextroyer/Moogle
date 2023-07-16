#!/bin/bash

#Puede necesitar mas de una compilacion
pdflatex Informe.tex

#Copia el pdf generado por el comando anterior al directorio raiz del proyecto moogle!
#Asume que el comando se ejecuta en la ruta (alguna ruta ...)/moogle!/Latex
path="$PWD/.."
cp Informe.pdf "$path/Informe.pdf" 