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
            items[i] = new SearchItem(documentos[i].Titulo,documentos[i].Titulo,FrecuenciaNormalizada(query,documentos[i]));
        }

        //Ordenar
        Ordenar(items);

        return new SearchResult(items, query);
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
