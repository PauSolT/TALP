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
    public InputField inputAnswer;
    public Button buttonAnswer;

    public List<Word> wordsTest = new();

    int currentNumberSyllab = 0;
    int correctAnswers = 0;

    string currentSyllab = "";

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


    public void ChangeMode(bool learning)
    {
        answerText.text = "";
        inputAnswer.text = "";
        answerOtherText.text = "";
        correctText.text = "✔";
        currentNumberSyllab = 0;
        correctAnswers = 0;
        currentSyllab = "";
        DisplaySyllab(wordsTest[currentNumberSyllab]);

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
            inputAnswer.text = wordsTest[currentNumberSyllab].word;
            correctText.text = "✔ " + (currentNumberSyllab + 1) + "/" + wordsTest.Count;
            saveManager.SetCurrentSave(saveManager.GetSaveKey(0));
            bestText.text = "-";
        }
    }


    void DisplaySyllab(Word syl)
    {
        string syllab = syl.japanese;
        questionText.text = syllab;
        currentSyllab = syllab;
    }

    public void AnswerLearning()
    {
        NextSyllab();
        inputAnswer.text = wordsTest[currentNumberSyllab].word;
        correctText.text = "✔ " + (currentNumberSyllab + 1) + "/" + wordsTest.Count;
        answerOtherText.text = wordsTest[currentNumberSyllab].word;
    }

    public void CheckAnswer()
    {
        inputAnswer.Select();
        string answer = inputAnswer.text.Trim();
        string syl = currentSyllab;
        inputAnswer.text = "";
        answerText.text = syl;
        answerOtherText.text = wordsTest[currentNumberSyllab].word;
        if (string.Compare(answer, wordsTest[currentNumberSyllab].word) == 0)
        {
            answerText.color = Color.green;
            correctAnswers++;
            correctText.text = "✔ " + correctAnswers + "/" + (currentNumberSyllab + 1);
        }
        else
        {
            answerText.color = Color.red;
            correctText.text = "✔ " + correctAnswers + "/" + (currentNumberSyllab + 1);
        }
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

}
