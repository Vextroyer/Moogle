namespace MoogleEngine;

/**
*Esta clase se encarga de procesar los textos a un formato asequible para otras partes del programa.
**/
static class Tokenizer{
    
    /**
    *Dado un texto realizar 4 operaciones basicas de formato sobre el
    *1-Reemplazar los saltos de linea por espacios
    *2-Reemplazar las tabulaciones por espacios
    *3-Convertirlo a minusculas para obtener uniformidad
    *4-Convertir las vocales con tilde a vocales simples
    **/
    public static string[] Procesar(string query){
        //Accede a los elementos del string para modificarlos en una sola pasada
        char[] txt = query.ToCharArray();

        for(int i=0;i<txt.Length;++i){
            //Espacios
            if(txt[i] == '\n' || txt[i] == '\t'){
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
        query = new String(txt);       
        string[] terminos = query.Split(" ",StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return terminos;
    }
}