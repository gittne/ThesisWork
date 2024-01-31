using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PagesHandler : MonoBehaviour
{
    [SerializeField] GameObject[] pages;
    int currentIndex;

    public void NextPage()
    {
        currentIndex++;

        if(currentIndex == pages.Length)
        {
            currentIndex = 0;
        }

        ReloadPage();
    }

    public void PreviosuPage()
    {
        currentIndex--;

        if (currentIndex < 0)
        {
            currentIndex = pages.Length - 1;
        }

        ReloadPage();
    }

    public void ReloadPage()
    {
        foreach(GameObject page in pages)
            page.SetActive(false);

        pages[currentIndex].SetActive(true);
    }

    public void ResetPages()
    {
        currentIndex = 0;
    }
}
