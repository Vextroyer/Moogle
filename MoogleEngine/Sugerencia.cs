namespace MoogleEngine;

/**
*   Esta clase se encarga de computar una sugerencia para la busqueda realizada por el usuario.
*   Computa la sugerencia termino a termino de la busqueda. Un termino es una secuencia finita
*   de letras y digitos. Utiliza la distancia de Levenstein entre terminos para computar sugerencias.
**/
static class Sugerencia{
    /**
    *   Miembro de datos que contiene los tokens de la busqueda actual.
    *   El metodo Tokenizer.ProcesarQuery(string query) actualiza el estado de este miembro de datos para que la clase 
    *   siempre contenga informacion de la consulta actual.   
    **/
    private static string[] _token = new string[0];

    //Metodo que establece los tokens de la consulta actual
    public static void Actualizar(string[] token){
        _token = (string[])token.Clone();
    }

    private static string[] _terminos = new string[0];//Son los distintos terminos que aparecen en la coleccion.
    //Metodo que establece los terminos de la coleccion. Es llamado desde la inicializacion de coleccion
    public static void Inicializar(string[] terminos){
        _terminos = terminos;
    }

    //Computa la sugerencia de la consulta actual
    public static string Sugerir(){
        string[] sugerencia = new string[_token.Length];//Listado de tokens de la sugerencia

        //Por cada token de la sugerencia
        for(int i=0;i<sugerencia.Length;++i){
            if(Tokenizer.EsTermino(_token[i])){//Si es un termino
                //Si este termino aparece en la coleccion lo mantengo
                if(Coleccion.EnCuantosDocumentosAparece(_token[i]) > 0){
                    sugerencia[i] = _token[i];
                }else{
                    //Si no aparece sugiero un termino similar que aparezca

                    string terminoSugerido = "";//Guarda el termino de menor diferencia,mayor similaridad
                    int similaridad = int.MaxValue;//Es la diferencia de terminoSugerido con el termino actual

                    foreach(string terminoActual in _terminos){
                        int similaridadActual = EditDistance(_token[i],terminoActual);
                        if(similaridadActual < similaridad){
                            similaridad = similaridadActual;
                            terminoSugerido = terminoActual;
                        }
                    }

                    sugerencia[i] = terminoSugerido;
                }

                continue;
            }
            sugerencia[i] = _token[i];//Si no es un termino es un operador, lo mantengo
        }
        return Tokenizer.CrearTexto(sugerencia);
    }

    //Calcula la distancia de Levenstein entre dos terminos
    private static int EditDistance(string word1,string word2){
        int n = word1.Length , m = word2.Length;//Tamano de las palabras
        int[,] d = new int[n + 1,m + 1];//Edit Distances, initially 0.
        for(int i=0;i<=n;++i)d[i,0] = i;//Inicializar filas
        for(int j=0;j<=m;++j)d[0,j] = j;//Inicializar filas

        for(int i=1;i<=n;++i){
            for(int j=1;j<=m;++j){
                int cost1 = d[i-1,j-1] + Match(word1[i-1],word2[j-1]);//Hacer coincidir los dos ultimos caracteres
                //Insertar o remover el ultimo caracter
                int cost2 = d[i-1,j] + 1;
                int cost3 = d[i,j-1] + 1;
                d[i,j] = Math.Min(cost1,Math.Min(cost2,cost3));
            }
        }
        return d[n,m];
    }
    //Devuelve 0 si dos caracteres son iguales, 1 en caso contrario
    private static int Match(char word1Char,char word2Char){
        if(word1Char == word2Char)return 0;
        return 1;
    }
}