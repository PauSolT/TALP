using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadWords : MonoBehaviour
{
    public TextAsset wordsJapJson;
    public TextAsset wordsJson;
    public TextAsset extrasJson;
    public TextAsset kanjiJson;

    public GameObject allButtonTests;

    [SerializeField]
    Button[] testButtons;

    List<Word> basicWords = new();
    List<Word> lesson3Numbers = new();
    List<Word> wordsTest = new();

    AnswerWordsManager answerManager;
    SaveManager saveManager;

    struct WordsContainer
    {
        public string[] basic;
        public string[] lesson3Numbers;
    }

    void Start()
    {
        LoadJson();
        answerManager = GetComponent<AnswerWordsManager>();
        saveManager = GetComponent<SaveManager>();
        testButtons = allButtonTests.GetComponentsInChildren<Button>();
        LoadButtons();
    }


    void LoadJson()
    {
        WordsContainer wordsJap = JsonUtility.FromJson<WordsContainer>(wordsJapJson.text);
        WordsContainer words = JsonUtility.FromJson<WordsContainer>(wordsJson.text);
        WordsContainer extras = JsonUtility.FromJson<WordsContainer>(extrasJson.text);
        WordsContainer kanji = JsonUtility.FromJson<WordsContainer>(kanjiJson.text);

        for (int i = 0; i < wordsJap.basic.Length; i++)
        {
            basicWords.Add(new Word(wordsJap.basic[i], kanji.basic[i], words.basic[i], extras.basic[i]));
        }
        for (int i = 0; i < wordsJap.lesson3Numbers.Length; i++)
        {
            lesson3Numbers.Add(new Word(wordsJap.lesson3Numbers[i], kanji.lesson3Numbers[i], words.lesson3Numbers[i], extras.lesson3Numbers[i]));
        }
    }

    void LoadButtons()
    {
        testButtons[0].onClick.AddListener(() => BasicWords(basicWords));
        testButtons[1].onClick.AddListener(() => BasicWords(lesson3Numbers));
    }

    public void BasicWords(List<Word> test)
    {
        wordsTest.Clear();
        wordsTest.AddRange(test);
        IListExtensions.Shuffle(wordsTest);
        saveManager.SetCurrentSave(saveManager.GetSaveKey(1));
        answerManager.SetCurrentWordsTest(wordsTest);
        answerManager.StartTest();
    }

    public void ShuffleTest()
    {
        IListExtensions.Shuffle(wordsTest);
        answerManager.SetCurrentWordsTest(wordsTest);
    }

}
