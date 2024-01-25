using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    List<string> saveNames;
    [SerializeField]
    List<string> saveKeys;

    Dictionary<string, string> saves;
    string currentSave = "";

    //readonly string saveTestHiraganaPure = "saveTestHiraganaPure";
    //readonly string saveTestHiraganaImpure = "saveTestHiraganaImpure";
    //readonly string saveTestHiraganaDiphthong = "saveTestHiraganaDiphthong";
    //readonly string saveTestHiragana = "saveTestHiragana";
    //readonly string saveTestKatakanaPure = "saveTestKatakanaPure";
    //readonly string saveTestKatakanaImpure = "saveTestKatakanaImpure";
    //readonly string saveTestKatakanaDiphthong = "saveTestKatakanaDiphthong";
    //readonly string saveTestKatakana = "saveTestKatakana";
    //readonly string saveTestHiraganaKatakana = "saveTestHiraganaKatakana";

    // Start is called before the first frame update
    void Start()
    {
        saveKeys.Add("noSave");
        saves.Add("noSave", "");
        for (int i = 1; i < saveNames.Count; i++)
        {
            saveKeys.Add(saveNames[i].Remove(0, 8));
            saves.Add(saveKeys[i], saveNames[i]);
        }
    }

    public string GetSaveKey(int index)
    {
        return saveKeys[index];
    }


    public void SetCurrentSave(string keySave)
    {
        currentSave = saves[keySave];
    }

    public string GetCurrentSave()
    {
        return currentSave;
    }

    public void Save(int answers)
    {
        PlayerPrefs.SetInt(currentSave, answers);
    }

    public void DeleteCurrentBest()
    {
        PlayerPrefs.DeleteKey(currentSave);
    }

    public void DeleteAllBests()
    {
        PlayerPrefs.DeleteAll();
    }

    public enum SaveName
    {

    }
}
