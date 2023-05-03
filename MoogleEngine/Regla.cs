namespace MoogleEngine;
/**
*Constituye las reglas que se le deben aplicar a los resultados de la consulta basado en los
*operadores utilizados por el usuario.
*Los operadores implementados son ! ^ * ~
*Los operadores ! ^ * tienen la misma precedencia , digamos 1. Ademas actuan como prefijos. (!x , ^x , *x)
*De tener una expresion como (!^**!****^^x) el operador mas cercano a la palabra es el que se evalua.
*El operador ~ tiene precedencia 2. Ademas actua como infijo. (x ~ y)
**/
class Regla{
    #region Miembros
    private List<string> _not;//Representa la regla !. Los resultados de la consulta no deben contener ninguna de estas palabras.
    private List<string> _must;//Representa la regla ^. Los resultados de la consulta deben contener todas estas palabras.
    private List<(string,int)> _should;//Representa la regla *. Se debe dar mayor puntuacion a los resultados que contengan esta palabra
    //acore al nivel de relevancia dado por el usario con la cantidad de asteriscos.
    private List<(string,string)> _close;//Representa la regla ~. Se debe dar mayor puntuacion a los resultados mientras mas cerca este
    //la palara.
    #endregion Miembros
    
    #region Constructores
    //Constructor por defecto
    public Regla(){
        this._not = new List<string>();
        this._must = new List<string>();
        this._should = new List<(string, int)>();
        this._close = new List<(string, string)>();
    }
    //Crea un conjunto de reglas a partir de un string que contiene terminos y operadores
    public Regla(string[] tokens):this(){
        //Antes de entrar aqui llama al constructor por defecto

        //Primera pasada, aplica operadores unarios(!,^,*)
        for(int i=0;i<tokens.Length;++i){
            System.Console.WriteLine(tokens[i]);
            if(EsTermino(tokens[i]))continue;//Si no es un termino
            switch(tokens[i]){
                case "!":
                    if(i + 1 < tokens.Length && EsTermino(tokens[i + 1]))this._not.Add(tokens[i + 1]);
                    break;
                
                case "^":
                    if(i + 1 < tokens.Length && EsTermino(tokens[i + 1]))this._must.Add(tokens[i + 1]);
                    break;
                
                case "*":
                    int cantidadDeAsteriscos = 1;
                    while(i + 1 < tokens.Length && tokens[i + 1] == "*"){
                        ++i;
                        ++cantidadDeAsteriscos;
                    }
                    if(i + 1 < tokens.Length && EsTermino(tokens[i+1]))this._should.Add((tokens[i + 1],cantidadDeAsteriscos));
                    break;
            }
        }

        List<string> terminosYCercania = new List<string>();//Lista que solo contiene terminos y operadores de cercania.Para aplicar el operador de cercania con una precedencia de 2
        foreach(string s in tokens)if(EsTermino(s) || s == "~")terminosYCercania.Add(s);
        //2da pasada. Aplica el operador cercania.
        for(int i=0;i<terminosYCercania.Count;++i){
            if(terminosYCercania[i] == "~" && i - 1 >= 0 && i + 1 < terminosYCercania.Count && EsTermino(terminosYCercania[i -1]) && EsTermino(terminosYCercania[i+1]))
                this._close.Add((terminosYCercania[i - 1],terminosYCercania[i + 1]));
        }
    }
    //Crea esta regla a partir de una copia de otra. Si es para snippet omite los operadores !(not) y ^(must).
    public Regla(Regla other, bool esParaSnippet = false):this(){
        //Si no es para snippet copia las reglas de los operadores !(not) y ^(must)
        if(!esParaSnippet){
            this._not = new List<string>(other.Not);
            this._must = new List<string>(other.Must);
        }
        //Copia las reglas de los operadores *(should) y ~(close)
        this._should = new List<(string, int)>(other.Should);
        this._close = new List<(string, string)>(other.Close);
    }
    #endregion Constructores
    
    #region Propiedades
    public string[] Not{
        get{
            return this._not.ToArray();
        }
    }
    public string[] Must{
        get{
            return this._must.ToArray();
        }
    }
    public (string,int)[] Should{
        get{
            return this._should.ToArray();
        }
    }
    public (string,string)[] Close{
        get{
            return this._close.ToArray();
        }
    }
    //Retorna verdadero si existe al menos una regla
    public bool IsEmpty{
        get{
            return (this._not.Count > 0) || (this._must.Count > 0) || (this._should.Count > 0) || (this._close.Count > 0);
        }
    }
    #endregion Propiedades

    #region Metodos
    private bool EsOperador(string token){
        if(string.IsNullOrEmpty(token))return false;
        return (token == "!" || token == "*" || token == "^" || token == "~");
    }
    private bool EsTermino(string token){
        if(string.IsNullOrEmpty(token))return false;
        return !EsOperador(token);
    }

    public override string ToString()
    {
        string toString = "";

        toString += "Regla---------\n";
        
        toString += "! ";
        foreach(string t in this._not)toString += t + " ";
        toString += "\n";

        toString += "^ ";
        foreach(string t in this._must)toString += t + " ";
        toString += "\n";

        toString += "* ";
        foreach(var v in this._should){
            for(int i =0;i<v.Item2;++i)toString += "*";
            toString += v.Item1 + " ";
        }
        toString += "\n";

        toString += "~ ";
        foreach(var v in this._close){
            toString +=  v.Item1 + " ~ " + v.Item2 + " ";
        }
        toString += "\n";

        toString += "FinRegla------\n";

        return toString;
    }
    
    //Devuelve una version de esta regla compatible con el calculo de snippet. Solo tiene operadores ~(close) y *(must).
    public Regla ParaSnippet(){
        return new Regla(this,true);
    }

    //Metodos auxliares para determinar valores para las reglas should(*)  y close(~)

    public static double CalcularShould(int cantidadDeAsteriscos){
        //Si tiene un asterisco es el doble de importante en la consulta que los demas terminos.
        //Si tiene k asteriscos, es k+1 veces mas importante en la consulta que los demas terminos.
        return cantidadDeAsteriscos + 1;
    }

    public static double CalcularClose(int cercania){
        if(cercania <= 0)return 1;//No modifica el valor
        //Aplicar la formula 1 + (B / (cercania - A)) donde:
        //A , B son parametros
        //cercania siempre es un numero natural. Garantizado arriba.
        //Esta funcion inversamente proporcional aumenta mientras la cercania disminuye y disminuye mientras la cercania
        //aumenta. Justamente el efecto deseado.
        //Experimente con distintos valores de cercania , A y B para entender completamente.
        //El sumando 1 garantiza que el retorno de la funcion siempre sea mayor que 1. Haciendo que documentos donde aparezcan
        //ambas palabras tengan mayor relevancia, auqnue este efecto no es notable para valores grandes de cercania.

        double A = 0.9;
        double B = 1500;

        return 1 + (B / (cercania - A));
        //Los valores actuales de A y B son son establecidos experimentalmente para que en la consulta
        //algoritmo ~ Bernoulli Ciencia de la Computacion aparezca como primer resultado.
    }

    #endregion Metodos
}