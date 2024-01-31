using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerManager : MonoBehaviour
{
    public Text questionText;
    public Text answerText;
    public Text answerRomajiText;
    public Text correctText;
    public Text bestText;
    public InputField inputAnswer;
    public Button buttonAnswer;

    public List<Syllab> syllabesTest = new();

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

    public void SetCurrentSyllabTest(List<Syllab> currentList)
    {
        syllabesTest = currentList;
    }


    public void ChangeMode(bool learning)
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
