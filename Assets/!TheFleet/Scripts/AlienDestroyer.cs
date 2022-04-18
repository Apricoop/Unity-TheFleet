using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Alien alien;
        if (alien = collision.GetComponentInParent<Alien>())
        {
            alien.touchedPlayer = true;
            alien.Pool();
            PlayerController.Instance.Die();
        }
    }
}
