using System;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    public static PageManager Instance;
    
    public List<string> pages = new List<string>();

    private void Awake()
    {
        Instance = this;
    }

    public void CloseAllPages()
    {
        foreach (string page in pages)
        {
            RegistryManager.Instance.GetGameObject(page).SetActive(false);
        }
    }

    public void OpenPage(string page)
    {
        CloseAllPages();
        RegistryManager.Instance.GetGameObject(page).SetActive(true);
    }
}
