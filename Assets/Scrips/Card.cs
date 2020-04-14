using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Jokbo
{
    GWONG, GODORI, HONGDAN, CHODAN, CHUNGDAN, YEOLGGUT, PI
}

public class Card
{
    private int month;
    private Jokbo jokbo;


    public Card(int _month, Jokbo _jokbo) 
    {
        month = _month;
        jokbo = _jokbo;
    }

    public int Get_month() { return month; }
    

    public Jokbo Get_jokbo()    { return jokbo; }
}
