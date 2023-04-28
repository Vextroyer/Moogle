namespace MoogleEngine;

/**
*Esta clase se encarga de procesar los textos a un formato asequible para otras partes del programa.
**/
static class Tokenizer{
    /**
    *Dado un texto devolver un array de los terminos que lo componen, donde cada termino es una palabra
    *o un numero, y un array de las posiciones que ocupan los terminos en el texto original. Ambos
    *arrays son del mismo tamano.
    **/
    public static (string[],int[]) ProcesarTexto(string texto){
        texto = Normalizar(texto,true);
        return Dividir(texto);
    }
    /**
    *Toma un string, que representa un texto y devuelve un array de string que representan los terminos que lo componen.
    *Para esto diferencia los terminos por los espacios en blanco.
    *ADVERTENCIA:
    *Este metodo divide el string teniendo en cuenta los espacios en blanco, todo caracterer que no sea espacio en blanco
    *es considerado parte de un termino.
    **/
    public static (string[],int[]) Dividir(string texto){
        //Anade un caracter ' ' al final para facilitar el procesamiento
        texto += " ";

        List<int>posiciones = new List<int>();
        List<string>terminos = new List<string>();
        string ultTerminoVisto = "";//Ultimo termino visto
        int posUltTerminoVisto = -1;//Posicion inicial del ultimo termino visto
        for(int i=0;i<texto.Length;++i){
            //Si es espacio
            if(texto[i] == ' '){
                //Si ya existe algun termino antes de este espacio entonces ese termino acaba aqui
                if(posUltTerminoVisto != -1){
                    posiciones.Add(posUltTerminoVisto);
                    terminos.Add(ultTerminoVisto);
                }
                ultTerminoVisto = "";
                posUltTerminoVisto = -1;
                continue;
            }
            //Si es el primer caracter del termino
            if(posUltTerminoVisto == -1)posUltTerminoVisto = i;
            ultTerminoVisto += texto[i];
        }

        if(posiciones.Count != terminos.Count)throw new Exception("No coinciden la cantidad de palabras con la cantidad de posiciones. Revise la implementacion.");
        return (terminos.ToArray(),posiciones.ToArray());
    }
    /**
    *Dado una consulta por el usuario devolver los terminos que la componen, ademas de realizar otras operaciones sobre esta.
    *Operaciones :
    *1-Remover las StopWords. De esta forma no son tenidas en cuenta a la hora de computar el score.
    **/
    public static string ProcesarQuery(string query){
        //Determina los diferentes terminos
        string[] terminos = Tokenizer.Tokenize(query);
        //Elimina las StopWords
        List<string> terminosSinStopWord = new List<string>();
        foreach(string termino in terminos){
            //Si no es StopWord
            if(!Coleccion.EsStopWord(termino))terminosSinStopWord.Add(termino);
        }
        return CrearTexto(terminosSinStopWord.ToArray());
    }
    //Metodo que dado un conjunto de terminos (array de string) crea un texto, concatenando los string
    public static string CrearTexto(string[] terminos){
        string texto = "";
        foreach (string s in terminos)texto += s + " ";
        return texto;
    }
    /**
    *   Dado un texto devolver un listado de tokens (palabras y operadores).
    *   Un token es:
    *       1- cada simbolo ^ ! ~
    *       2- una secuencia finita de simbolos *
    *       3- una secuencia finita de letras o digitos
    **/
    private static string comodin = "/";//Caracter comodin, ningun caracter utilizable coincidira con el 
    public static string[] Tokenize(string texto){
        List<string> tokens = new List<string>();//tokens
        texto += comodin;
        string lastToken = comodin;
        //Por cada caracter del texto
        foreach(char c in texto){
            //En cada paso o se expande el token actual, o se agruega a la lista y se crea uno nuevo
            
            if(c == '!' || c == '^' || c == '~'){//Si es simbolo definido
                AddToken(tokens,lastToken);//Anade a la lista el antiguo
                lastToken = c.ToString();//Crea uno nuevo con este simbolo
                continue;
            }
            if(c == '*'){
                if(lastToken.Last() != '*'){//Si el anterior no es asterisco
                    AddToken(tokens,lastToken);//Anadelo
                    lastToken = "*";//Comienza una secuencia de asteriscos
                }
                else lastToken += "*";//COntinua la secuencia
                continue;
            }
            if(char.IsLetterOrDigit(c)){
                if(!char.IsLetterOrDigit(lastToken.Last()))//Si lo anterior no es un termino
                {
                    AddToken(tokens,lastToken);//Agregalo
                    lastToken = c.ToString();//Comienza un nuevo termino
                }
                else lastToken += c;//Si no, expandelo
                continue;
            }
            //Es un caracter de otro tipo
            AddToken(tokens,lastToken);
            lastToken = c.ToString();
        }
        //Normalizar
        for(int i=0;i<tokens.Count;++i)tokens[i] = Normalizar(tokens[i]);
        return tokens.ToArray();
    }
    //Metodo auxiliar para agregar un token a la lista
    private static void AddToken(List<string> tokens,string token){
        if(IsValidToken(token))tokens.Add(token);
    }
    //Dado un token determina si es valido
    private static bool IsValidToken(string token){
        if(token == "!" || token == "^" || token == "~")return true;
        if(token.First() == '*'){
            foreach(char c in token){
                if(c != '*')return false;
            }
            return true;
        }
        if(char.IsLetterOrDigit(token.First())){
            foreach(char c in token){
                if(!char.IsLetterOrDigit(c))return false;
            }
            return true;
        }
        return false;
    }
    /**
    *Realiza dos modificaciones sobre un texto
    *1-Convertir a minusculas para obtener uniformidad
    *2-Convertir las vocales con tilde a vocales simples
    *3-Si el texto es de un archivo conserva solamente letras y digitos
    **/
    private static string Normalizar(string texto,bool esTextoDeArchivo = false){
        char[]txt = texto.ToCharArray();
        for(int i=0;i<txt.Length;++i){
            //Ejecucion solo en textos de archivos
            if(esTextoDeArchivo && !char.IsLetterOrDigit(txt[i]))txt[i] = ' ';//Caracter delimitador
            //Minusculas
            txt[i] = char.ToLower(txt[i]);
            //Vocales
            switch(txt[i]){
               case 'á':
                    txt[i] = 'a';
                    break;

                case 'é':
                    txt[i] = 'e';
                    break;
            
                case 'í':
                    txt[i] = 'i';
                    break;
            
                case 'ó':
                    txt[i] = 'o';
                    break;
            
                case 'ú':
                    txt[i] = 'u';
                    break;
            }
        }
        return new String(txt);
    }
}