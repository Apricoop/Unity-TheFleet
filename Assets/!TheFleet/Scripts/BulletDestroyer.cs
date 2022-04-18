using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet;
        if (bullet = collision.GetComponentInParent<Bullet>())
        {
            bullet.Pool();
        }
    }
}
