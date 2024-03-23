using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadWords : MonoBehaviour
{
    public GameObject allButtonTests;
    public TextAsset words;

    [SerializeField]
    Button[] testButtons;

    List<List<Word>> allwords = new();
    List<Word> wordsTest = new();

    AnswerWordsManager answerManager;
    SaveManager saveManager;

    List<int> activateJapaneseAnswers = new() { 1, 3 };
    List<int> haveSameListOfWords = new() { 2, 4 };
    WordsData jsonWords = new();

    public class WordsData
    {
        public Dictionary<string, Dictionary<string, List<string>>> words;
    }

    void Start()
    {
        jsonWords = LoadJson();
        LoadAllWords();
        answerManager = GetComponent<AnswerWordsManager>();
        saveManager = GetComponent<SaveManager>();
        testButtons = allButtonTests.GetComponentsInChildren<Button>();
        LoadButtons();
    }

    public WordsData LoadJson()
    {
        string jsonContent = words.text;

        return DeserializeJson(jsonContent);
    }

    private WordsData DeserializeJson(string jsonContent)
    {
        WordsData wordsData = new WordsData();
        wordsData.words = new Dictionary<string, Dictionary<string, List<string>>>();

        // Deserialize the JSON content manually
        Dictionary<string, object> jsonDict = (Dictionary<string, object>)MiniJSON.Json.Deserialize(jsonContent);
        foreach (KeyValuePair<string, object> categoryEntry in jsonDict)
        {
            Dictionary<string, object> categoryDict = (Dictionary<string, object>)categoryEntry.Value;
            Dictionary<string, List<string>> categoryWords = new Dictionary<string, List<string>>();

            foreach (KeyValuePair<string, object> wordsEntry in categoryDict)
            {
                List<object> wordsList = (List<object>)wordsEntry.Value;
                List<string> words = new List<string>();

                foreach (object wordObj in wordsList)
                {
                    words.Add(wordObj.ToString());
                }

                categoryWords.Add(wordsEntry.Key, words);
            }

            wordsData.words.Add(categoryEntry.Key, categoryWords);
        }

        return wordsData;
    }


    void LoadAllWords()
    {
        List<KeyValuePair<string, Dictionary<string, List<string>>>> words = new(jsonWords.words);

        for (int i = 0; i < words[0].Value.Count; i++)
        {
            allwords.Add(new());
            List<List<string>> valuesOfWords = new(words[i].Value.Values);
            for (int j = 0; j < valuesOfWords[i].Count; j++)
            {
                List<string> listToMakeWord = new();
                foreach (KeyValuePair<string, Dictionary<string, List<string>>> item in words)
                {
                    List<List<string>> wordsToArrange = new(item.Value.Values);
                    listToMakeWord.Add(wordsToArrange[i][j]);
                }

                allwords[i].Add(new Word(listToMakeWord[0], listToMakeWord[1], listToMakeWord[2], listToMakeWord[3]));
            }
        }
    }


    void LoadButtons()
    {
        int iAllWords = -1;
        for (int i = 0; i < testButtons.Length; i++)
        {
            int index = i;
            if (activateJapaneseAnswers.Contains(i))
            {
                testButtons[i].onClick.AddListener(() => answerManager.ActivateCheckJapaneseAnswer());
            }
            else
            {
                testButtons[i].onClick.AddListener(() => answerManager.DeactivateCheckJapaneseAnswer());
            }
            if (!haveSameListOfWords.Contains(i))
            {
                iAllWords++;
            }
            int indexWords = iAllWords;

            testButtons[i].onClick.AddListener(() => BasicWords(allwords[indexWords], index+1, testButtons[index]));
        }
    }

    public void BasicWords(List<Word> test, int saveSlot, Button buttonToRestart)
    {
        wordsTest.Clear();
        wordsTest.AddRange(test);
        IListExtensions.Shuffle(wordsTest);
        saveManager.SetCurrentSave(saveManager.GetSaveKey(saveSlot));
        answerManager.SetCurrentWordsTest(wordsTest);
        answerManager.SetButtonToRestart(buttonToRestart);
        answerManager.StartTest();
    }

    public void ShuffleTest()
    {
        IListExtensions.Shuffle(wordsTest);
        answerManager.SetCurrentWordsTest(wordsTest);
    }

}
