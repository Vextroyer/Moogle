namespace MoogleEngine;

/**
*Esta clase es un contenedor global para informacion relacionada con la coleccion de documentos.
**/

static class Coleccion{
    //Determina si la coleccion ha sido inicializada. Empieza siendo false.
    private static bool _inicializada = false;
    public static bool Inicializada{
        get{
            return _inicializada;
        }
        private set{
            _inicializada = value;
        }
    }

    //Son todos los documentos que constituyen la coleccion
    private static Documento[] _documentos = new Documento[0];
    //Cantidad de documentos en la coleccion
    public static int Count{
        get{
            return _documentos.Length;
        }
    }
    public static Documento[] Documentos{
        get{
            return Coleccion._documentos;
        }
    }
    private static Dictionary<string, List<int>> _terminosYApariciones = new Dictionary<string, List<int>>();//Relaciona el termino con los documentos en los que aparece

    //Inicializa la coleccion con los documentos dados.
    public static void Inicializar(){
        //Solo se puede inicializar una vez
        if(Coleccion.Inicializada)return;

        //Carga los documentos
        Coleccion._documentos = Cargador.Load();

        //Por cada documento
        for(int i=0;i<Coleccion.Count;++i){
            //Asocia este documento a dicho termino
            foreach(string termino in Coleccion.At(i).TerminosSinRepeticiones){
                if(!Coleccion._terminosYApariciones.ContainsKey(termino))Coleccion._terminosYApariciones.Add(termino,new List<int>());//Si es un nuevo termino, agregalo
                Coleccion._terminosYApariciones[termino].Add(i);//Asocia el termino con el documento donde aparece.
            }
        }

        Coleccion.Inicializada = true;
    }

    //Devuelve una copia de el documento en determinada posicion de la coleccion
    public static Documento At(int index){
        if(index < 0 || index >= Coleccion.Count)throw new IndexOutOfRangeException();

        return new Documento(Coleccion._documentos[index]);
    }
    //Cantidad de documentos diferentes de la coleccion donde aparece este termino
    public static int EnCuantosDocumentosAparece(string termino){
        if(string.IsNullOrEmpty(termino) || !Coleccion._terminosYApariciones.ContainsKey(termino))return 0;
        return Coleccion._terminosYApariciones[termino].Count;
    }
    //Determina si un termino dado es StopWord en este documento
    public static bool EsStopWord(string termino){
        //Considero como stopword algun los terminos que aparecen en mas del 75% de los documentos
        return 4*EnCuantosDocumentosAparece(termino) >= 3 * Coleccion.Count;
    }
}