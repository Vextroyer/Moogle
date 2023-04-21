﻿namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query) {
        //Carga los documentos
        if(!Coleccion.Inicializada)Coleccion.Inicializar(Cargador.Load());

        //Procesar la consulta
        string[] terminosDeLaConsulta = Tokenizer.ProcesarQuery(query);
        System.Console.Write(terminosDeLaConsulta.Length + " ");
        foreach(string s in terminosDeLaConsulta)System.Console.Write(s);
        System.Console.WriteLine();

        //Esta funcionalidad se puede encapsular
        //Determina el score de cada documento
        double[] score = Valorador.Valorar(terminosDeLaConsulta,Coleccion.Documentos);

        //Crea un resultado por cada documento
        SearchItem[] items = new SearchItem[Coleccion.Count];
        for(int i=0;i<items.Length;++i){
            items[i] = new SearchItem(Coleccion.At(i).Titulo,Snippet.GetSnippet(Tokenizer.CombinarEnTexto(terminosDeLaConsulta),Coleccion.At(i)),score[i]);
        }

        //Ordena los documentos basados en su score descendentemente
        items = Sorter.Sort(items);

        //No muestres resultados irrelevantes en la busqueda
        items = Depurador.Depurar(items);
        //Hasta aqui

        return new SearchResult(items, query);
    }
}
