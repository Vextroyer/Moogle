namespace MoogleEngine;
/**
*Constituye las reglas que se le deben aplicar a los resultados de la consulta basado en los
*operadores utilizados por el usuario.
**/
class Regla{
    #region Miembros
    private string[] _not;//Representa la regla !. Los resultados de la consulta no deben contener ninguna de estas palabras.
    private string[] _must;//Representa la regla ^. Los resultados de la consulta deben contener todas estas palabras.
    private (string,int)[] _should;//Representa la regla *. Se debe dar mayor puntuacion a los resultados que contengan esta palabra
    //acore al nivel de relevancia dado por el usario con la cantidad de asteriscos.
    private (string,string)[] _close;//Representa la regla ~. Se debe dar mayor puntuacion a los resultados mientras mas cerca este
    //la palara.
    #endregion Miembros
    
    #region Constructores
    public Regla(string[] not,string[] must,(string,int)[] should,(string,string)[] close){
        this._not = new string[0];
        if(not != null)this._not =(string[]) not.Clone();

        this._must = new string[0];
        if(must != null)this._must =(string[]) must.Clone();

        this._should = new (string,int)[0];
        if(should != null)this._should =((string,int)[]) should.Clone();

        this._close = new (string,string)[0];
        if(close != null)this._close =((string,string)[]) close.Clone();
    }
    #endregion Constructores
    
    #region Propiedades
    public string[] Not{
        get{
            return (string[])this._not.Clone();
        }
    }
    public string[] Must{
        get{
            return (string[])this._must.Clone();
        }
    }
    public (string,int)[] Should{
        get{
            return ((string,int)[])this._should.Clone();
        }
    }
    public (string,string)[] Close{
        get{
            return ((string,string)[])this._close.Clone();
        }
    }
    #endregion Propiedades
}