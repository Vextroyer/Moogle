namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query) {
        //Carga los documentos
        Coleccion.Inicializar();

        //Procesar la consulta
        query = Tokenizer.ProcesarQuery(query).Item1;

        //Esta funcionalidad se puede encapsular
        //Determina el score de cada documento
        double[] score = Valorador.Valorar(new Vector(new Documento(query,"Iam your query")),Coleccion.Vectores);

        //Crea un resultado por cada documento
        SearchItem[] items = new SearchItem[Coleccion.Count];
        for(int i=0;i<items.Length;++i){
            items[i] = new SearchItem(Coleccion.At(i).Titulo,Snippet.GetSnippet(query,Coleccion.At(i)),score[i]);
        }

        //Ordena los documentos basados en su score descendentemente
        items = Sorter.Sort(items);

        //No muestres resultados irrelevantes en la busqueda
        items = Depurador.Depurar(items);
        //Hasta aqui

        return new SearchResult(items, query);
    }
}
