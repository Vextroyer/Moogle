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

        SearchItem[] items = new SearchItem[documentos.Length];
        for(int i=0;i<items.Length;++i){
            items[i] = new SearchItem(documentos[i].titulo,documentos[i].titulo,documentos[i].FrecuenciaBruta(query));
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
        /**
        Tomemos un arreglo de tamano 1, ya esta ordenado.
        Supongamos que tenemos un arreglo de tamano n que ya esta ordenado descendentemente, es decir
            ai >= aj para todo i < j
        Demostremos que el algoritmo es capaz de ordenar el arreglo para n + 1:
        Tomemos an+1, si an >= an+1 ya esta ordenado.
        Si no, cambiamos an y an+1.
        Si an-1 >= an, ya esta ordenado.
        Si no cambiamos an-1 y an.
        Y proseguimos asi en un proceso finito porque n es finito y disminuye en 1 en cada paso.
        Digamos que este proceso termino en un indice j.
        **/

        return new SearchResult(items, query);
    }
}
