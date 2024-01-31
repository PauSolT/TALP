using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSyllabs : MonoBehaviour
{
    public TextAsset romajiJson;
    public TextAsset hiraganaJson;
    public TextAsset katakanaJson;

    Alphabet romaji = new();
    Alphabet hiragana = new();
    Alphabet katakana = new();

    List<Syllab> syllabesPure = new();
    List<Syllab> syllabesImpure = new();
    List<Syllab> syllabesDiphthong = new();
    List<Syllab> syllabesTest = new();

    AnswerManager answerManager;
    SaveManager saveManager;

    // Start is called before the first frame update
    void Start()
    {
        LoadJson();
        InitPureSyllabes();
        InitImpureSyllabes();
        InitDiphthongSyllabes();
        answerManager = GetComponent<AnswerManager>();
        saveManager = GetComponent<SaveManager>();
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

        answerManager.SetCurrentSyllabTest(syllabesTest);
        answerManager.ChangeMode(learning);
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

        answerManager.SetCurrentSyllabTest(syllabesTest);
        answerManager.ChangeMode(learning);

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

        answerManager.SetCurrentSyllabTest(syllabesTest);
        answerManager.ChangeMode(learning);
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

        answerManager.SetCurrentSyllabTest(syllabesTest);
        answerManager.ChangeMode(learning);

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

        answerManager.SetCurrentSyllabTest(syllabesTest);
        answerManager.ChangeMode(learning);
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
        answerManager.SetCurrentSyllabTest(syllabesTest);
        answerManager.ChangeMode(learning);
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
        answerManager.SetCurrentSyllabTest(syllabesTest);
        answerManager.ChangeMode(false);
    }

    public void KatakanaTest()
    {
        saveManager.SetCurrentSave(saveManager.GetSaveKey(8));
        syllabesTest.Clear();
        syllabesTest.AddRange(syllabesPure.FindAll(s => s.type == 2));
        syllabesTest.AddRange(syllabesImpure.FindAll(s => s.type == 2));
        syllabesTest.AddRange(syllabesDiphthong.FindAll(s => s.type == 2));
        IListExtensions.Shuffle(syllabesTest);
        answerManager.SetCurrentSyllabTest(syllabesTest);
        answerManager.ChangeMode(false);
    }

    public void HiraganaAndKatakanaTest()
    {
        saveManager.SetCurrentSave(saveManager.GetSaveKey(9));
        syllabesTest.Clear();
        syllabesTest.AddRange(syllabesPure);
        syllabesTest.AddRange(syllabesImpure);
        syllabesTest.AddRange(syllabesDiphthong);
        IListExtensions.Shuffle(syllabesTest);
        answerManager.SetCurrentSyllabTest(syllabesTest);
        answerManager.ChangeMode(false);
    }

}
