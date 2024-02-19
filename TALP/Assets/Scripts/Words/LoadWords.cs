using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWords : MonoBehaviour
{
    public TextAsset wordsJapJson;
    public TextAsset wordsJson;
    public TextAsset extrasJson;

    List<Word> basicWords = new();
    List<Word> wordsTest = new();

    AnswerWordsManager answerManager;
    SaveManager saveManager;

    struct WordsContainer
    {
        public string[] basic;
    }

    void Start()
    {
        LoadJson();
        answerManager = GetComponent<AnswerWordsManager>();
        saveManager = GetComponent<SaveManager>();
    }


    void LoadJson()
    {
        WordsContainer wordsJap = JsonUtility.FromJson<WordsContainer>(wordsJapJson.text);
        WordsContainer words = JsonUtility.FromJson<WordsContainer>(wordsJson.text);
        WordsContainer extras = JsonUtility.FromJson<WordsContainer>(extrasJson.text);

        for (int i = 0; i < wordsJap.basic.Length; i++)
        {
            basicWords.Add(new Word(wordsJap.basic[i], "", words.basic[i], extras.basic[i]));
        }
    }

    public void BasicWords()
    {
        wordsTest.Clear();
        wordsTest.AddRange(basicWords);
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
