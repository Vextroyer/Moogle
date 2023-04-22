namespace MoogleEngine;
/**
*Esta clase se encarga de computar un valor de ranking para cada documento de la coleccion.
*Utiliza el modelo TF-IDF
**/
static class Valorador{
    //Dado una consulta como vector y una coleccion de documentos como vectores computa un valor de similaridad
    //de la consulta respecto a cada vector
    public static double[] Valorar(Vector query,Vector[] documentos){
        double[] score = new double[documentos.Length];

        //Calcula el score de cada documento
        for(int i=0;i<documentos.Length;++i)score[i] = Similaridad(query, documentos[i]);

        return score;
    }

    /**
    *Metodos relativos al modelo vectorial con TF-IDF
    **/
    //Calcula la similaridad entre dos vectores
    public static double Similaridad(Vector u,Vector v){
        return SimilaridadCoseno(u,v);
    }
    //Calucla la similaridad por la formula del coseno de dos vectores
    public static double SimilaridadCoseno(Vector u,Vector v){
        double num = u * v;
        double den = (u * u) * (v * v);
        return num / Math.Sqrt(den);
    }
    public static double Idf(string termino){
        //idf(t,D) = log ( |D| / (1 + |Dt|))
        //D es la cantidad total de documentos, Dt es la cantidad de documentos de D donde aparece t
        double D = Coleccion.Count;
        double Dt = 1 + Coleccion.EnCuantosDocumentosAparece(termino);
        return Math.Log10(D / Dt);
    }
    //Decide un modo de calcular el Tf(t,d)
    public static double Tf(string termino, Documento documento){
        return  FrecuenciaNormalizada(termino,documento);
    }

    //Devueve la cantidad de veces que aparece un termino en un documento
    public static int FrecuenciaBruta(string termino,Documento documento){
        return  documento.TermCount(termino);
    }

    //Calcula la frecuencia normalizada del termino en el documento
    public static double FrecuenciaNormalizada(string termino,Documento documento){
        double frecuenciaBruta = FrecuenciaBruta(termino,documento);
        double mayorFrecuenciaBruta = documento.MostFrequentCount;
        //mayorFrecuenciaBruta = 0 nunca sucedera, porque esto implica que existe un documento vacio y de suceder esto ya la clase Cargador se hubiese encargado de ignorarlo, o que no existen documentos en la coleccion, y la clase Cargador se encargara de lanzar una excepcion en dicho caso.
        return frecuenciaBruta / mayorFrecuenciaBruta;
    }
}