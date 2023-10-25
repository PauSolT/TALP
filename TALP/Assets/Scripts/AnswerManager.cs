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
    public List<Syllab> syllabesTest = new();

    int currentNumberSyllab = 0;
    int correctAnswers = 0;
    int alphabet = 1;

    string currentSyllab = "";
    string save = "";
    string saveTestHiraganaPure = "saveTestHiraganaPure";
    string saveTestHiraganaImpure = "saveTestHiraganaImpure";
    string saveTestHiraganaDiphthong = "saveTestHiraganaDiphthong";
    string saveTestHiragana = "saveTestHiragana";
    string saveTestKatakanaPure = "saveTestKatakanaPure";
    string saveTestKatakanaImpure = "saveTestKatakanaImpure";
    string saveTestKatakanaDiphthong = "saveTestKatakanaDiphthong";
    string saveTestKatakana = "saveTestKatakana";
    string saveTestHiraganaKatakana = "saveTestHiraganaKatakana";


    // Start is called before the first frame update
    void Start()
    {
        LoadJson();
        InitPureSyllabes();
        InitImpureSyllabes();
        InitDiphthongSyllabes();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            buttonAnswer.onClick.Invoke();
        }
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
        {
            IListExtensions.Shuffle(syllabesTest);
            save = saveTestHiraganaPure;
        }

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
        {
            IListExtensions.Shuffle(syllabesTest);
            save = saveTestKatakanaPure;
        }

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
        {
            IListExtensions.Shuffle(syllabesTest);
            save = saveTestHiraganaImpure;
        }
        
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
        {
            IListExtensions.Shuffle(syllabesTest);
            save = saveTestKatakanaImpure;
        }
        
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
        {
            IListExtensions.Shuffle(syllabesTest);
            save = saveTestHiraganaDiphthong;
        }

        ChangeMode(1, learning);
    }

    public void DiphthongHiraganaLearning()
    {
        DiphthongHiraganaTest(true);
    }

    public void DiphthongKatakanaTest(bool learning = false)
    {
        syllabesTest.Clear();
        syllabesTest.AddRange(syllabesDiphthong);
        if (!learning)
        {
            IListExtensions.Shuffle(syllabesTest);
            save = saveTestKatakanaDiphthong;
        }
        ChangeMode(2, learning);
    }

    public void DiphthongKatakanaLearning()
    {
        DiphthongKatakanaTest(true);
    }

    public void HiraganaTest()
    {
        save = saveTestHiragana;
        syllabesTest.Clear();
        syllabesTest.AddRange(syllabesPure);
        syllabesTest.AddRange(syllabesImpure);
        syllabesTest.AddRange(syllabesDiphthong);
        IListExtensions.Shuffle(syllabesTest);
        ChangeMode(1, false);
    }

    public void KatakanaTest()
    {
        save = saveTestKatakana;
        syllabesTest.Clear();
        syllabesTest.AddRange(syllabesPure);
        syllabesTest.AddRange(syllabesImpure);
        syllabesTest.AddRange(syllabesDiphthong);
        IListExtensions.Shuffle(syllabesTest);
        ChangeMode(2, false);
    }

    public void HiraganaAndKatakanaTest()
    {
        save = saveTestHiraganaKatakana;
        syllabesTest.Clear();
        syllabesTest.AddRange(syllabesPure);
        syllabesTest.AddRange(syllabesImpure);
        syllabesTest.AddRange(syllabesDiphthong);
        syllabesTest.AddRange(syllabesPure);
        syllabesTest.AddRange(syllabesImpure);
        syllabesTest.AddRange(syllabesDiphthong);
        IListExtensions.Shuffle(syllabesTest);
        ChangeMode(3, false);

    }

    void ChangeMode(int alph, bool learning)
    {
        answerText.text = "";
        inputAnswer.text = "";
        answerRomajiText.text = "";
        alphabet = alph;
        correctText.text = "✔";
        currentNumberSyllab = 0;
        correctAnswers = 0;
        currentSyllab = "";
        DisplaySyllab(syllabesTest[currentNumberSyllab], alphabet);

        if (!learning)
        {
            buttonAnswer.onClick.RemoveAllListeners();
            buttonAnswer.onClick.AddListener(() => CheckAnswer());
            correctText.text = "✔ " + correctAnswers + "/" + (currentNumberSyllab);
            bestText.text = "MEJOR: " + PlayerPrefs.GetInt(save, 0);
            inputAnswer.Select();
        }
        else
        {
            buttonAnswer.onClick.RemoveAllListeners();
            buttonAnswer.onClick.AddListener(() => AnswerLearning());
            inputAnswer.text = syllabesTest[currentNumberSyllab].romaji;
            correctText.text = "✔ "+ (currentNumberSyllab +1) + "/" + syllabesTest.Count;
            save = "";
            bestText.text = "-";
        }
    }


    void DisplaySyllab(Syllab syl, int alphabet = 1)
    {
        string syllab = "";
        if (alphabet == 1)
        {
            syllab = syl.hiragana;
        }
        else if (alphabet == 2)
        {
            syllab = syl.katakana;
        }
        else if (alphabet == 3)
        {
            if (syl.alreadyHiragana)
            {
                syllab = syl.katakana;
                foreach (Syllab syllabToChange in syllabesTest.FindAll(s => s == syl))
                {
                    syllabToChange.alreadyKatakana = true;
                }

            } else if (syl.alreadyKatakana)
            {
                syllab = syl.hiragana;
                foreach (Syllab syllabToChange in syllabesTest.FindAll(s => s == syl))
                {
                    syllabToChange.alreadyHiragana = true;
                }
            }
            else
            {
                int rand = Random.Range(0, 2);
                if (rand == 1)
                {
                    syllab = syl.hiragana;
                    foreach (Syllab syllabToChange in syllabesTest.FindAll(s => s == syl))
                    {
                        syllabToChange.alreadyHiragana = true;
                    } 
                } else
                {
                    syllab = syl.katakana;
                    foreach (Syllab syllabToChange in syllabesTest.FindAll(s => s == syl))
                    {
                        syllabToChange.alreadyKatakana = true;
                    }
                }
            }
        }
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
        inputAnswer.Select();
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
            if (PlayerPrefs.GetInt(save, 0) < correctAnswers)
            {
                PlayerPrefs.SetInt(save, correctAnswers);
            }
        }
    }

    public void DeleteAllBests()
    {
        PlayerPrefs.DeleteAll();
    }

    public void DeleteCurrentBest()
    {
        PlayerPrefs.DeleteKey(save);
    }

    public class Syllab
    {
        public string romaji;
        public string hiragana;
        public string katakana;
        public bool alreadyHiragana = false;
        public bool alreadyKatakana = false;
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