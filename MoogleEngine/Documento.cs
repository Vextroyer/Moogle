namespace MoogleEngine;

/**
*Esta clase es la abstraccion de un documento.
**/
public class Documento{

    #region Miembros
    private string[] _terminos;//Estas son los terminos que componen el documento, un termino es una palabra o un numero.
    private int[] _posicionEnTexto;//Esta es la posicion inicial en texto de cada termino del documento
    private Dictionary<string,List<int>> _contenido;//Este diccionario representa el contenido del documento agrupado de la forma (termino, posiciones en _terminos en las que aparece)
    private int _mostFrequentTermCount;//Cantidad de veces que se repite el termino que mas se repite y no es stop word
    #endregion Miembros

    #region Propiedades
    //El titulo del Documento
    public string Titulo{
        get;
        private set;
    }
    //Este es el contenido textual del documento
    public string Texto{
        get;
        private set;
    }
    //Cantidad de veces que se repite el termino que mas se repite y no es stop word
    public int MostFrequentCount{
        get{
            //Si se calculan por primera vez
            if(this._mostFrequentTermCount == 0){
                foreach(var entry in this._contenido){
                    if(Coleccion.EsStopWord(entry.Key))continue;
                    this._mostFrequentTermCount = Math.Max(this._mostFrequentTermCount,entry.Value.Count);
                }
            }
            return this._mostFrequentTermCount;
        }
        private set{
            this._mostFrequentTermCount = value;
        }
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
    //Devuelve una copia del conjunto de terminos que conforman el documento, no hay repeticiones.
    public string[] TerminosSinRepeticiones{
        get{
            return this._contenido.Keys.ToArray();
        }
    }

    //Devuelve una copia del array posiciones
    public int[] PosicionEnTexto{
        get{
            return (int[])this._posicionEnTexto.Clone();
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

        this.Texto = texto;

        //Los terminos que componen el texto y las posiciones donde aparecen
        (string[],int[]) auxiliar = Tokenizer.ProcesarTexto(texto);
        this._terminos = auxiliar.Item1;
        this._posicionEnTexto = auxiliar.Item2;
        //Terminos y posiciones tienen la misma cantidad de elementos porque de lo contrario Tokenizer.ProcesarTexto(texto) hubiese lanzado una excepcion y este codigo no se hubiese ejecutado
        
        this._contenido = new Dictionary<string, List<int>>();

        this._mostFrequentTermCount = 0;

        for(int i = 0;i < this._terminos.Length;++i){
            if(!this._contenido.ContainsKey(this._terminos[i])) this._contenido.Add(this._terminos[i],new List<int>());//Si no existe el termino en el diccionario, agregalo.
            
            this._contenido[this._terminos[i]].Add(i);//Guarda la posicion en que aparece
        }
    }
    //Crea este documento a partir de una copia de otro 
    public Documento(Documento other){
        this.Titulo = other.Titulo;
        this.Texto = other.Texto;
        this.MostFrequentCount = other.MostFrequentCount;
        this._contenido = other.Contenido;
        this._terminos = other.Terminos;
        this._posicionEnTexto = other.PosicionEnTexto;
    }

    #endregion Constructores

    #region Metodos
    //Devuelve la cantidad de apariciones de termino en el documento
    public int TermCount(string termino){
        if(string.IsNullOrEmpty(termino) || !this._contenido.ContainsKey(termino))return 0;
        else return this._contenido[termino].Count;
    }
    //Retorna verdadero si el documento contiene el termino, falso de otra forma
    public bool Contiene(string termino){
        return TermCount(termino) > 0;//Si lo contiene al menos 1 vez entonces lo contiene 
    }
    //Retorna falso si el documento contiene al termino, verdadero de la otra forma
    public bool NoContiene(string termino){
        return !Contiene(termino);
    }
    //Determina la menor distancia entre alguna aparicion de estos terminos en el documento o -1 si alguno no aparece
    public int Cercania(string terminoA,string terminoB){
        if(this.NoContiene(terminoA) || this.NoContiene(terminoB))return -1;
        //Ambos aparecen
        
        //Determino las posiciones en el texto donde aparecen
        //Tomo las posiciones en termino donde aparece -> cada una de esta posiciones tiene asociada una posicion en texto -> tomo esa
        
        int[] aparicionesDeA = this._contenido[terminoA].ToArray();
        for(int i=0;i<aparicionesDeA.Length;++i)aparicionesDeA[i] = this._posicionEnTexto[aparicionesDeA[i]];

        int[] aparicionesDeB = this._contenido[terminoB].ToArray();
        for(int i=0;i<aparicionesDeB.Length;++i)aparicionesDeB[i] = this._posicionEnTexto[aparicionesDeB[i]];

        //Ambos arreglos estan ordenados ascendentemente por la forma en que se crean durante la creacion del documento
        int cercania = int.MaxValue;//valor maximo porque va a disminuir
        int aPuntero = 0;//Para moverse por las apariciones de A
        int bPuntero = 0;//Para moverser por la apariciones de B
        while(aPuntero < aparicionesDeA.Length && bPuntero < aparicionesDeB.Length){
            int distancia = Math.Abs(aparicionesDeA[aPuntero] - aparicionesDeB[bPuntero]);
            cercania = Math.Min(cercania,distancia);
            //Debo mover el puntero que contenga el menor valor, porque asi su valor aumentara debido a la monotonia creciente
            //de los arreglos, y solo asi es posible que la diferencia disminuya, si aumento el de mayor valor, la diferencia
            //necesariamente aumentara.
            if(aparicionesDeA[aPuntero] <= aparicionesDeB[bPuntero])++aPuntero;
            else ++bPuntero;
        }
        
        // for(int i=0;i<aparicionesDeA.Length;++i){
        //     for(int j=0;j<aparicionesDeB.Length;++j){
        //         int distancia = Math.Abs(aparicionesDeA[i] - aparicionesDeB[i]);
        //         if(distancia < cercania){
        //             cercania = distancia;
        //         }
        //     }
        // }

        return cercania;
    }

    #endregion Metodos
}