namespace MoogleEngine;


public static class Moogle
{
    private static Documento[] _documentos;//Contiene el corpus de documentos. De esta forma solo es necesario cargarlo una vez.
    public static SearchResult Query(string query) {
        //Carga los documentos
        if(_documentos == null)_documentos = Cargador.Load();

        //Procesar la consulta
        string[] terminos = Procesar(query);
        foreach(string s in terminos)System.Console.WriteLine(s);

        //Esta funcionalidad se puede encapsular
        //Determina el score de cada documento
        double[] score = Valorar(terminos,_documentos);

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
    #region Procesar
    private static string[] Procesar(string query){
        query = query.ToLower();
        string[] terminos = query.Split(" ",StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        //Vocales
        return terminos;
    }
    #endregion Procesar

    #region TF-IDF
    
    //Metodo que determina el valor de cada documento, relativo a la consulta
    //Esta basado en el TF-IDF
    public static double[] Valorar(string[] terms,Documento[] docs){
        double[] score = new double[docs.Length];

        //Halla la frecuencia inversa de cada termino
        double[] idf = new double[terms.Length];
        for(int i=0;i<idf.Length;++i){
            idf[i]=CalcularIdf(terms[i],docs);
        }
        
        //Calcula el score de cada documento
        for(int i=0;i<docs.Length;++i){
            //Es la combinacion del score individual de cada termino con respecto al documento
            for(int j=0;j<idf.Length;++j){
                score[i] += docs[i].FrecuenciaNormalizada(terms[j]) * idf[j];
            }
            
        }

        return score;
    }
    private static double CalcularIdf(string query,Documento[] docs){
        //idf(t,D) = log ( |D| / |{d E D: t E d}|)
        //El idf es el logaritmo en alguna base de la cantidad total de documentos entre la cantidad de documentos que contienen el termino

        //Calcula cuantos documentos tienen el termino query
        double cntDocumentosEnQueAparece =  1.0;//Evita la division por 0
        foreach(Documento d in docs)if(d.FrecuenciaBooleana(query))cntDocumentosEnQueAparece += 1.0;

        //Calcula el idf
        double idf = ((double)(docs.Length)) / cntDocumentosEnQueAparece;
        idf = Math.Log10(idf);

        return idf;
    }

    #endregion TF-IDF
}
