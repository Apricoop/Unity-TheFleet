using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPage : Page, IConfigurablePage
{
    [SerializeField] Button level1Button, level2Button, endlessButton;

    public void ConfigurePage()
    {
        level1Button.onClick.RemoveAllListeners();
        level1Button.onClick.AddListener(() =>
        {
            GameManager.Instance.StartLevel(1);
        });

        level2Button.onClick.RemoveAllListeners();
        level2Button.onClick.AddListener(() =>
        {
            GameManager.Instance.StartLevel(2);
        });

        endlessButton.onClick.RemoveAllListeners();
        endlessButton.onClick.AddListener(() =>
        {
            GameManager.Instance.StartLevel(0);
        });
    }
}
