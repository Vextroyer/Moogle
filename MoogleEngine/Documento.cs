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

    #endregion Propiedades

    #region Constructores
    public Documento(string[] terminos, string nombre){
        this.Titulo = nombre;

        this._texto = (string[]) terminos.Clone();

        this._contenido = new Dictionary<string, List<int>>();

        this.MostFrequentCount = 0;
        
        for(int i = 0;i < terminos.Length;++i){
            if(!this._contenido.ContainsKey(terminos[i])){
                this._contenido.Add(terminos[i],new List<int>());
                this._contenido[terminos[i]].Add(i);
            }
            else
                this._contenido[terminos[i]].Add(i);
            
            MostFrequentCount = Math.Max(MostFrequentCount,this._contenido[terminos[i]].Count);
        }
    }
    //Crea este documento a partir de una copia de otro 
    public Documento(Documento other){
        this.Titulo = other.Titulo;
        this.MostFrequentCount = other.MostFrequentCount;
        this._contenido = other.CopiarContenido();
        this._texto =(string[]) other._texto.Clone();
    }

    //Metodo auxiliar para devlover una copia del contenido
    public Dictionary<string,List<int>> CopiarContenido(){
        //Copia cada entrada del diccionario
        Dictionary<string,List<int>> contenido = new Dictionary<string, List<int>>();
        foreach(var c in _contenido){
            contenido.Add(c.Key,c.Value);
        }
        return contenido;
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
    public string GetSnippet(string[] terminos){
        string snippet = "";

        //De la palabra
        string query = terminos[0];

        //Busca su primera aparicion
        if(this.FrecuenciaBooleana(query)){
            // int pos = this._contenido[query].First();
            System.Console.WriteLine($"{query} Aparece {this._contenido[query].Count} veces");
            // System.Console.WriteLine($"Su primera ocurrencia es {pos}");
            System.Console.WriteLine($"Existen un total de {this._texto.Length} palabras");
            
            int idealPos = -1;//Posicion ideal para hallar el snippet
            int score = 0;//Valor que genera esta posicion

            //Por cada posicion donde aparece la palabra
            foreach(int pos in this._contenido[query]){
                System.Console.WriteLine(pos);
                //Cuan buena es esta posicion
                int cntRep = 0;//Cantidad de veces que se repite query
                for(int i = Math.Max(0,pos - SnippetLength / 2), snippetWords = 0;i < this._texto.Length && snippetWords < SnippetLength;++i,++snippetWords){
                    if(this._texto[i] == query)++cntRep;
                }
                if(cntRep > score){
                    score = cntRep;
                    idealPos = pos;
                }
            }
            System.Console.WriteLine(idealPos);
            //Computa el snippet
            for(int i = Math.Max(0,idealPos - SnippetLength / 2), snippetWords = 0;i < this._texto.Length && snippetWords < SnippetLength;++i,++snippetWords){
                snippet += this._texto[i] + " ";
            }
        }

        return snippet;
    }
    #endregion Metodos
}