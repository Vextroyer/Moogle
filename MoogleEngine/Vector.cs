namespace MoogleEngine;

/**
*Esta clase representa un documento como un vector n-dimensional donde cada dimension del vector constituye una palabra
*de un conjunto (En este caso el conjunto de todas las palabras con las que se puede formar el corpus) y cada componente
*representa el valor tf-idf de la palabra en el documento en la coleccion.
**/

class Vector{

    /**
    *Por motivos de eficiencia utilizo un diccionario para representar el vector.
    *Las llaves del diccionario representan las dimensiones del vector, si una llave no aparece significa que el valor de
    *esa componente es 0.
    *Los valores del diccionario representan los valores de las componentes del vector
    **/
    private Dictionary<string,double> _vector;
    
    #region Constructores
    //Crea un vector a partir de un documento
    public Vector(Documento documento){
        this._vector = new Dictionary<string, double>();
        foreach(string termino in documento.TerminosSinRepeticiones){
            this._vector.Add(termino,Valorador.Pesar(termino,documento));
        }
    }
    //Crea un vector a partir de otro vector
    public Vector(Vector other):this(other.Contenido){
    }
    //Crea un vector a partir del contenido de otro vector.
    private Vector(Dictionary<string,double> otherVectorContent){
        this._vector = new Dictionary<string, double>(otherVectorContent);
    } 
    #endregion Constructores
    
    #region  Propiedades
    //Accede al valor tf-idf del termino dado
    public double this[string index]{
        get{
            if(index == null || !this._vector.ContainsKey(index))return 0;
            return this._vector[index];
        }
    }
    //Devuelve la cantidad de palabras diferentes que componen al vector
    public int Dimension{
        get{
            return this._vector.Count;
        }
    }
    //Accede a los distintos terminos que conforman el vector
    public string[] Terminos{
        get{
            return this._vector.Keys.ToArray();
        }
    }
    //Accede a una copia del contenido del vector
    public Dictionary<string,double> Contenido{
        get{
            return new Dictionary<string,double>(this._vector);
        }
    }
    #endregion Propiedades

    #region Operadores
    //Producto Punto, o producto de dos vectores. Es la suma de los prouctos componente a componente.
    public static double operator*(Vector v,Vector u){
        double dotProduct = 0.0;
        //Solo las palabras presentes en ambos vectores, componentes diferentes de 0, aportan a la suma
        //Tecnica para multiplicar con el vector de menos dimensiones
        if(v.Dimension <= u.Dimension){
            foreach(string termino in v.Terminos){
                dotProduct += v[termino] * u[termino];
            }
        }
        else{
            foreach(string termino in u.Terminos){
                dotProduct += v[termino] * u[termino];
            }
        }
        return dotProduct;
    }
    //Suma de vectores. Es la suma componente a componente
    public static Vector operator+(Vector v,Vector u){
        Dictionary<string,double> nuevoVector = new Dictionary<string, double>();

        foreach(string palabra in v.Terminos){
            nuevoVector.Add(palabra,v[palabra]);
        }
        foreach(string palabra in u.Terminos){
            if(!nuevoVector.ContainsKey(palabra))nuevoVector.Add(palabra,u[palabra]);
            else nuevoVector[palabra] += u[palabra];
        }

        return new Vector(nuevoVector);
    }
    #endregion Operadores

    override public string ToString(){
        string s = "[";
        foreach(var entry in this._vector){
            s += new string($" <{entry.Key},{entry.Value}> ");
        }
        s+="]";
        return s;
    }
}