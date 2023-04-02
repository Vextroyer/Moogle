namespace MoogleEngine;

/**
*Esta clase se encarga de decidir dado un conjunto de SearchItem, cuales son relevantes para mostrar y cuales pueden ser desechados.
**/

class Depurador{
    //Elimina resultados irrelevantes al usuario, que tienen poca o ninguna relacion con su busqueda
    public static SearchItem[] Depurar(SearchItem[] items){
        //Determina la cantidad de elementos irrelevantes
        int cntIrrelevantes = 0;
        for(int i =0;i<items.Length;++i){
            if(EsIrrelevante(items[i].Score))++cntIrrelevantes;
        }

        SearchItem[] auxItems = new SearchItem[items.Length - cntIrrelevantes];
        for(int i=0,j =0;i<items.Length && j < auxItems.Length;++i){
            if(EsIrrelevante(items[i].Score))continue;
            auxItems[j] = items[i];
            ++j;
        }

        return auxItems;
    }
    //Metodo auxiliar para determinar si un resultado es relevante basado en su score
    private static bool EsIrrelevante(double score){
        return score == 0;
    }
}