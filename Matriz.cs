/**IMPLEMENTACION DE LA CLASE MATRIZ
Una matriz es una estructura ordenada en filas y columna
Las matrices que representa esta clase son de coeficientes reales
**/
class Matriz{
    #region Miembros
    
    //Tomo el nombre de A para mantener la relacion con la notacion matematica A[i][j]
    //Utilizo un arreglo bidimensional que es estructuralmente equivalente a una matriz
    private double[,] A;

    #endregion Miembros

    #region Propiedades
    //Indizador
    public double this[int fila,int columna]{
        get{
            if(fila < 0 || fila > this.Dimension.Item1 || columna < 0 || columna > this.Dimension.Item2)throw new IndexOutOfRangeException();
            return A[fila,columna];
        }
        set{
            if(fila < 0 || fila > this.Dimension.Item1 || columna < 0 || columna > this.Dimension.Item2)throw new IndexOutOfRangeException();
            A[fila,columna] = value;
        }
    }
    //Dimensiones de la matriz. El primer valor devuelto representa las filas y el segundo valor representa las columnas.
    public (int,int) Dimension{
        get{
            return (A.GetLength(0),A.GetLength(1));
        }
    }

    //Devuelve la matriz traspuesta correspondiente a esta matriz
    public Matriz Traspuesta{
        get{
            //Invierte filas y columnas
            double[,] traspuesta = new double[this.Dimension.Item2,this.Dimension.Item1];

            for(int j = 0;j<this.Dimension.Item2;++j){
                for(int i=0;i<this.Dimension.Item1;++i){
                    traspuesta[j,i] = this[i,j];
                }
            }

            return new Matriz(traspuesta);
        }
    }
    #endregion Propiedades

    #region Constructores
    //Crea una Matriz vacia, no confundir matriz nula con matriz vacia
    public Matriz(){
        A = new double[0,0];
    }
    //Crea una Matriz a partir de un arreglo bidimensional.
    public Matriz(double[,] B):this(){
        Inicializar(B);
    }
    //Crea una matriz nula de dimension n x m
    public Matriz(int n,int m){
        if(n <= 0 && m <= 0)throw new InvalidDataException("Ambas dimensiones deben ser numeros estrictamente mayores que 0");

        A = new double[n,m];
    }

    //Metodo para asignar los valores a una matriz
    public void Inicializar(double[,] B){
        
        A = new double[B.GetLength(0),B.GetLength(1)];
        
        for(int i=0;i<A.GetLength(0);++i){
            for(int j=0;j<A.GetLength(1);++j){
                this[i,j] = B[i,j];
            }
        }
    }

    #endregion Constructores

    #region Metodos

    //Imprime la matriz
    override public string ToString(){
        int filas = this.Dimension.Item1;
        int columnas = this.Dimension.Item2;
        string s = "";
        for(int i=0;i<filas;++i){
            for(int j=0;j<columnas;++j){
                if(j == 0)s += "( ";
                s += this[i,j] + " ";
                if(j == columnas - 1)s += ")\n";
            }
        }
        return s;
    }
    /*
    //Imprime la matriz como un sistema de ecuaciones
    public static void ImprimirComoSistema(Matriz M){
        int fil = M.Dimension.Item1;
        int col = M.Dimension.Item2;

        for(int i=0;i<fil;++i){
            Console.Write("{ ");
            for(int j=0;j<col;++j){
                Console.WriteLine($"{M.Sub(i,j)} * x{j} ");
                if(j < col - 1)Console.Write(" + ");
            }
            Console.WriteLine();
        }
    }
    */

    #region Operaciones

    //Calcula la suma algebraica elemento a elemento de dos matrices, solo esta garantizado su funcionamiento para matrices de iguales dimensiones
    public static Matriz operator+(Matriz M1,Matriz M2){
        if(!SePuedenSumar(M1,M2))throw new InvalidOperationException("No se pueden sumar matrices de diferente dimension");
        double[,] suma = new double[M1.Dimension.Item1,M1.Dimension.Item2];
        for(int i=0;i<M1.Dimension.Item1;++i){
            for(int j=0;j<M1.Dimension.Item2;++j){
                suma[i,j] = M1[i,j] + M2[i,j];
            }
        }
        return new Matriz(suma);
    }
    public static bool SePuedenSumar(Matriz M1,Matriz M2){
        return M1.Dimension == M2.Dimension;
    }

    //Calcula el producto de dos matrices, solo esta garantizado su funcionamiento para matrices de dimensiones n x p , p x m
    public static Matriz operator*(Matriz M1,Matriz M2){
            if(!SePuedenMultiplicar(M1,M2))throw new InvalidOperationException("No se pueden sumar matrices de diferente dimension");

            //El resutado de multiplicar dos matrices es una matriz con una cantida de filas igual a la primera y una cantidad de columnas igual a la segunda
            double[,] producto = new double[M1.Dimension.Item1,M2.Dimension.Item2];


            for(int i=0;i<M1.Dimension.Item1;++i){
                for(int j=0;j<M2.Dimension.Item2;++j){
                    for(int k=0;k<M1.Dimension.Item2;++k){
                        producto[i,j] += M1[i,k] * M2[k,j];
                    }
                }
            }
            return new Matriz(producto);
    }
    public static bool SePuedenMultiplicar(Matriz M1,Matriz M2){
        return M1.Dimension.Item2 == M2.Dimension.Item1;
        //La cantidad de columnas de la primera es igual a la cantidad de filas de la segunda
    }

    #region Metodo de Gauss   
    //Escalonar una matriz utilizando el metodo de Gauss
    public static Matriz EscalonarPorGauss(Matriz M1){

        int fil = M1.Dimension.Item1;
        int col = M1.Dimension.Item2; 
        double[,] matriz = new double[fil,col];

        for(int i=0;i<fil;++i){
            for(int j=0;j<col;++j){
                matriz[i,j] = M1[i,j];
            }
        }
        
        //Comienza el metodo de Gauss
        int ulFila = 0;//Posicion de la ultima fila que se puede utilizar
        //Por cada columna, excepto los terminos independientes
        for(int k=0;k < col - 1;++k){
            //Seleccionar una fila pivote y colocarla de fila superior
            int p = -1;//inicialmente no existe pivote
            
            //Ignorar las filas superiores que ya estan reducidas
            for(int i=ulFila;i<fil;++i){
                //Si el Aik != 0 entonces es un pivote valido
                if(matriz[i,k] != 0){
                    p = i;
                    break;
                }
            }
            if(p == -1)continue;//Para esta columna no es necesario hacer transformaciones
            
            //Si llego a este paso, es necesario incrementar la ulFila antes de pasar al paso siguiente

            //Colocar el pivote como fila superior
            //Por cada columna
            for(int j=0;j<col;j++){
                double c = matriz[ulFila,j];
                matriz[ulFila,j] = matriz[p,j];
                matriz[p,j] = c;
            }

            //Eliminando incognitas
            for(int i=ulFila+1;i<fil;++i){
                double c = - matriz[i,k] / matriz[ulFila,k];
                for(int j = k;j<col;++j){
                    matriz[i,j] += c * matriz[ulFila,j];
                }
            }

            //Aqui incremento ulFila
            ulFila++;
        }

        return new Matriz(matriz);
    }
    //Sobrecarga del Metodo EscalonarPorGauss para que funcione con arreglos bidimensionales
    public static Matriz EscalonarPorGauss(double[,] M1){
        return EscalonarPorGauss(new Matriz(M1));
    }
    #endregion Metodo de Gauss

    #endregion Operaciones


    #endregion Metodos
}
