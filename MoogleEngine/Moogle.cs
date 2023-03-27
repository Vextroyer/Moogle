namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query) {
        // Modifique este método para responder a la búsqueda

        /*
        SearchItem[] items = new SearchItem[3] {
            new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.9f),
            new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.5f),
            new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.1f),
        };*/

        Documento[] documentos = Cargador.Load();

        //Crea un resultado por cada documento
        SearchItem[] items = new SearchItem[documentos.Length];
        for(int i=0;i<items.Length;++i){
            items[i] = new SearchItem(documentos[i].titulo,documentos[i].titulo + " " + documentos[i].FrecuenciaBruta(query),FrecuenciaNormalizada(query,documentos[i]));
        }

        //Ordenamiento respecto al score
        //Insertion Sort
        for(int i=0;i<items.Length;++i){
            for(int j=i;j>0 && items[j].Score > items[j - 1].Score;--j){
                SearchItem c = new SearchItem(items[j]);
                items[j] = items[j - 1];
                items[j - 1] = c;
            }
        }

        return new SearchResult(items, query);
    }

    //La frecuencia normalizada es una variacion de la frecuencia de termino que evita una predisposicion hacia documentos largos
    public static float FrecuenciaNormalizada(string query,Documento d){
        float frecuenciaBruta = d.FrecuenciaBruta(query);
        float mayorFrecuenciaBruta = d.MostFrequentCount;
        return frecuenciaBruta / mayorFrecuenciaBruta;
    }
}
