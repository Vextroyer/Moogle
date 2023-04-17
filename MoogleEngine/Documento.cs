namespace MoogleEngine;

/**
*Esta clase es la abstraccion de un documento.
**/
public class Documento{

    #region Miembros
    private Dictionary<string,List<int>> _contenido;//Este diccionario representa el contenido del documento agrupado de la forma (termino, posiciones en las que aparece)
    private string[] _terminos;//Estas son los terminos que componen el documento, un termino es una palabra o un numero.

    #endregion Miembros

    #region Propiedades
    //El titulo del Documento
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
    //Devuelve una copia de los terminos
    public string[] Terminos{
        get{
            return (string[]) this._terminos.Clone();
        }
    }
    //Devuelve verdadero si el documento esta vacio. El documento esta vacio si no tiene terminos.
    public bool IsEmpty{
        get{
            return !(this._terminos.Length > 0);
        }
    }

    #endregion Propiedades

    #region Constructores
    //Crea un documento a partir de un texto
    public Documento(string texto,string nombre){
        this.Titulo = nombre;

        //Los terminos que componen el texto
        this._terminos = Tokenizer.ProcesarTexto(texto).Item1;

        this._contenido = new Dictionary<string, List<int>>();

        this.MostFrequentCount = 0;

        for(int i = 0;i < this._terminos.Length;++i){
            if(!this._contenido.ContainsKey(this._terminos[i])) this._contenido.Add(this._terminos[i],new List<int>());//Si no existe el termino en el diccionario, agregalo.
            
            this._contenido[this._terminos[i]].Add(i);//Guarda la posicion en que aparece
            
            MostFrequentCount = Math.Max(MostFrequentCount,TermCount(this._terminos[i]));
        }
    }
    //Crea este documento a partir de una copia de otro 
    public Documento(Documento other){
        this.Titulo = other.Titulo;
        this.MostFrequentCount = other.MostFrequentCount;
        this._contenido = other.Contenido;
        this._terminos = other.Terminos;
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