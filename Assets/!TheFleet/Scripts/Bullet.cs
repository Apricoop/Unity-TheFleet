using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Eraslank.Util;

public class Bullet : MonoBehaviour, IPoolable
{
    public float damage;
    public bool piercing;
    public bool seeker;

    [SerializeField] Rigidbody2D rb2d;
    public bool IsAvailable { get; set; }

    private HashSet<IDamageable> alreadyHit = new HashSet<IDamageable>();

    Coroutine poolCor;

    LevelManager lM;

    private void OnValidate()
    {
        if (!rb2d)
            rb2d = GetComponent<Rigidbody2D>();
    }
    private void Awake()
    {
        OnValidate();
    }

    void FindNewTarget()
    {
        if (!lM)
            lM = LevelManager.Instance;
        var aliens = lM.activeAliens.FindAll(a => !a.isSeeked);
        target = FindClosest(aliens);
        if (target)
            target.isSeeked = true;
    }

    Alien FindClosest(List<Alien> aliens)
    {
        float minScore = float.MaxValue;
        Alien a = null;

        foreach (var item in aliens)
        {
            var dist = Vector2.Distance(rb2d.position, item.transform.position);
            if (dist < minScore)
            {
                minScore = dist;
                a = item;
            }
        }
        return a;
    }

    Alien target;
    private void FixedUpdate()
    {
        if (!seeker)
            return;
        if (!target)
        {
            rb2d.angularVelocity = 0;
            FindNewTarget();
            return;
        }
        var dir = (Vector2)target.transform.position - rb2d.position;
        dir.Normalize();

        var rotateAmount = Vector3.Cross(dir, transform.up).z;

        rb2d.angularVelocity = -rotateAmount * 500f;
        rb2d.velocity = transform.up * 5f;
    }

    public void Pool(UnityAction onPool = null)
    {
        gameObject.SetActive(false);
        IsAvailable = true;
        if (target)
            target.isSeeked = false;
        onPool?.Invoke();
    }
    public void DePool(UnityAction onDePool = null)
    {
        IsAvailable = false;
        transform.rotation = Quaternion.identity;
        gameObject.SetActive(true);
        alreadyHit.Clear();
        onDePool?.Invoke();
        if (seeker)
            FindNewTarget();
    }
    public void Shoot()
    {
        var angle = transform.eulerAngles.z;
        if (angle > 180)
            angle -= 360;
        rb2d.velocity = Vector3.up * 5f + Vector3.right * -angle *.075f; 
        poolCor = this.SuperInvoke(() =>
        {
            Pool();
        }, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable iDamageable;
        if ((iDamageable = collision.GetComponentInParent<IDamageable>()) != null
            && !alreadyHit.Contains(iDamageable))
        {
            alreadyHit.Add(iDamageable);
            iDamageable.TakeDamage(damage);
            if (!piercing)
            {
                StopCoroutine(poolCor);
                Pool();
            }
            piercing = false;
        }
    }
}
