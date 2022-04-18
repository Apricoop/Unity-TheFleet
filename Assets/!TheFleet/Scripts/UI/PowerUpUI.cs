using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PowerUpUI : MonoBehaviour, IPoolable
{
    public PowerUpData powerUpData;
    [SerializeField] Image fillImage, powerUpImage;

    public bool IsAvailable { get; set; }

    Tween floatTween;
    Tween decayTween;

    public void DePool(UnityAction onDePool = null)
    {
        if (!gameObject.activeSelf)
            transform.SetAsLastSibling();
        powerUpImage.sprite = powerUpData.sprite;
        gameObject.SetActive(true);

        floatTween?.Kill();
        fillImage.rectTransform.anchoredPosition = Vector2.zero;
        var anchorPosTarget = new Vector2(0, fillImage.rectTransform.anchoredPosition.y + 100);
        
        floatTween = fillImage.rectTransform.DOAnchorPos(anchorPosTarget,.5f)
        .SetLoops(2,LoopType.Yoyo)
        .SetEase(Ease.InOutQuad);

        powerUpData.GivePower();
        decayTween?.Kill();
        fillImage.fillAmount = 0;
        fillImage.DOFillAmount(1, .5f)
        .OnComplete(()=>
        {
            decayTween = fillImage.DOFillAmount(0, 12.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Pool();
            });
        });
    }

    public void Pool(UnityAction onPool = null)
    {
        powerUpData.TakePower();
        gameObject.SetActive(false);
        onPool?.Invoke();
    }
}
