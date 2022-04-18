using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Threading;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.Networking;

public class PageManager : MonoBehaviourSingleton<PageManager>
{
    public List<Page> pages;
    Page currentPage;
    Stack<Page> visitedPages = new Stack<Page>();
    bool isDismissed;

    public void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Initialize();
    }

    private void Initialize()
    {
        pages.ForEach(p => { p.Hide(0); p.SetAlpha(0); });
    }
    // Use this for initialization
    void Start()
    {
        if(PlayerPrefs.GetInt("CameFromGameScene") == 1)
        {
            PlayerPrefs.SetInt("CameFromGameScene", 0);
            ChangePage(EPageName.Main);
        }
        else
            ChangePage(EPageName.Splash);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DismissPage();
        }
    }

    public void ChangePage(Page page)
    {
        if (currentPage != null && currentPage == page)
            return;
        if (currentPage != null)
            SetPage(currentPage, false);
        currentPage = page;
        SetPage(currentPage, true);

        Application.targetFrameRate = 60;
    }

    private bool CanChangePage(EPageName pageName, bool forcePageChange)
    {
        return !(currentPage != null && currentPage.pageName == pageName && !forcePageChange);
    }

    public void ChangePage(EPageName pageName, bool forcePageChange = false, UnityAction<Page> onFinish = null)
    {
        if (!CanChangePage(pageName,forcePageChange))
            return;
        Page page = GetPage(pageName);
        ChangePage(page);
        onFinish?.Invoke(currentPage);
    }
    public void ChangePage<T>(EPageName pageName, bool forcePageChange = false, UnityAction<T> onFinish = null)
    {
        if (!CanChangePage(pageName,forcePageChange))
            return;
        Page page = GetPage(pageName);
        ChangePage(page);
        onFinish?.Invoke(currentPage.GetComponent<T>());
    }
    void SetPage(Page page, bool showPage)
    {
        if (showPage)
        {
            IConfigurablePage iCP = page.GetComponent<IConfigurablePage>();
            if (iCP != null)
                iCP.ConfigurePage();
        }
        else
        {
            IDisposablePage iDP = page.GetComponent<IDisposablePage>();
            if (iDP != null)
                iDP.DisposePage();
        }
        if (!isDismissed && !showPage)
        {
            visitedPages.Push(page);
        }
        isDismissed = false;

        if (showPage)
            page.Show();
        else
            page.Hide();
    }

    public Page GetPage(EPageName page)
    {
        return pages.Where(p => p.pageName == page).First();
    }
    public void DismissPage()
    {
        if (visitedPages.Count < 1)
        {
            return;
        }
        isDismissed = true;
        ChangePage(visitedPages.Pop());
    }

    public void RemoveLastVisitedPage()
    {
        if (visitedPages.Count < 1)
            return;
        visitedPages.Pop();
    }
}