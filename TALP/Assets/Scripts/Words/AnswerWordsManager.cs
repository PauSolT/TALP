using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerWordsManager : MonoBehaviour
{
    public Text questionText;
    public Text answerText;
    public Text answerOtherText;
    public Text correctText;
    public Text bestText;
    public Text learningText;
    public Text onlyKanjiText;
    public InputField inputAnswer;
    public Button buttonAnswer;

    public List<Word> wordsTest = new();

    int currentNumberSyllab = 0;
    int correctAnswers = 0;

    string currentSyllab = "";

    bool learning = false;
    bool onlyKanji = false;
    [SerializeField]
    bool checkJapaneseAnswer;

    TouchScreenKeyboard touchScreenKeyboard;
    SaveManager saveManager;

    // Start is called before the first frame update
    void Start()
    {
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

    public void SetCurrentWordsTest(List<Word> currentList)
    {
        wordsTest = currentList;
    }

    public void StartTest()
    {
        answerText.text = "";
        inputAnswer.text = "";
        answerOtherText.text = "";
        correctText.text = "✔";
        currentNumberSyllab = 0;
        correctAnswers = 0;
        currentSyllab = "";

        if (wordsTest.Count == 0)
        {
            return;
        }

        DisplaySyllab(wordsTest[currentNumberSyllab]);

        if (!learning)
        {
            buttonAnswer.onClick.RemoveAllListeners();
            buttonAnswer.onClick.AddListener(() => CheckAnswer());
            correctText.text = "✔ " + correctAnswers + "/" + (currentNumberSyllab);
            bestText.text = "MEJOR: " + PlayerPrefs.GetInt(saveManager.GetCurrentSave(), 0);
            inputAnswer.Select();
            learningText.text = "APRENDER: NO";
        }
        else
        {
            buttonAnswer.onClick.RemoveAllListeners();
            buttonAnswer.onClick.AddListener(() => AnswerLearning());
            HandleDisplayLearning();
            saveManager.SetCurrentSave(saveManager.GetSaveKey(0));
            bestText.text = "-";
            learningText.text = "APRENDER: SI";
        }
    }

    public void ChangeMode()
    {
        learning = !learning;
        StartTest();
    }


    public void DisplaySyllab(Word syl)
    {
        string syllab = syl.japanese;
        if(!string.IsNullOrEmpty(syl.kanji) && !checkJapaneseAnswer)
        {
            syllab = syl.kanji + " (" + syl.japanese + ")";
        }
        if (!string.IsNullOrEmpty(syl.kanji) && onlyKanji ||
            !string.IsNullOrEmpty(syl.kanji) && checkJapaneseAnswer)
        {
            syllab = syl.kanji;
        }

        questionText.text = syllab;
        currentSyllab = syllab;
    }

    public void AnswerLearning()
    {
        NextSyllab();
        HandleDisplayLearning();
    }

    void HandleDisplayLearning()
    {
        if (checkJapaneseAnswer)
        {
            inputAnswer.text = wordsTest[currentNumberSyllab].japanese;
            answerOtherText.text = wordsTest[currentNumberSyllab].japanese;
        }
        else
        {
            inputAnswer.text = wordsTest[currentNumberSyllab].word;
            answerOtherText.text = wordsTest[currentNumberSyllab].word;
        }
        correctText.text = "✔ " + (currentNumberSyllab + 1) + "/" + wordsTest.Count;
    }
    public void CheckAnswer()
    {
        inputAnswer.Select();
        string answer = inputAnswer.text.Trim().ToLower();
        string syl = currentSyllab;
        inputAnswer.text = "";
        answerText.text = syl;


        if(checkJapaneseAnswer)
        {
            answerOtherText.text = wordsTest[currentNumberSyllab].japanese;
        }
        else
        {
            answerOtherText.text = wordsTest[currentNumberSyllab].word;
        }

        if (!string.IsNullOrEmpty(wordsTest[currentNumberSyllab].extra))
        {
            answerOtherText.text += "/\n" + wordsTest[currentNumberSyllab].extra;
        }

        if (string.Compare(answer, wordsTest[currentNumberSyllab].word) == 0 ||
            checkJapaneseAnswer && string.Compare(answer, wordsTest[currentNumberSyllab].japanese) == 0)
        {
            answerText.color = Color.green;
            correctAnswers++;
        }
        else
        {
            answerText.color = Color.red;
        }
        correctText.text = "✔ " + correctAnswers + "/" + (currentNumberSyllab + 1);
        NextSyllab();
    }

    void NextSyllab()
    {
        if (currentNumberSyllab < wordsTest.Count - 1)
        {
            currentNumberSyllab++;
            DisplaySyllab(wordsTest[currentNumberSyllab]);
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

    public void OnlyKanjiMode()
    {
        onlyKanji = !onlyKanji;

        if (onlyKanji)
        {
            onlyKanjiText.text = "SOLO KANJI: SI";
        } else
        {
            onlyKanjiText.text = "SOLO KANJI: NO";
        }
        DisplaySyllab(wordsTest[currentNumberSyllab]);
    }

    public void ActivateCheckJapaneseAnswer()
    {
        checkJapaneseAnswer = true;
    }

    public void DeactivateCheckJapaneseAnswer()
    {
        checkJapaneseAnswer = false;
    }
}
