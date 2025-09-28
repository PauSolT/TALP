using System.Collections.Generic;
using UnityEngine;

public class SyllabManager : MonoBehaviour
{

    [SerializeField] List<SyllabSO> pureTest = new();
    [SerializeField] List<SyllabSO> impureTest = new();
    [SerializeField] List<SyllabSO> diphthongTest = new();

    List<Syllab> syllabesTest = new();

    AnswerManager answerManager;
    SaveManager saveManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        answerManager = GetComponent<AnswerManager>();
        saveManager = GetComponent<SaveManager>();
    }

    public void PureHiraganaTest(bool learning = false)
    {
        syllabesTest.Clear();
        PrepareTest(pureTest, true);
        if (!learning)
        {
            IListExtensions.Shuffle(syllabesTest);
            saveManager.SetCurrentSave(saveManager.GetSaveKey(1));
        }

        answerManager.SetCurrentSyllabTest(syllabesTest);
        answerManager.ChangeMode(learning);
    }

    public void PureKatakanaTest(bool learning = false)
    {
        syllabesTest.Clear();
        PrepareTest(pureTest, false);
        if (!learning)
        {
            IListExtensions.Shuffle(syllabesTest);
            saveManager.SetCurrentSave(saveManager.GetSaveKey(5));
        }

        answerManager.SetCurrentSyllabTest(syllabesTest);
        answerManager.ChangeMode(learning);

    }

    public void ImpureHiraganaTest(bool learning = false)
    {
        syllabesTest.Clear();
        PrepareTest(impureTest, true);
        if (!learning)
        {
            IListExtensions.Shuffle(syllabesTest);
            saveManager.SetCurrentSave(saveManager.GetSaveKey(2));
        }

        answerManager.SetCurrentSyllabTest(syllabesTest);
        answerManager.ChangeMode(learning);
    }

    public void ImpureKatakanaTest(bool learning = false)
    {
        syllabesTest.Clear();
        PrepareTest(impureTest, false);
        if (!learning)
        {
            IListExtensions.Shuffle(syllabesTest);
            saveManager.SetCurrentSave(saveManager.GetSaveKey(6));
        }

        answerManager.SetCurrentSyllabTest(syllabesTest);
        answerManager.ChangeMode(learning);

    }

    public void DiphthongHiraganaTest(bool learning = false)
    {
        syllabesTest.Clear();
        PrepareTest(diphthongTest, true);
        if (!learning)
        {
            IListExtensions.Shuffle(syllabesTest);
            saveManager.SetCurrentSave(saveManager.GetSaveKey(3));
        }

        answerManager.SetCurrentSyllabTest(syllabesTest);
        answerManager.ChangeMode(learning);
    }
    public void DiphthongKatakanaTest(bool learning = false)
    {
        syllabesTest.Clear();
        PrepareTest(diphthongTest, false);
        if (!learning)
        {
            IListExtensions.Shuffle(syllabesTest);
            saveManager.SetCurrentSave(saveManager.GetSaveKey(7));
        }
        answerManager.SetCurrentSyllabTest(syllabesTest);
        answerManager.ChangeMode(learning);
    }

    public void HiraganaTest()
    {
        saveManager.SetCurrentSave(saveManager.GetSaveKey(4));
        syllabesTest.Clear();
        PrepareTest(pureTest, true);
        PrepareTest(impureTest, true);
        PrepareTest(diphthongTest, true);
        IListExtensions.Shuffle(syllabesTest);
        answerManager.SetCurrentSyllabTest(syllabesTest);
        answerManager.ChangeMode(false);
    }

    public void KatakanaTest()
    {
        saveManager.SetCurrentSave(saveManager.GetSaveKey(8));
        syllabesTest.Clear();
        PrepareTest(pureTest, false);
        PrepareTest(impureTest, false);
        PrepareTest(diphthongTest, false);
        IListExtensions.Shuffle(syllabesTest);
        answerManager.SetCurrentSyllabTest(syllabesTest);
        answerManager.ChangeMode(false);
    }

    public void HiraganaAndKatakanaTest()
    {
        saveManager.SetCurrentSave(saveManager.GetSaveKey(9));
        syllabesTest.Clear();
        PrepareTest(pureTest, true);
        PrepareTest(impureTest, true);
        PrepareTest(diphthongTest, true);
        PrepareTest(pureTest, false);
        PrepareTest(impureTest, false);
        PrepareTest(diphthongTest, false);
        IListExtensions.Shuffle(syllabesTest);
        answerManager.SetCurrentSyllabTest(syllabesTest);
        answerManager.ChangeMode(false);
    }

    private void PrepareTest(List<SyllabSO> test, bool isHiragana)
    {
        foreach (SyllabSO s in test)
        {
            Syllab syl = new Syllab();
            syl.romaji = s.romaji;
            syl.japanese = isHiragana ? s.hiragana : s.katakana;
            syllabesTest.Add(syl);
        }

    }

}
