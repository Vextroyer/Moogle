namespace MoogleEngine;

/**
*Esta clase se encarga de procesar los textos a un formato asequible para otras partes del programa.
**/
static class Tokenizer{

    /**
    *Dado un array realizar operaciones basicas de formato sobre el
    *1-Conservar solo letras y numeros
    *2-Convertir a minusculas para obtener uniformidad
    *3-Convertir las vocales con tilde a vocales simples
    **/
    public static void Procesar(char[] txt){
        for(int i=0;i<txt.Length;++i){
            //Simbolos
            if(!char.IsLetterOrDigit(txt[i])){
                txt[i] = ' ';
                continue;
            }

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
    *Dado un texto devolver un array de los terminos que lo componen, donde cada termino es una palabra
    *o un numero, y un array de las posiciones que ocupan los terminos en el texto original. Ambos
    *arrays son del mismo tamano.
    **/
    public static (string[],int[]) ProcesarTexto(string texto){
        //Accede a los elementos del string para modificarlos
        char[] txt = texto.ToCharArray();
        Tokenizer.Procesar(txt);
        texto = new String(txt);       
        return Dividir(texto);
    }
}