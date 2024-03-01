using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Word 
{
[SerializeField]
    public string japanese;
[SerializeField]
    public string kanji;
[SerializeField]
    public string word;
[SerializeField]
    public string extra;

    public Word(string jap, string kan, string wor, string ext)
    {
        japanese = jap;
        kanji = kan;
        word = wor;
        extra = ext;
    }
}
