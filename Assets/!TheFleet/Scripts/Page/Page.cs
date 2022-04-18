using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public abstract class Page : MonoBehaviour, IHideablePage
{
	[SerializeField] CanvasGroup cG;
	Tween alphaTween;
	public EPageName pageName = EPageName.NONE;

    private void OnValidate()
    {
		if (!cG)
			cG = GetComponent<CanvasGroup>();
    }

    private void Awake()
    {
		OnValidate();
    }


    private void Start() => cG = GetComponent<CanvasGroup>();

	public bool IsHidden { get; set; }

	public void Hide(float duration = .2f)
	{
		alphaTween?.Kill();

		alphaTween = cG.DOFade(0,duration)
		.OnComplete(() =>
		{
			gameObject.SetActive(IsHidden = false);
		})
		.SetEase(Ease.OutQuad)
		.Play();
	}
	public void Show(float duration = .2f)
	{
		alphaTween?.Kill();
		gameObject.SetActive(IsHidden = true);

		alphaTween = cG.DOFade(1, duration)
		.SetEase(Ease.InQuad)
		.Play();
	}

	public void SetAlpha(float amount)
	{
		if (cG == null)
			cG = GetComponent<CanvasGroup>();
		cG.alpha = amount;
	}
}