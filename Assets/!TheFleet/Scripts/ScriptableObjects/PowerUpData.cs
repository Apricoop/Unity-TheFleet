using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Power Up Data", menuName = "Power Up Data")]
public class PowerUpData : ScriptableObject
{
    public EPowerUp powerUpType;
    public Sprite sprite;

    public void GivePower()
    {
        PlayerController pc = PlayerController.Instance;
        switch (powerUpType)
        {
            case EPowerUp.Damage:
                pc.damage = 2;
                break;
            case EPowerUp.Piercing:
                pc.piercing = true;
                break;
            case EPowerUp.Spread:
                pc.spreadShot = true;
                break;
            case EPowerUp.Seeker:
                pc.seekerShot = true;
                break;
        }
    }

    public void TakePower()
    {
        PlayerController pc = PlayerController.Instance;
        switch (powerUpType)
        {
            case EPowerUp.Damage:
                pc.damage = 1;
                break;
            case EPowerUp.Piercing:
                pc.piercing = false;
                break;
            case EPowerUp.Spread:
                pc.spreadShot = false;
                break;
            case EPowerUp.Seeker:
                pc.seekerShot = false;
                break;
        }
    }
}
