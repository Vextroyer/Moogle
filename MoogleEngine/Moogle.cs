namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query) {
        //Carga los documentos
        Coleccion.Inicializar();

        //Procesar la consulta
        (string,Regla) auxiliar = Tokenizer.ProcesarQuery(query);
        query = auxiliar.Item1;
        // query = Tokenizer.ProcesarQuery(query).Item1;

        //Esta funcionalidad se puede encapsular
        //Determina el score de cada documento
        double[] score = Valorador.Valorar(new Documento(query,"Iam your query"),Coleccion.Documentos,auxiliar.Item2);

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
