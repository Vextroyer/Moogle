namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query) {

        //Carga los documentos
        Documento[] documentos = Cargador.Load();

        //Crea un resultado por cada documento
        SearchItem[] items = new SearchItem[documentos.Length];
        for(int i=0;i<items.Length;++i){
            items[i] = new SearchItem(documentos[i].Titulo,documentos[i].Titulo,Valorar(query,documentos[i]));
        }

        //Ordena los documentos basados en su score descendentemente
        Ordenar(items);

        //No muestres resultados irrelevantes en la busqueda
        items = Depurar(items);

        return new SearchResult(items, query);
    }

    //Metodo que determina el valor de un documento, relativo a la consulta
    //Esta basado en el TF-IDF
    public static float Valorar(string query,Documento d){
        return d.FrecuenciaNormalizada(query);
    }

    #region Orenamiento
    //Ordena los documentos descendentemente por valor de score utilizando el insertion sort
    private static void Ordenar(SearchItem[] items){
        for(int i=0;i<items.Length;++i){
            for(int j=i;j>0 && items[j].Score > items[j - 1].Score;--j){
                SearchItem c = new SearchItem(items[j]);
                items[j] = items[j - 1];
                items[j - 1] = c;
            }
        }
    }
    #endregion Ordenamiento

    #region Depuracion
    //Elimina resultados irrelevantes al usuario, que tienen poca o ninguna relacion con su busqueda
    private static SearchItem[] Depurar(SearchItem[] items){
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
    private static bool EsIrrelevante(float score){
        return score == 0;
    }
    #endregion Depuracion

}
