namespace MoogleEngine;
/**
*Esta clase se encarga de computar un valor de ranking para cada documento de la coleccion
**/
static class Valorador{
    //Dado una coleccion de documentos y un listado de terminos, computa un valor de relevancia para cada documento con resprecto a los terminos
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
}