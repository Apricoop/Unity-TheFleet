using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Eraslank.Util;

public class SplashPage : Page, IConfigurablePage
{
    [SerializeField] Image logoImage;

    public void ConfigurePage()
    {
        logoImage.material.SetFloat("_NoiseScale", 0);
        logoImage.material.SetFloat("_Alpha", 0);

        DOVirtual.Float(0, 1, .5f, (a) => logoImage.material.SetFloat("_Alpha", a))
        .SetDelay(.5f);

        DOVirtual.Float(0, 100, .5f, (a) => logoImage.material.SetFloat("_NoiseScale", a))
        .SetLoops(2,LoopType.Yoyo)
        .SetDelay(1.5f)
        .OnComplete(()=> 
        {
            this.SuperInvoke(() =>
            {
                PageManager.Instance.ChangePage(EPageName.Main);
            }, 1.5f);
        });
        
    }
}
