using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour
{
    public GameObject containerOfPages;
    public List<GameObject> allPages;
    public Text pagesText;

    int maxPages;
    int currentPage = 1;

    void Start()
    {
        maxPages = containerOfPages.transform.childCount;
        SetAllPages();
    }

    public void PreviousPage()
    {
        currentPage--;
        if (currentPage < 1)
        {
            currentPage = maxPages;
        }
        ShowPage();
    }

    public void NextPage()
    {
        currentPage++;
        if (currentPage > maxPages)
        {
            currentPage = 1;
        }
        ShowPage();
    }

    void ShowPage()
    {
        foreach (GameObject page in allPages)
        {
            page.SetActive(false);
        }
        allPages[currentPage - 1].SetActive(true);
        pagesText.text = "PÀGINA " + currentPage + "/" + maxPages;
    }

    void SetAllPages()
    {
        for (int i = 0; i < maxPages; i++)
        {
            allPages.Add(containerOfPages.transform.GetChild(i).gameObject);
            if(i != 0)
            {
                allPages[i].SetActive(false);
            }
        }
    }

}
