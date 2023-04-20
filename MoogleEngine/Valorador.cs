namespace MoogleEngine;
/**
*Esta clase se encarga de computar un valor de ranking para cada documento de la coleccion.
*Utiliza el modelo TF-IDF
**/
static class Valorador{
    //Dado una coleccion de documentos y un listado de terminos, computa un valor de relevancia para cada documento con resprecto a los terminos
    public static double[] Valorar(string[] terms,Documento[] docs){
        double[] score = new double[docs.Length];

        //Calcula el score de cada documento
        for(int i=0;i<docs.Length;++i){
            //Es la combinacion del score individual de cada termino con respecto al documento
            for(int j=0;j<terms.Length;++j){
                score[i] += FrecuenciaNormalizada(terms[j],docs[i]) * Idf(terms[j]);
            }
            
        }

        return score;
    }

    /**
    *Metodos relativos al modelo TF-IDF
    **/

    public static double Idf(string query){
        //idf(t,D) = log ( |D| / (1 + |Dt|))
        //D es la cantidad total de documentos, Dt es la cantidad de documentos de D donde aparece t
        double D = Coleccion.Count;
        double Dt = Coleccion.EnCuantosDocumentosAparece(query);
        return Math.Log2(D / (1.0 + Dt));
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