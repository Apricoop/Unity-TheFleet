using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUp : MonoBehaviour, IPoolable
{
    public PowerUpData powerUpData;
    public EPowerUp powerUpType;

    public bool IsAvailable { get; set; }

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Rigidbody2D rb2d;

    private void OnValidate()
    {
        if (!spriteRenderer)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (!rb2d)
            rb2d = GetComponent<Rigidbody2D>();
    }
    private void Awake()
    {
        OnValidate();
    }

    public void Stop()
    {
        rb2d.velocity = Vector2.zero;
    }
    public void FallDown()
    {
        rb2d.velocity = -Vector2.up * 1.5f;
    }

    public void DePool(UnityAction onDePool = null)
    {
        IsAvailable = false;
        spriteRenderer.sprite = powerUpData.sprite;
        powerUpType = powerUpData.powerUpType;

        gameObject.SetActive(true);

        FallDown();

        onDePool?.Invoke();

    }

    public void Pool(UnityAction onPool = null)
    {
        gameObject.SetActive(false);
        IsAvailable = true;
        onPool?.Invoke();
    }
}
