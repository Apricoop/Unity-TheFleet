using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eraslank.Util;
using UnityEngine.Events;

public class PlayerController : MonoBehaviourSingleton<PlayerController>
{
    [Header("Common Settings")]
    public bool canMove = true;
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform shootFrom;
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] float xRange;



    [Header("Powers")]
    public float damage = 1;
    public bool piercing = false;
    public bool spreadShot = false;
    public bool seekerShot = false;


    [Header("Desktop Settings")]
    [SerializeField] float movementSpeed = 5f;


    [Header("Mobile Settings")]
    [SerializeField] float dragSensitivity = .04f;
    [SerializeField] float tapTimeThreshold = 0.2f;

    private bool touchDidMove;
    private float timeTouchBegan;

    PoolManager<Bullet> bulletPool;

    public UnityAction OnDie = null;
    public UnityAction<PowerUp> OnPowerUpCollect = null;

    private void OnValidate()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void Awake()
    {
        OnValidate();
        bulletPool = new PoolManager<Bullet>(bulletPrefab.gameObject);
    }

    private IEnumerator Start()
    {
        while (LevelManager.Instance == null)
            yield return null;
        LevelManager.Instance.OnLevelFinished += () =>
        {
            canMove = false;
        };
    }

    void Update()
    {
        if (!canMove)
        {
            rb2d.velocity = Vector2.zero;
            return;
        }
        switch (SystemInfo.deviceType)
        {
            case DeviceType.Desktop:
                HandleDesktop();
                break;
            default:
                HandleMobile();
                break;
        }
    }

    void HandleDesktop()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        rb2d.velocity = Vector2.right * horizontal * Time.deltaTime * movementSpeed;
        StayInBounds();

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Shoot();
        }
    }

    void HandleMobile()
    {
        if (Input.touchCount <= 0)
            return;
        var touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            timeTouchBegan = Time.time;
            touchDidMove = false;
        }
        if (touch.phase == TouchPhase.Moved)
        {
            var movement = Input.GetTouch(0).deltaPosition.x * dragSensitivity * Time.deltaTime;
            var pos = transform.position;
            pos += Vector3.right * movement;
            transform.position = pos;
            touchDidMove = true;
            StayInBounds();
        }
        if (touch.phase == TouchPhase.Ended)
        {
            float tapTime = Time.time - timeTouchBegan;
            if (tapTime <= tapTimeThreshold && !touchDidMove)
            {
                Shoot();
            }
        }
    }

    void StayInBounds()
    {
        if (Mathf.Abs(transform.position.x) > xRange)
            transform.position = new Vector3(Mathf.Sign(transform.position.x) * xRange, transform.position.y);
    }
    void Shoot()
    {
        for (int i = (spreadShot ? -1 : 0); i <= (spreadShot ? 1 : 0); i++)
        {
            var b = bulletPool.Get();
            b.DePool(() =>
            {
                b.transform.position = shootFrom.position;
                b.transform.rotation = Quaternion.Euler(0, 0, 15 * i);
                b.damage = damage;
                b.seeker = seekerShot;
                b.piercing = piercing;
                b.Shoot();
            });
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var powerUp = collision.GetComponentInParent<PowerUp>();
        if(powerUp)
        {
            powerUp.Pool();
            OnPowerUpCollect?.Invoke(powerUp);
        }
    }

    public void Die()
    {
        canMove = false;
        OnDie?.Invoke();
    }
}
