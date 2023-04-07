namespace MoogleEngine;

/**
*Esta clase se encarga de procesar los textos a un formato asequible para otras partes del programa.
**/
static class Tokenizer{
    public static string[] Procesar(string query){
        query = query.ToLower();
        string[] terminos = query.Split(" ",StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        //Vocales
        return terminos;
    }
}