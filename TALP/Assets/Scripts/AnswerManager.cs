using System.Collections;
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
    public Text correctText;
    public Text bestText;
    public InputField inputAnswer;
    public Button buttonAnswer;

    Alphabet romaji = new();
    Alphabet hiragana = new();
    Alphabet katakana = new();

    List<Syllab> syllabesPure = new();
    List<Syllab> syllabesImpure = new();
    List<Syllab> syllabesTest = new();

    int currentNumberSyllab = 0;
    int correctAnswers = 0;
    int alphabet = 1;

    string currentSyllab = "";
    string save = "";


    // Start is called before the first frame update
    void Start()
    {
        LoadJson();
        InitPureSyllabes();
        InitImpureSyllabes();
        ImpureKatakanaLearning();
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

    void PureHiraganaLearning()
    {
        syllabesTest = syllabesPure;
        answerText.text = "";
        alphabet = 1;
        DisplaySyllab(syllabesTest[currentNumberSyllab], alphabet);
        inputAnswer.text = syllabesTest[currentNumberSyllab].romaji;
    }

    void PureKatakanaLearning()
    {
        syllabesTest = syllabesPure;
        answerText.text = "";
        alphabet = 2;
        DisplaySyllab(syllabesTest[currentNumberSyllab], alphabet);
        inputAnswer.text = syllabesTest[currentNumberSyllab].romaji;
    }

    void ImpureHiraganaLearning()
    {
        syllabesTest = syllabesImpure;
        answerText.text = "";
        alphabet = 1;
        DisplaySyllab(syllabesTest[currentNumberSyllab], alphabet);
        inputAnswer.text = syllabesTest[currentNumberSyllab].romaji;
    }

    void ImpureKatakanaLearning()
    {
        syllabesTest = syllabesImpure;
        answerText.text = "";
        alphabet = 2;
        DisplaySyllab(syllabesTest[currentNumberSyllab], alphabet);
        inputAnswer.text = syllabesTest[currentNumberSyllab].romaji;
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
    }

    public void CheckAnswer()
    {
        string answer = inputAnswer.text;
        string syl = currentSyllab;
        inputAnswer.text = "";
        answerText.text = syl;

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
