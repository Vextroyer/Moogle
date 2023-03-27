namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query) {
        // Modifique este método para responder a la búsqueda

        //Carga los documentos
        Documento[] documentos = Cargador.Load();

        //Crea un resultado por cada documento
        SearchItem[] items = new SearchItem[documentos.Length];
        for(int i=0;i<items.Length;++i){
            float score = FrecuenciaNormalizada(query,documentos[i]);
            items[i] = new SearchItem(documentos[i].Titulo,documentos[i].Titulo + " " + score,score);
        }

        //Ordena los documentos basados en su score descendentemente
        Ordenar(items);

        //No muestres resultados irrelevantes en la busqueda
        items = Depurar(items);

        return new SearchResult(items, query);
    }

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

    //La frecuencia normalizada es una variacion de la frecuencia de termino que evita una predisposicion hacia documentos largos
    private static float FrecuenciaNormalizada(string query,Documento d){
        float frecuenciaBruta = d.FrecuenciaBruta(query);
        float mayorFrecuenciaBruta = d.MostFrequentCount;
        //mayorFrecuenciaBruta = 0 nunca sucedera, porque esto implica que existe un documento vacio y de suceder esto ya la clase Cargador se hubiese encargado de ignorarlo, o que no existen documentos en la coleccion, y la clase Cargador se encargara de lanzar una excepcion en dicho caso.
        return frecuenciaBruta / mayorFrecuenciaBruta;
    }
}
