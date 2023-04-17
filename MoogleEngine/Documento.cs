namespace MoogleEngine;

/**
*Esta clase es la abstraccion de un documento.
*Lo represento como las diferentes palabras que lo componen y sus repeticiones
**/
public class Documento{

    #region Miembros
    private Dictionary<string,List<int>> _contenido;//Este diccionario representa el contenido del documento agrupado de la forma (termino, posiciones en las que aparece)
    private string[] _texto;//Este es el contenido textual del documento, palabra por palabra.

    #endregion Miembros

    #region Propiedades
    public string Titulo{
        get;
        private set;
    }
    //Cantidad de veces que se repite el termino que mas se repite
    public int MostFrequentCount{
        get;
        private set;
    }
    //Devuelve una copia del contenido
    public Dictionary<string,List<int>> Contenido{
        get{
            return new Dictionary<string, List<int>>(_contenido);
        }
    }
    //Devuelve una copia del texto
    public string[] Texto{
        get{
            return (string[]) this._texto.Clone();
        }
    }

    #endregion Propiedades

    #region Constructores
    public Documento(string[] terminos, string nombre){
        this.Titulo = nombre;

        this._texto = (string[]) terminos.Clone();

        this._contenido = new Dictionary<string, List<int>>();

        this.MostFrequentCount = 0;

        
        for(int i = 0;i < terminos.Length;++i){
            if(!this._contenido.ContainsKey(terminos[i])) this._contenido.Add(terminos[i],new List<int>());//Si no existe el termino en el diccionario, agregalo.
            
            this._contenido[terminos[i]].Add(i);//Guarda la posicion en que aparece
            
            MostFrequentCount = Math.Max(MostFrequentCount,this._contenido[terminos[i]].Count);
        }
    }
    //Crea este documento a partir de una copia de otro 
    public Documento(Documento other){
        this.Titulo = other.Titulo;
        this.MostFrequentCount = other.MostFrequentCount;
        this._contenido = other.Contenido;
        this._texto =(string[]) other.Texto;
    }

    #endregion Constructores

    #region Metodos
    //Devuelve la cantidad de apariciones de termino en el documento
    public int TermCount(string termino){
        if(string.IsNullOrEmpty(termino) || !this._contenido.ContainsKey(termino))return 0;
        else return this._contenido[termino].Count;
    }

    //Metodo para obtener el conjunto de las palabras que forman este documento. Este conjunto no contiene palabras repetidas
    public string[] GetUniqueTerms(){
        //Crea tantos strings como palabras diferentes
        string[] terminosDistintos = new string[this._contenido.Count];
        
        int i = 0;
        foreach(string s in this._contenido.Keys){
            terminosDistintos[i++] = s;
        }

        return terminosDistintos;
    }

    #endregion Metodos
}