namespace MoogleEngine;

/**
*Clase para computar el snippet.
*Un snippet es un trozo de documento. Un snippet es un subdocumento o subconjunto del documento.
**/

static class Snippet{
    private const int SnippetLength = 20;//Cantidad maxima de terminos mostrados en el snippet

    /**
    *Computa el mejor snippet para el documento basado en la consulta.
    *Por cada aparicion de cada termino unico de la consulta en el documento
    *creo un subdocumento cuyo termino central es el termino de la consulta y su 
    *texto son los terminos vecinos del temino de la consulta . Luego devuelvo el mejor de estos
    *documentos relativos a la consulta.
    **/
    public static string GetSnippet(string query,Documento doc){
        string snippet = "";
        
        //De la query
        Documento queryDoc = new Documento(query,"I am your query");//Creo un Documento a partir de mi query
        string[] terminosQuery = queryDoc.TerminosSinRepeticiones;//Determino los terminos unicos de mi query
        
        //Del documento
        string[] terminosDocumento = doc.Terminos;//Terminos del documento
        int[] posicionTerminosDocumento = doc.PosicionEnTexto;//Posiciones en el texto que ocupan los terminos
        List<Documento> subDocumentos = new List<Documento>();//Documentos formados por subconjuntos de los terminos del documento que determinan los terminos de la query
        List<(int,int)>posiciones = new List<(int, int)>();//Posiciones en el texto del documento donde comienza y termina los terminos del subdocumento

        //Por cada aparicion de algun termino de la query en el documento
        foreach(string term in terminosQuery){
            if(doc.NoContiene(term))continue;
            foreach(int pos in doc.Contenido[term]){

                //Construyo un subdocumento con esta seccion del documento
                string textoSubDocumento = "";
                
                int minPos = Math.Max(0,pos - SnippetLength / 2);//Indice del termino inicial
                int maxPos = 0;//Indice del termino final

                //El nuevo subdocumento tendra como contenido una vecindad del termino de query
                for(int i = minPos, snippetWords = 0;i < terminosDocumento.Length && snippetWords < SnippetLength;++i,++snippetWords){
                    textoSubDocumento += terminosDocumento[i] + " ";
                    maxPos = Math.Max(maxPos,i);
                }
                subDocumentos.Add(new Documento(textoSubDocumento,"I am your subDocument"));
                posiciones.Add((posicionTerminosDocumento[minPos],posicionTerminosDocumento[maxPos] + terminosDocumento[maxPos].Length));
            }
        }

        //Si no aparece ningun termino en este documento
        if(subDocumentos.Count == 0)return snippet;

        Vector[] vectoresSubDocumentos = new Vector[subDocumentos.Count];
        for(int i =0;i<vectoresSubDocumentos.Length;++i)vectoresSubDocumentos[i] = new Vector(subDocumentos[i]);
        double[] valor = Valorador.Valorar(new Vector(queryDoc),vectoresSubDocumentos);

        //Mi snippet es el subdocumento de mejor score
        int posicionMejor = 0;
        double valorMejor = double.MinValue;
        for(int i=0;i<valor.Length;++i){
            if(valorMejor < valor[i]){
                valorMejor = valor[i];
                posicionMejor = i;
            }
        }

        //Construye el snippet a partir del texto original que contiene a los terminos del subdocumento. Por motivos de visualidad.
        string texto = doc.Texto;
        for(int i = posiciones[posicionMejor].Item1;i<posiciones[posicionMejor].Item2;++i){
            snippet += texto[i];
        }

        return snippet;
    }
}