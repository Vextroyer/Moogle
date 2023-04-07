namespace MoogleEngine;


public static class Moogle
{
    private static Documento[] _documentos;//Contiene el corpus de documentos. De esta forma solo es necesario cargarlo una vez.
    public static SearchResult Query(string query) {
        //Carga los documentos
        if(_documentos == null)_documentos = Cargador.Load();

        //Procesar la consulta
        string[] terminos = Tokenizer.Procesar(query);
        foreach(string s in terminos)System.Console.WriteLine(s);

        //Esta funcionalidad se puede encapsular
        //Determina el score de cada documento
        double[] score = Valorador.Valorar(terminos,_documentos);

        //Crea un resultado por cada documento
        SearchItem[] items = new SearchItem[_documentos.Length];
        for(int i=0;i<items.Length;++i){
            items[i] = new SearchItem(_documentos[i].Titulo,_documentos[i].GetSnippet(terminos),score[i]);
        }

        //Ordena los documentos basados en su score descendentemente
        items = Sorter.Sort(items);

        //No muestres resultados irrelevantes en la busqueda
        items = Depurador.Depurar(items);
        //Hasta aqui

        return new SearchResult(items, query);
    }
}
