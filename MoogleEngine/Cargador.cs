namespace MoogleEngine;
/**
*Se encarga de la entrada de informacion desde la carpeta Content,su procesamiento y distribucion a las otras partes pertinentes del programa
**/
public static class Cargador{
    private static Documento[] _documentos;//Contiene el corpus de Documentos. De esta forma solo es necesario cargarlos una sola vez.
    private const string contentDir = "../Content";//Path del directorio donde estan los documentos

    //Carga los archivos y devuelve documentos con informacion de cada archivo
    public static Documento[] Load(){
        //Si ya se cargaron los documentos, devuelve una copia
        if(_documentos != null){
            Documento[] documentosCopia = new Documento[_documentos.Length];
            for(int i=0;i<_documentos.Length;++i)documentosCopia[i] = new Documento(_documentos[i]);
            return documentosCopia;
        }

        //De la carpeta Content :

        string[] archivos = Directory.EnumerateFiles(contentDir).ToArray();//Identifica todos los archivos
        //Si no hay archivos, excepcion
        if(archivos.Length == 0)throw new Exception("No existen documentos que cargar, por favor anada documentos a la base de datos y ejecute nuevamente el programa.");

        //Pueden haber documentos vacios
        Documento[] documentos = new Documento[archivos.Length];//Crea tantos documentos como archivos

        //Hazle corresponder a cada documento un archivo
        int cntDocumentosVacios = 0;//Puede suceder que despues de procesado, un documento no tenga informacion util y por tanto este vacio
        for(int i=0;i<documentos.Length;++i){
            string[] contenido = ObtenElContenido(archivos[i]);
            if(contenido.Length > 0)
                documentos[i] = new Documento(contenido,ObtenElTitulo(archivos[i]));
            else
                ++cntDocumentosVacios;
        }
        
        //Si no hay documentos no vacios, excepcion
        if(documentos.Length - cntDocumentosVacios == 0)throw new Exception("No existe informacion relevante en la base de datos de documentos, por favor actualicela");

        //Documentos no vacios
        Documento[] documentosValidos = new Documento[documentos.Length - cntDocumentosVacios];
        for(int i=0,j = 0;i<documentos.Length && j < documentosValidos.Length;++i){
            if(documentos[i] == null)continue;
            documentosValidos[j] = documentos[i];
            ++j;
        }

        //Guarda los documentos para una proxima busqueda
        _documentos = new Documento[documentosValidos.Length];
        for(int i=0;i<_documentos.Length;++i)_documentos[i] = new Documento(documentosValidos[i]);

        return documentosValidos;
    }

    //Procesa y devuelve un listado de los terminos del archivo
    private static string[] ObtenElContenido(string archivo){
        string texto = File.ReadAllText(archivo);//Texto sin procesar

        texto = texto.ReplaceLineEndings(" ");//Convierte los saltos de lineas en espacios en blanco
        texto = texto.ToLower();//Convierte todas las letras a minusculas

        char[] textoFormateado = new char[texto.Length];//Texto temporal que contiene solo letras, numeros y espacios en blanco
        for(int j=0;j<textoFormateado.Length;++j)textoFormateado[j] = ' ';//Inicialmente esta compuesto por espacios en blanco
        for(int i = 0,j = 0;i<texto.Length;++i){
           if(char.IsLetterOrDigit(texto[i]) || char.IsWhiteSpace(texto[i])){
                //Reemplazar vocales con tilde por vocales simples
                switch(texto[i]){
                    case 'á':
                        textoFormateado[j] = 'a';
                        break;
            
                    case 'é':
                        textoFormateado[j] = 'e';
                        break;
            
                    case 'í':
                        textoFormateado[j] = 'i';
                        break;
            
                    case 'ó':
                        textoFormateado[j] = 'o';
                        break;
            
                    case 'ú':
                        textoFormateado[j] = 'u';
                        break;
            
                    default:
                        textoFormateado[j] = texto[i];
                        break;
                }
                ++j;
           }
        }
        texto = new string(textoFormateado);
        
        string[] terminos = texto.Split(" ",StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);//Separa cada termino y omite palabras vacias o de espacios
        
        return terminos;
    }

    /**
    *Procesa el nombre de un archivo y devuelve un titulo visualmente presentable
    *Titulo sin Procesar : Content/leonardo_da_vinci.txt
    *Titulo Procesado : "Leonardo Da Vinci"
    **/
    private static string ObtenElTitulo(string nombreArchivo){
        
        //nombreArchivo obedece al siguiente formato $"{contentDir}/{nombre}.{extension}", donde :
        // {contentDir} es el directorio donde estan contenidos los archivos
        // {nombre} es el nombre del archivo, en minusculas y con "_"(underscores) en vez de espacios
        // {extension} es la extension del archivo , en este caso .txt 
        
        string nombre = nombreArchivo.Substring(contentDir.Length + 1,nombreArchivo.Length - contentDir.Length - 1 - ".txt".Length);//Obten de nombreArchivo el {nombre}, para entender el por que de las expresiones dentro de Substring observa el ejemplo spbre el encabezado de la funcion

        string[] palabras = nombre.Split('_');//Dividelo en las palabras que lo conforman
        
        //Pon en mayuzcula las iniciales de las palabras y separalas por espacios 
        nombre = "";
        foreach(string p in palabras){
            char[] palabra = p.ToCharArray();
            if(palabra.Length  > 1)palabra[0] = char.ToUpper(palabra[0]);//No conviertas a mayusculas las expresiones como a, o, e, i ,u
            if(!string.IsNullOrEmpty(nombre))nombre = nombre + " ";
            nombre = nombre + new string(palabra);
        }

        return nombre;
    }

}
