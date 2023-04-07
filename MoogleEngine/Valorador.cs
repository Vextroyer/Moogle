namespace MoogleEngine;
/**
*Esta clase se encarga de computar un valor de ranking para cada documento de la coleccion.
*Utiliza el modelo TF-IDF
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
                score[i] += FrecuenciaNormalizada(terms[j],docs[i]) * idf[j];
            }
            
        }

        return score;
    }

    /**
    *Metodos relativos al modelo TF-IDF
    **/

    public static double CalcularIdf(string query,Documento[] docs){
        //idf(t,D) = log ( |D| / |{d E D: t E d}|)
        //El idf es el logaritmo en alguna base de la cantidad total de documentos entre la cantidad de documentos que contienen el termino

        //Calcula cuantos documentos tienen el termino query
        double cntDocumentosEnQueAparece =  1.0;//Evita la division por 0
        foreach(Documento d in docs)if(FrecuenciaBooleana(query,d))cntDocumentosEnQueAparece += 1.0;

        //Calcula el idf
        double idf = ((double)(docs.Length)) / cntDocumentosEnQueAparece;
        idf = Math.Log10(idf);

        return idf;
    }

    //Determina si un termino aparece o no en el documento
    public static bool FrecuenciaBooleana(string query,Documento doc){
        return doc.TermCount(query) != 0;
    }

    //Devueve la cantidad de veces que aparece un termino en un documento
    public static int FrecuenciaBruta(string query,Documento doc){
        return  doc.TermCount(query);
    }

    //Calcula la frecuencia normalizada del termino en el documento
    public static double FrecuenciaNormalizada(string query,Documento doc){
        double frecuenciaBruta = FrecuenciaBruta(query,doc);
        double mayorFrecuenciaBruta = doc.MostFrequentCount;
        //mayorFrecuenciaBruta = 0 nunca sucedera, porque esto implica que existe un documento vacio y de suceder esto ya la clase Cargador se hubiese encargado de ignorarlo, o que no existen documentos en la coleccion, y la clase Cargador se encargara de lanzar una excepcion en dicho caso.
        return frecuenciaBruta / mayorFrecuenciaBruta;
    }
}