using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject mainButtons;
    public GameObject hiraganaButtons;
    public GameObject katakanaButtons;

    public void GoMain()
    {
        mainButtons.SetActive(true);
        hiraganaButtons.SetActive(false);
        katakanaButtons.SetActive(false);
    }

    public void GoHiragana()
    {
        mainButtons.SetActive(false);
        hiraganaButtons.SetActive(true);
    }

    public void GoKatakana()
    {
        mainButtons.SetActive(false);
        katakanaButtons.SetActive(true);
    }

    public void GoToWords()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToSyllabs()
    {
        SceneManager.LoadScene(0);
    }

}
