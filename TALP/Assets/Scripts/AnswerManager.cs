﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerManager : MonoBehaviour
{
    public TextAsset romajiJson;
    public TextAsset hiraganaJson;
    public TextAsset katakanaJson;

    public Text questionText;
    public Text answerText;
    public Text answerRomajiText;
    public Text correctText;
    public Text bestText;
    public InputField inputAnswer;
    public Button buttonAnswer;

    Alphabet romaji = new();
    Alphabet hiragana = new();
    Alphabet katakana = new();

    List<Syllab> syllabesPure = new();
    List<Syllab> syllabesImpure = new();
    List<Syllab> syllabesDiphthong= new();
    List<Syllab> syllabesTest = new();

    int currentNumberSyllab = 0;
    int correctAnswers = 0;
    int alphabet = 1;

    string currentSyllab = "";
    string save = "";
    string saveTestHiraganaPure = "saveTestHiraganaPure";
    string saveTestHiraganaImpure = "saveTestHiraganaImpure";
    string saveTestHiraganaDiphthong = "saveTestHiraganaDiphthong";
    string saveTestHiragana = "saveTestHiragana";


    // Start is called before the first frame update
    void Start()
    {
        LoadJson();
        InitPureSyllabes();
        InitImpureSyllabes();
        InitDiphthongSyllabes();
    }

    void LoadJson()
    {
        romaji = JsonUtility.FromJson<Alphabet>(romajiJson.text);
        hiragana = JsonUtility.FromJson<Alphabet>(hiraganaJson.text);
        katakana = JsonUtility.FromJson<Alphabet>(katakanaJson.text);
    }

    void InitPureSyllabes()
    {
        for (int i = 0; i < romaji.pure.Length; i++)
        {
            Syllab syllab = new();
            syllab.romaji = romaji.pure[i];
            syllab.hiragana = hiragana.pure[i];
            syllab.katakana = katakana.pure[i];
            syllabesPure.Add(syllab);
        }
    }

    void InitImpureSyllabes()
    {
        for (int i = 0; i < romaji.impure.Length; i++)
        {
            Syllab syllab = new();
            syllab.romaji = romaji.impure[i];
            syllab.hiragana = hiragana.impure[i];
            syllab.katakana = katakana.impure[i];
            syllabesImpure.Add(syllab);
        }
    }

    void InitDiphthongSyllabes()
    {
        for (int i = 0; i < romaji.diphthong.Length; i++)
        {
            Syllab syllab = new();
            syllab.romaji = romaji.diphthong[i];
            syllab.hiragana = hiragana.diphthong[i];
            syllab.katakana = katakana.diphthong[i];
            syllabesDiphthong.Add(syllab);
        }
    }

    public void PureHiraganaTest(bool learning = false)
    {
        syllabesTest.Clear();
        syllabesTest.AddRange(syllabesPure);
        if (!learning)
            IListExtensions.Shuffle(syllabesTest);

        ChangeMode(1, learning);
    }

    public void PureHiraganaLearning()
    {
        PureHiraganaTest(true);
    }

    public void PureKatakanaTest(bool learning = false)
    {
        syllabesTest.Clear();
        syllabesTest.AddRange(syllabesPure);
        if (!learning)
            IListExtensions.Shuffle(syllabesTest);
        
        ChangeMode(2, learning);
        
    }

    public void PureKatakanaLearning()
    {
        PureKatakanaTest(true);
    }

    public void ImpureHiraganaTest(bool learning = false)
    {
        syllabesTest.Clear();
        syllabesTest.AddRange(syllabesImpure);
        if (!learning)
            IListExtensions.Shuffle(syllabesTest);
        
        ChangeMode(1, learning);
    }

    public void ImpureHiraganaLearning()
    {
        ImpureHiraganaTest(true);
    }

    public void ImpureKatakanaTest(bool learning = false)
    {
        syllabesTest.Clear();
        syllabesTest.AddRange(syllabesImpure);
        if (!learning)
            IListExtensions.Shuffle(syllabesTest);
        
        ChangeMode(2, learning);
        
    }

    public void ImpureKatakanaLearning()
    {
        ImpureKatakanaTest(true);
    }

    public void DiphthongHiraganaTest(bool learning = false)
    {
        syllabesTest.Clear();
        syllabesTest.AddRange(syllabesDiphthong);
        if (!learning)
            IListExtensions.Shuffle(syllabesTest);
        
        ChangeMode(1, learning);
    }

    public void DiphthongHiraganaLearning()
    {
        DiphthongHiraganaTest(true);
    }

    public void DiphthongKatakanaTest(bool learning = false)
    {
        syllabesTest = syllabesDiphthong;
        IListExtensions.Shuffle(syllabesTest);
        ChangeMode(2, learning);
    }

    public void DiphthongKatakanaLearning()
    {
        DiphthongKatakanaTest(true);
        syllabesTest = syllabesDiphthong;
        
    }

    public void HiraganaTest()
    {
        syllabesTest = syllabesPure;
        syllabesTest.AddRange(syllabesImpure);
        syllabesTest.AddRange(syllabesDiphthong);
        IListExtensions.Shuffle(syllabesTest);
        ChangeMode(1, false);
    }

    public void KatakanaTest()
    {
        syllabesTest = syllabesPure;
        syllabesTest.AddRange(syllabesImpure);
        syllabesTest.AddRange(syllabesDiphthong);
        IListExtensions.Shuffle(syllabesTest);
        ChangeMode(2, false);
    }

    void ChangeMode(int alph, bool learning )
    {
        answerText.text = "";
        answerRomajiText.text = "";
        alphabet = alph;
        correctText.text = "✔ ";
        currentNumberSyllab = 0;
        correctAnswers = 0;
        currentSyllab = "";
        DisplaySyllab(syllabesTest[currentNumberSyllab], alphabet);

        if (!learning)
        {
            buttonAnswer.onClick.RemoveAllListeners();
            buttonAnswer.onClick.AddListener(() => CheckAnswer());
            correctText.text = "✔ " + correctAnswers + "/" + (currentNumberSyllab);
        }
        else
        {
            buttonAnswer.onClick.RemoveAllListeners();
            buttonAnswer.onClick.AddListener(() => AnswerLearning());
            inputAnswer.text = syllabesTest[currentNumberSyllab].romaji;
            correctText.text = "✔ "+ (currentNumberSyllab +1) + "/" + syllabesTest.Count;
        }
    }


    void DisplaySyllab(Syllab syl, int alphabet = 1)
    {
        string syllab = alphabet == 1 ? syl.hiragana : syl.katakana;
        questionText.text = syllab;
        currentSyllab = syllab; 
    }

    public void AnswerLearning()
    {
        NextSyllab();
        inputAnswer.text = syllabesTest[currentNumberSyllab].romaji;
        correctText.text = "✔ "+ (currentNumberSyllab +1) + "/" + syllabesTest.Count;
        answerRomajiText.text = syllabesTest[currentNumberSyllab].romaji;
    }

    public void CheckAnswer()
    {
        string answer = inputAnswer.text.Trim();
        string syl = currentSyllab;
        inputAnswer.text = "";
        answerText.text = syl;
        answerRomajiText.text = syllabesTest[currentNumberSyllab].romaji;
        if (string.Compare(answer, syllabesTest[currentNumberSyllab].romaji) == 0)
        {
            answerText.color = Color.green;
            correctAnswers++;
            correctText.text = "✔ " + correctAnswers + "/" + (currentNumberSyllab+1);
        } else
        {
            answerText.color = Color.red;   
            correctText.text = "✔ " + correctAnswers + "/" + (currentNumberSyllab+1);
        }
        NextSyllab();
    }

    void NextSyllab()
    {
        if (currentNumberSyllab < syllabesTest.Count-1)
        {
            currentNumberSyllab++;
            DisplaySyllab(syllabesTest[currentNumberSyllab], alphabet);
        }
        else
        {
            questionText.text = "Fin";
        }
    }

    public class Syllab
    {
        public string romaji;
        public string hiragana;
        public string katakana;
    }

    class Alphabet
    {
        public string[] pure = null;
        public string[] impure = null;
        public string[] diphthong = null;
    }

    
}
public static class IListExtensions
{
    public static void Shuffle<T>(this IList<T> ts)
    {
        int count = ts.Count;
        int last = count - 1;
        for (int i = 0; i < last; ++i)
        {
            int r = Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}