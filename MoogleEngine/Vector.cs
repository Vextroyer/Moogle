namespace MoogleEngine;

/**
*Esta clase representa un documento como un vector n-dimensional donde cada dimension del vector constituye una palabra
*de un conjunto (En este caso el conjunto de todas las palabras con las que se puede formar el corpus) y cada componente
*representa la cantidad de veces que se repite la palabra en el documento.
**/

class Vector{

    /**
    *Por motivos de eficiencia utilizo un diccionario para representar el vector.
    *Las llaves del diccionario representan las dimensiones del vector, si una llave no aparece significa que el valor de
    *esa componente es 0.
    *Los valores del diccionario representan los valores de las componentes del vector
    **/
    private Dictionary<string,int> _vector;
    
    #region Constructores
    //Crea un vector a partir del contenido de un documento
    public Vector(Dictionary<string,List<int>> contenido){
        this._vector = new Dictionary<string,int>();
        foreach(var entry in contenido){
            this._vector.Add(entry.Key,entry.Value.Count);
        }
    }
    //Crea un vector a partir del contenido de otro vector.
    public Vector(Dictionary<string,int> otherVectorContent){
        this._vector = new Dictionary<string, int>(otherVectorContent);
    } 
    #endregion Constructores
    
    #region  Propiedades
    public int this[string index]{
        get{
            if(index == null || !this._vector.ContainsKey(index))return 0;
            return this._vector[index];
        }
    }
    //Accede a las distintas palabras que conforman el vector
    public string[] Dimensiones{
        get{
            string[] dimen = new string[this._vector.Count];
            int i = 0;
            foreach(var entry in this._vector){
                dimen[i] = entry.Key;
                ++i;
            }
            return dimen;
        }
    }
    #endregion Propiedades

    #region Operadores
    //Producto Punto, o producto de dos vectores. Es la suma de los prouctos componente a componente.
    public static double operator*(Vector v,Vector u){
        double dotProduct = 0.0;
        
        foreach(string palabra in v.Dimensiones){
            dotProduct += v[palabra] * u[palabra];
        }
        return dotProduct;
    }
    //Suma de vectores. Es la suma componente a componente
    public static Vector operator+(Vector v,Vector u){
        Dictionary<string,int> nuevoVector = new Dictionary<string, int>();

        foreach(string palabra in v.Dimensiones){
            nuevoVector.Add(palabra,v[palabra]);
        }
        foreach(string palabra in u.Dimensiones){
            if(!nuevoVector.ContainsKey(palabra))nuevoVector.Add(palabra,u[palabra]);
            else nuevoVector[palabra] += u[palabra];
        }

        return new Vector(nuevoVector);
    }
    #endregion Operadores
}