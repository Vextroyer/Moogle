namespace MoogleEngine;

/**
*Esta clase se encarga de ordenar un conjunto de SearchItems
**/

class Sorter{
    //Ordena los documentos descendentemente por valor de score utilizando el insertion sort
    public static SearchItem[] Sort(SearchItem[] items){
        //Crea una copia de la coleccion
        SearchItem[] newItems = new SearchItem[items.Length];
        for(int i=0;i<newItems.Length;++i)newItems[i] = new SearchItem(items[i]);
        
        //Ordena la coleccion
        for(int i=0;i<newItems.Length;++i){
            for(int j=i;j>0 && newItems[j].Score > newItems[j - 1].Score;--j){
                SearchItem c = new SearchItem(newItems[j]);
                newItems[j] = newItems[j - 1];
                newItems[j - 1] = c;
            }
        }

        return newItems;
    }
}