using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Power Up Pool", menuName = "Power Up Pool")]
public class PowerUpPool : ScriptableObject
{
    public List<PowerUpPoolItem> powerUps;

    private WeightedRandomBag<PowerUpData> bag;

    public PowerUpData GetPowerUp()
    {
        bag = new WeightedRandomBag<PowerUpData>();
        foreach(var e in powerUps)
        {
            bag.AddEntry(e.powerUp, e.weight);
        }
        return bag.GetRandom();
    }
}
[System.Serializable]
public struct PowerUpPoolItem
{
    public PowerUpData powerUp;
    public int weight;
}