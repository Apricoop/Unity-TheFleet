using Eraslank.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Alien : MonoBehaviour, IDamageable, IPoolable
{
    public AlienData alienData;
    public EAlien alienType;

    public bool isSeeked = false;
    public bool IsAvailable { get; set; }

    private float startingHealth;
    private float currentHealth;
    private int score;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] Animator animator;

    public bool touchedPlayer = false;
    public UnityAction onDie = null;
    private void OnValidate()
    {
        if (!spriteRenderer)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (!rb2d)
            rb2d = GetComponent<Rigidbody2D>();
        if (!animator)
            animator = GetComponentInChildren<Animator>();
    }
    private void Awake()
    {
        OnValidate();
    }

    public void Kill(float delay = 1.5f)
    {
        animator.enabled = false;
        rb2d.isKinematic = true;
        rb2d.velocity = Vector2.zero;
        this.SuperInvoke(() =>
        {
            Pool();
        }, delay);
    }

    public void DePool(UnityAction onDePool = null)
    {
        animator.enabled = true;
        IsAvailable = false;
        isSeeked = false;
        touchedPlayer = false;
        currentHealth = startingHealth = alienData.alienHealth;
        spriteRenderer.color = alienData.alienColor;
        alienType = alienData.alienType;
        score = alienData.score;

        gameObject.SetActive(true);

        rb2d.isKinematic = false;
        rb2d.velocity = -Vector2.up * .5f;

        onDePool?.Invoke();

    }

    public void Pool(UnityAction onPool = null)
    {
        if(!touchedPlayer)
        {
            LevelManager.Instance.GainScore(score, transform.position.y);
            LevelManager.Instance.TryDropPowerUp(transform.position);
        }
        gameObject.SetActive(false);
        IsAvailable = true;
        onDie?.Invoke();
        onPool?.Invoke();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        var c = spriteRenderer.color;
        c.a = 1 * (currentHealth / startingHealth);
        spriteRenderer.color = c;

        if (currentHealth <= 0)
        {
            touchedPlayer = false;
            Pool();
        }
    }
}
