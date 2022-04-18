using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Alien Data", menuName = "Alien Data")]
public class AlienData : ScriptableObject
{
    public EAlien alienType;
    public Color alienColor;
    public float alienHealth;
    public int score;
}
