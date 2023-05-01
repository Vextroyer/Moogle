namespace MoogleEngine;
/**
*Esta clase se encarga de computar un valor de ranking para cada documento de la coleccion.
*Utiliza el modelo TF-IDF
**/
static class Valorador{
    //Dado una consulta como documento y una coleccion de documentos computa un valor de similaridad
    //de la consulta respecto a cada documento. De existir aplica reglas definidas por el usuario.
    public static double[] Valorar(Documento query,Documento[] documentos,Regla regla){
        Vector vectorQuery = new Vector(query);//Vector de la consulta
        Vector[] vectorDocumento = new Vector[documentos.Length];//Vector de los documentos 
        for(int i=0;i<vectorDocumento.Length;++i)vectorDocumento[i] = new Vector(documentos[i]);//Inicializa los vectores de los documentos

        double[] score = new double[documentos.Length];

        //Aplica el operador *(should), aumenta la importancia relativa de dichos terminos.
        foreach((string,int) v in regla.Should)vectorQuery.AplicarOperador(v.Item1,Regla.CalcularShould(v.Item2));

        //Calcula el score de cada documento
        for(int i=0;i<documentos.Length;++i){

            #region Operadores
            //El documento no es valido cuando no cumple con las reglas establecidas por el usuario:
            //-Existe algun termino de la regla !(not) que aparece en el, pues ninguno debe aparecer.
            //-Existe algun termino de la regla ^(must) que no aparece en el, pues todos deben aparecer.
            bool esDocumentoValido = true;
            //Si alguno de !(not) esta
            foreach(string s in regla.Not){
                if(documentos[i].Contiene(s)){
                    esDocumentoValido = false;
                    break;
                }
            }
            //Si algunos de ^(must) no esta
            foreach(string s in regla.Must){
                if(documentos[i].NoContiene(s)){
                    esDocumentoValido = false;
                    break;
                }
            }
            if(!esDocumentoValido)continue;

            //Aplica el operador ~(close)
            foreach((string,string) v in regla.Close){
                string a = v.Item1;
                string b = v.Item2;
                //Determino la menor distancia de los terminos en el documento
                int cercania = documentos[i].Cercania(a,b);
                if(cercania == -1)continue;//Alguno no aparece
                //Aumenta el valor relativo a ambos terminos en dependencia de su cercania.
                vectorDocumento[i].AplicarOperador(a,Regla.CalcularClose(cercania));
                vectorDocumento[i].AplicarOperador(b,Regla.CalcularClose(cercania));
            }

            #endregion Operadores

            score[i] = Similaridad(vectorQuery,vectorDocumento[i]);
        }

        return score;
    }

    /**
    *Metodos relativos al modelo vectorial con TF-IDF
    **/
    //Calcula la similaridad entre dos vectores
    public static double Similaridad(Vector u,Vector v){
        return SimilaridadCoseno(u,v);
    }
    //Calcula la similaridad por la formula del coseno de dos vectores
    public static double SimilaridadCoseno(Vector u,Vector v){
        double num = u * v;
        double den = (u * u) * (v * v);
        return num / Math.Sqrt(den);
    }
    //Calcula un peso tfidf para un termino en un documento
    public static double Pesar(string termino,Documento documento){
        return Tf(termino,documento)*Valorador.Idf(termino);
    }
    public static double Idf(string termino){
        //idf(t,D) = log ( |D| / (1 + |Dt|))
        //D es la cantidad total de documentos, Dt es la cantidad de documentos de D donde aparece t
        double D = Coleccion.Count;
        double Dt = Coleccion.EnCuantosDocumentosAparece(termino);
        return Math.Log10(D / ( 1 + Dt ) );
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