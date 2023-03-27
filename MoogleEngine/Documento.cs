namespace MoogleEngine;

/**
*Esta clase es la abstraccion de un documento.
*Lo represento como las diferentes palabras que lo componen y sus repeticiones
**/
public class Documento{
    public string titulo{
        get;
        private set;
    }
    
    //Cantidad total de terminos que aparecen en el documento
    public int cntTerminos{
        get;
        private set;
    }

    //Este diccionario representa el contenido del documento agrupado de la forma (termino, cantidad de repeticiones)
    private Dictionary<string,int> _contenido;

    public Documento(string[] terminos, string nombre){
        this.titulo = nombre;

        this._contenido = new Dictionary<string, int>();

        foreach(string t in terminos){
            this.cntTerminos ++;
            if(!this._contenido.ContainsKey(t))
                this._contenido.Add(t,1);
            else
                ++this._contenido[t];
        }
    }

    //Devueve la cantidad de veces que aparece un termino en un documento
    public int FrecuenciaBruta(string termino){
        if(this._contenido.ContainsKey(termino))return this._contenido[termino];
        else return 0;
    }
}