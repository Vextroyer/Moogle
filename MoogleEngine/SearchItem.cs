namespace MoogleEngine;

public class SearchItem
{
    public SearchItem(string title, string snippet, float score)
    {
        this.Title = title;
        this.Snippet = snippet;
        this.Score = score;
    }

    //Crea una copia de otro objeto SearchItem
    public SearchItem(SearchItem other){
        //Esto funciona porque el tipo string se copia por valor
        this.Title = other.Title;
        this.Snippet = other.Snippet;
        this.Score = other.Score;
    }

    public string Title { get; private set; }

    public string Snippet { get; private set; }

    public float Score { get; private set; }
}
