using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word 
{
    public string japanese;
    public string kanji;
    public string word;
    public string extra;

    public Word(string jap, string kan, string wor, string ext)
    {
        japanese = jap;
        kanji = kan;
        word = wor;
        extra = ext;
    }
}
