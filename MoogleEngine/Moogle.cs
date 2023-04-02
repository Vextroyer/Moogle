namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query) {
        //Procesar la consulta
        string[] terminos = Procesar(query);
        foreach(string s in terminos)System.Console.WriteLine(s);

        //Carga los documentos
        Documento[] documentos = Cargador.Load();

        //Determina el score de cada documento
        double[] score = Valorar(terminos,documentos);

        //Crea un resultado por cada documento
        SearchItem[] items = new SearchItem[documentos.Length];
        for(int i=0;i<items.Length;++i){
            items[i] = new SearchItem(documentos[i].Titulo,documentos[i].Titulo + " " + score[i],score[i]);
        }

        //Ordena los documentos basados en su score descendentemente
        items = Sorter.Sort(items);

        //No muestres resultados irrelevantes en la busqueda
        items = Depurador.Depurar(items);

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
        double cntDocumentosUtiles =  1.0;//Evita la division por 0
        foreach(Documento d in docs)if(d.FrecuenciaBooleana(query))cntDocumentosUtiles += 1.0;

        //Calcula el idf
        double idf = ((double)(docs.Length)) / cntDocumentosUtiles;
        idf = Math.Log10(idf);

        return idf;
    }

    #endregion TF-IDF
}
