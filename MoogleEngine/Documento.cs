namespace MoogleEngine;

/**
*Esta clase es la abstraccion de un documento.
*Lo represento como las diferentes palabras que lo componen y sus repeticiones
**/
public class Documento{

    #region Miembros
    private const int SnippetLength = 20;//Cantidad maxima de terminos mostrados en el snippet
    private Dictionary<string,List<int>> _contenido;//Este diccionario representa el contenido del documento agrupado de la forma (termino, posiciones en las que aparece)
    private string[] _texto;//Este es el contenido textual del documento, palabra por palabra.

    #endregion Miembros

    #region Propiedades
    public string Titulo{
        get;
        private set;
    }

    //De todos los terminos del documento guarda la frecuencia bruta del de mayor frecuencia bruta, auxiliar para calcular la frecuencia normalizada 
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
        this._texto =(string[]) other._texto.Clone();
    }

    #endregion Constructores

    #region Metodos
    //Determina si un termino aparece o no en el documento
    public bool FrecuenciaBooleana(string query){
        if(string.IsNullOrEmpty(query))return false;
        return this._contenido.ContainsKey(query);//Este metodo no arrojara una excepcion pues esto solo sucede si el string es nulo, y en tal caso se hubiese ejecutado la linea de codigo anterior
    }

    //Devueve la cantidad de veces que aparece un termino en un documento
    public int FrecuenciaBruta(string termino){
        if(this._contenido.ContainsKey(termino))return this._contenido[termino].Count;
        else return 0;
    }

    //Calcula la frecuencia normalizada del termino en el documento
    public double FrecuenciaNormalizada(string query){
        double frecuenciaBruta = this.FrecuenciaBruta(query);
        double mayorFrecuenciaBruta = this.MostFrequentCount;
        //mayorFrecuenciaBruta = 0 nunca sucedera, porque esto implica que existe un documento vacio y de suceder esto ya la clase Cargador se hubiese encargado de ignorarlo, o que no existen documentos en la coleccion, y la clase Cargador se encargara de lanzar una excepcion en dicho caso.
        return frecuenciaBruta / mayorFrecuenciaBruta;
    }

    //Metodo para generar un snippet a partir de una consulta
    public string GetSnippet(string[] terminosQuery){
        string snippet = "";
        
        //Creo un Documento a partir de mi query
        Documento query = new Documento(terminosQuery,"I am your query");
        
        //Determino los distintos terminos de mi query
        string[] terminosDistintos = query.GetUniqueTerms();
        
        //Corpus de subDocumentos formados por los terminos de query
        List<Documento> subDocumentos = new List<Documento>();

        //Por cada aparicion de algun termino de la query en el documento
        foreach(string term in terminosDistintos){
            if(!this.FrecuenciaBooleana(term))continue;
            foreach(int pos in this._contenido[term]){

                //Construyo un minidocumento con esta seccion del documento
                List<string> miniDocumentoTerms = new List<string>();
                for(int i = Math.Max(0,pos - SnippetLength / 2), snippetWords = 0;i < this._texto.Length && snippetWords < SnippetLength;++i,++snippetWords){
                    miniDocumentoTerms.Add(this._texto[i]);
                }
                subDocumentos.Add(new Documento(miniDocumentoTerms.ToArray(),"I am your subDocument"));
            }
        }

        //Si no aparece ningun termino en este documento
        if(subDocumentos.Count == 0)return snippet;

        double[] valor = Valorador.Valorar(terminosDistintos,subDocumentos.ToArray());
        
        //Mi snippet es el subdocumento de mejor score
        int posicionMejor = 0;
        double valorMejor = double.MinValue;
        for(int i=0;i<valor.Length;++i){
            if(valorMejor < valor[i]){
                valorMejor = valor[i];
                posicionMejor = i;
            }
        }

        foreach(string s in subDocumentos[posicionMejor]._texto)snippet += s + " ";

        return snippet;
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