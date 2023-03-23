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
            items[i] = new SearchItem(documentos[i].titulo,documentos[i].titulo,0.3f);
        }

        return new SearchResult(items, query);
    }
}
