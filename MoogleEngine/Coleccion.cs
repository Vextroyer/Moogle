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
    public static Documento[] Documentos{
        get{
            return (Documento[])_documentos.Clone();
        }
        private set{
            _documentos = value;
        }
    }
    //Cantidad de documentos en la coleccion
    public static int Count{
        get{
            return _documentos.Length;
        }
    }

    //Es el conjunto de terminos a partir de los cuales se crean los documentos de la coleccion
    private static string[] _terminos = new string[0];
    //Conjunto de terminos
    public static string[] Terminos{
        get{
            return (string[])_terminos.Clone();
        }
        private set{
            _terminos = value;
        }
    }
    //Cardinalidad del conjunto de terminos a partir del cual se crean los documentos de esta coleccion
    public static int UniqueTermsCount{
        get{
            return _terminos.Length;
        }
    }
    private static Dictionary<string, List<int>> _terminosYApariciones = new Dictionary<string, List<int>>();//Relaciona el termino con los documentos en los que aparece

    //Inicializa la coleccion con los documentos dados.
    public static void Inicializar(Documento[] documentos){
        //Solo se puede inicializar una vez
        if(Coleccion.Inicializada)return;

        Coleccion.Inicializada = true;
        
        Coleccion.Documentos = documentos;

        //Por cada documento
        for(int i=0;i<Coleccion.Count;++i){
            //Asocia este documento a dicho termino
            foreach(string termino in Coleccion.At(i).GetUniqueTerms()){
                if(!Coleccion._terminosYApariciones.ContainsKey(termino))Coleccion._terminosYApariciones.Add(termino,new List<int>());//Si es un nuevo termino, agregalo
                Coleccion._terminosYApariciones[termino].Add(i);//Asocia el termino con el documento donde aparece.
            }
        }
        
        Coleccion.Terminos = Coleccion._terminosYApariciones.Keys.ToArray();
        /*//Debugging
        System.Console.WriteLine("-----DEBUGGING------");
        System.Console.WriteLine($"La coleccion tiene {Coleccion.Count} documentos");
        System.Console.WriteLine($"La coleccion tiene {Coleccion.UniqueTermsCount} terminos unicos");
        StreamWriter newFile = new StreamWriter("../Documentacion/DistintasPalabras.txt");
        Coleccion._terminos = Coleccion._terminos.Order().ToArray();
        foreach(string s in Coleccion.Terminos)newFile.WriteLine(s);
        newFile.Close();
        System.Console.WriteLine("-----END-----DEBUGGING------");
        */
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
    //Documentos de la coleccion donde aparece este termino, arreglo vacio si no aparece
    public static int[] EnCualesDocumentosAparece(string termino){
        if(string.IsNullOrEmpty(termino) || !Coleccion._terminosYApariciones.ContainsKey(termino))return new int[0];
        return Coleccion._terminosYApariciones[termino].ToArray();
    }
}