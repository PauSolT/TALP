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

    List<Syllab> syllabsPure = new();
    List<Syllab> syllabsTest = new();

    int currentNumberSyllab = 0;
    int correctAnswers = 0;
    int alphabet = 1;

    string currentSyllab = "";
    string save = "";


    // Start is called before the first frame update
    void Start()
    {
        LoadJson();
        InitSyllabes();
        PureKatakanaLearning();
    }

    void LoadJson()
    {
        romaji = JsonUtility.FromJson<Alphabet>(romajiJson.text);
        hiragana = JsonUtility.FromJson<Alphabet>(hiraganaJson.text);
        katakana = JsonUtility.FromJson<Alphabet>(katakanaJson.text);
    }

    void InitSyllabes()
    {
        for (int i = 0; i < romaji.pure.Length; i++)
        {
            Syllab syllab = new();
            syllab.romaji = romaji.pure[i];
            syllab.hiragana = hiragana.pure[i];
            syllab.katakana = katakana.pure[i];
            syllabsPure.Add(syllab);
        }
    }

    void PureHiraganaLearning()
    {
        syllabsTest = syllabsPure;
        answerText.text = "";
        alphabet = 1;
        DisplaySyllab(syllabsTest[currentNumberSyllab]);
        inputAnswer.text = syllabsTest[currentNumberSyllab].romaji;
    }

    void PureKatakanaLearning()
    {
        syllabsTest = syllabsPure;
        answerText.text = "";
        alphabet = 2;
        DisplaySyllab(syllabsTest[currentNumberSyllab], alphabet);
        inputAnswer.text = syllabsTest[currentNumberSyllab].romaji;
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
        inputAnswer.text = syllabsTest[currentNumberSyllab].romaji;
        correctText.text = "✔ "+ (currentNumberSyllab +1) + "/" + syllabsTest.Count;
    }

    public void CheckAnswer()
    {
        string answer = inputAnswer.text;
        string syl = currentSyllab;
        inputAnswer.text = "";
        answerText.text = syl;

        if (string.Compare(answer, syllabsTest[currentNumberSyllab].romaji) == 0)
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
        if (currentNumberSyllab < syllabsTest.Count-1)
        {
            currentNumberSyllab++;
            DisplaySyllab(syllabsTest[currentNumberSyllab], alphabet);
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
