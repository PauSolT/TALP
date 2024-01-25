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

    string currentSyllab = "";
    

    TouchScreenKeyboard touchScreenKeyboard;
    SaveManager saveManager;

    // Start is called before the first frame update
    void Start()
    {
        LoadJson();
        InitPureSyllabes();
        InitImpureSyllabes();
        InitDiphthongSyllabes();
        touchScreenKeyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        saveManager = GetComponent<SaveManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || (touchScreenKeyboard != null && touchScreenKeyboard.status == TouchScreenKeyboard.Status.Done))
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
            syllab.japanese = hiragana.pure[i];
            syllab.type = 1;
            syllabesPure.Add(syllab);
            Syllab syllab2 = new();
            syllab2.romaji = romaji.pure[i];
            syllab2.japanese = katakana.pure[i];
            syllab2.type = 2;
            syllabesPure.Add(syllab2);
        }
    }

    void InitImpureSyllabes()
    {
        for (int i = 0; i < romaji.impure.Length; i++)
        {
            Syllab syllab = new();
            syllab.romaji = romaji.impure[i];
            syllab.japanese = hiragana.impure[i];
            syllab.type = 1;
            syllabesImpure.Add(syllab);
            Syllab syllab2 = new();
            syllab2.romaji = romaji.impure[i];
            syllab2.japanese = katakana.impure[i];
            syllab2.type = 2;
            syllabesImpure.Add(syllab2);
        }
    }

    void InitDiphthongSyllabes()
    {
        for (int i = 0; i < romaji.diphthong.Length; i++)
        {
            Syllab syllab = new();
            syllab.romaji = romaji.diphthong[i];
            syllab.japanese = hiragana.diphthong[i];
            syllab.type = 1;
            syllabesDiphthong.Add(syllab);
            Syllab syllab2 = new();
            syllab2.romaji = romaji.diphthong[i];
            syllab2.japanese = katakana.diphthong[i];
            syllab2.type = 2;
            syllabesDiphthong.Add(syllab2);
        }
    }

    public void PureHiraganaTest(bool learning = false)
    {
        
        syllabesTest.Clear();
        syllabesTest.AddRange(syllabesPure.FindAll(s => s.type == 1));
        if (!learning)
        {
            IListExtensions.Shuffle(syllabesTest);
            saveManager.SetCurrentSave(saveManager.GetSaveKey(1));
        }

        ChangeMode(learning);
    }

    public void PureHiraganaLearning()
    {
        PureHiraganaTest(true);
    }

    public void PureKatakanaTest(bool learning = false)
    {
        syllabesTest.Clear();
        syllabesTest.AddRange(syllabesPure.FindAll(s => s.type == 2));
        if (!learning)
        {
            IListExtensions.Shuffle(syllabesTest);
            saveManager.SetCurrentSave(saveManager.GetSaveKey(5));
        }

        ChangeMode(learning);
        
    }

    public void PureKatakanaLearning()
    {
        PureKatakanaTest(true);
    }

    public void ImpureHiraganaTest(bool learning = false)
    {
        syllabesTest.Clear();
        syllabesTest.AddRange(syllabesImpure.FindAll(s => s.type == 1));
        if (!learning)
        {
            IListExtensions.Shuffle(syllabesTest);
            saveManager.SetCurrentSave(saveManager.GetSaveKey(2));
        }

        ChangeMode(learning);
    }

    public void ImpureHiraganaLearning()
    {
        ImpureHiraganaTest(true);
    }

    public void ImpureKatakanaTest(bool learning = false)
    {
        syllabesTest.Clear();
        syllabesTest.AddRange(syllabesImpure.FindAll(s => s.type == 2));
        if (!learning)
        {
            IListExtensions.Shuffle(syllabesTest);
            saveManager.SetCurrentSave(saveManager.GetSaveKey(6));
        }

        ChangeMode(learning);
        
    }

    public void ImpureKatakanaLearning()
    {
        ImpureKatakanaTest(true);
    }

    public void DiphthongHiraganaTest(bool learning = false)
    {
        syllabesTest.Clear();
        syllabesTest.AddRange(syllabesDiphthong.FindAll(s => s.type == 1));
        if (!learning)
        {
            IListExtensions.Shuffle(syllabesTest);
            saveManager.SetCurrentSave(saveManager.GetSaveKey(3));
        }

        ChangeMode(learning);
    }

    public void DiphthongHiraganaLearning()
    {
        DiphthongHiraganaTest(true);
    }

    public void DiphthongKatakanaTest(bool learning = false)
    {
        syllabesTest.Clear();
        syllabesTest.AddRange(syllabesDiphthong.FindAll(s => s.type == 2));
        if (!learning)
        {
            IListExtensions.Shuffle(syllabesTest);
            saveManager.SetCurrentSave(saveManager.GetSaveKey(7));
        }
        ChangeMode(learning);
    }

    public void DiphthongKatakanaLearning()
    {
        DiphthongKatakanaTest(true);
    }

    public void HiraganaTest()
    {
        saveManager.SetCurrentSave(saveManager.GetSaveKey(4));
        syllabesTest.Clear();
        syllabesTest.AddRange(syllabesPure.FindAll(s => s.type == 1));
        syllabesTest.AddRange(syllabesImpure.FindAll(s => s.type == 1));
        syllabesTest.AddRange(syllabesDiphthong.FindAll(s => s.type == 1));
        IListExtensions.Shuffle(syllabesTest);
        ChangeMode(false);
    }

    public void KatakanaTest()
    {
        saveManager.SetCurrentSave(saveManager.GetSaveKey(8));
        syllabesTest.Clear();
        syllabesTest.AddRange(syllabesPure.FindAll(s => s.type == 2));
        syllabesTest.AddRange(syllabesImpure.FindAll(s => s.type == 2));
        syllabesTest.AddRange(syllabesDiphthong.FindAll(s => s.type == 2));
        IListExtensions.Shuffle(syllabesTest);
        ChangeMode(false);
    }

    public void HiraganaAndKatakanaTest()
    {
        saveManager.SetCurrentSave(saveManager.GetSaveKey(9));
        syllabesTest.Clear();
        syllabesTest.AddRange(syllabesPure);
        syllabesTest.AddRange(syllabesImpure);
        syllabesTest.AddRange(syllabesDiphthong);
        IListExtensions.Shuffle(syllabesTest);
        ChangeMode(false);

    }

    void ChangeMode(bool learning)
    {
        answerText.text = "";
        inputAnswer.text = "";
        answerRomajiText.text = "";
        correctText.text = "✔";
        currentNumberSyllab = 0;
        correctAnswers = 0;
        currentSyllab = "";
        DisplaySyllab(syllabesTest[currentNumberSyllab]);

        if (!learning)
        {
            buttonAnswer.onClick.RemoveAllListeners();
            buttonAnswer.onClick.AddListener(() => CheckAnswer());
            correctText.text = "✔ " + correctAnswers + "/" + (currentNumberSyllab);
            bestText.text = "MEJOR: " + PlayerPrefs.GetInt(saveManager.GetCurrentSave(), 0);
            inputAnswer.Select();
        }
        else
        {
            buttonAnswer.onClick.RemoveAllListeners();
            buttonAnswer.onClick.AddListener(() => AnswerLearning());
            inputAnswer.text = syllabesTest[currentNumberSyllab].romaji;
            correctText.text = "✔ "+ (currentNumberSyllab +1) + "/" + syllabesTest.Count;
            saveManager.SetCurrentSave(saveManager.GetSaveKey(0));
            bestText.text = "-";
        }
    }


    void DisplaySyllab(Syllab syl)
    {
        string syllab = syl.japanese;
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
            DisplaySyllab(syllabesTest[currentNumberSyllab]);
        }
        else
        {
            questionText.text = "Fin";
            if (PlayerPrefs.GetInt(saveManager.GetCurrentSave(), 0) < correctAnswers)
            {
                saveManager.Save(correctAnswers);
            }
        }
    }

    public void DeleteAllBests()
    {
        saveManager.DeleteAllBests();
    }

    public void DeleteCurrentBest()
    {
        saveManager.DeleteCurrentBest();
    }

}
