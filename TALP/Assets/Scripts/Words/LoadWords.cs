using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadWords : MonoBehaviour
{
    public TextAsset wordsJapJson;
    public TextAsset wordsJson;
    public TextAsset extrasJson;
    public TextAsset kanjiJson;

    public GameObject allButtonTests;

    [SerializeField]
    Button[] testButtons;

    List<List<Word>> allwords = new();
    List<Word> wordsTest = new();

    AnswerWordsManager answerManager;
    SaveManager saveManager;

    List<int> activateJapaneseAnswers = new() { 1 };
    List<int> haveSameListOfWords = new() { 2 };
    WordsData jsonWords = new();

    string filePath = "Assets/JSONs/words.json";
    public class WordsData
    {
        public Dictionary<string, Dictionary<string, List<string>>> words;
    }

    void Start()
    {
        jsonWords = LoadJson(filePath);
        LoadAllWords();
        answerManager = GetComponent<AnswerWordsManager>();
        saveManager = GetComponent<SaveManager>();
        testButtons = allButtonTests.GetComponentsInChildren<Button>();
        LoadButtons();
    }

    public WordsData LoadJson(string jsonFilePath)
    {
        // Check if the file exists
        if (!File.Exists(jsonFilePath))
        {
            Debug.LogError("JSON file does not exist: " + jsonFilePath);
            return null;
        }

        // Read the JSON file
        string jsonContent = File.ReadAllText(jsonFilePath);

        // Deserialize the JSON into a custom class
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
        for (int i = 0; i < testButtons.Length; i++)
        {
            if (activateJapaneseAnswers.Contains(i))
            {
                testButtons[i].onClick.AddListener(() => answerManager.ActivateCheckJapaneseAnswer());
            }
            else
            {
                testButtons[i].onClick.AddListener(() => answerManager.DeactivateCheckJapaneseAnswer());
            }
        }

        int iAllWords = -1;
        for (int i = 0; i < testButtons.Length; i++)
        {
            if (!haveSameListOfWords.Contains(i))
            {
                iAllWords++;
            }
            int index = iAllWords;

            testButtons[i].onClick.AddListener(() => BasicWords(allwords[index]));
        }
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
