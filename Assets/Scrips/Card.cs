using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Card
{
    private int month;
    private Jokbo jokbo;
    private bool ssangpi;


    public Card(int _month, Jokbo _jokbo, bool _ssangpi) 
    {
        month = _month;
        jokbo = _jokbo;
        ssangpi = _ssangpi;
    }

    public int Get_month() { return month; }
    public Jokbo Get_jokbo()    { return jokbo; }
    public bool Get_ssangpi() { return ssangpi; }
}
