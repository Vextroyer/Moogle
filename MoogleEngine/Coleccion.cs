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
            return _documentos;
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

    //Inicializa la coleccion con los documentos dados.
    public static void Inicializar(Documento[] documentos){
        //Solo se puede inicializar una vez
        if(Coleccion.Inicializada)return;

        Coleccion.Inicializada = true;
        
        Coleccion.Documentos = documentos;
    }

    //Devuelve el documento en determinada posicion de la coleccion
    public static Documento At(int index){
        if(index < 0 || index >= Coleccion.Count)throw new IndexOutOfRangeException();

        return _documentos[index];
    }
}