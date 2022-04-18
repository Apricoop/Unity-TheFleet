using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Level Data")]
public class LevelData : ScriptableObject
{

    public int levelId = 0;
    public Sprite bg;
    public float spawnDelay = 2f;
    public float spawnDelayMin = .5f;
    public float spawnDelayDecreaseBy = .2f;

    [Header("Randomize Settings (OVERRIDES MANUAL LIST)")]
    [Tooltip("(Also Randomizes If Alien Are Not Set)")] [SerializeField] bool randomizeAmount = false;
    [SerializeField] Vector2Int alienAmountBetween;

    [Header("Aliens")]
    [Space(20)]
    [SerializeField] bool randomizeOrder = false;
    [SerializeField] List<EAlien> aliens;


    public List<EAlien> GetAliens()
    {
        if (!randomizeAmount && this.aliens != null && this.aliens.Count > 0)
        {
            if (randomizeOrder)
                return this.aliens.Shuffle();
            else
                return this.aliens;
        }

        List<EAlien> aliens = new List<EAlien>();
        int length = Random.Range(alienAmountBetween.x, alienAmountBetween.y);
        for (int i = 0; i < length; i++)
        {
            aliens.Add((EAlien)Random.Range(0, (int)EAlien.COUNT));
        }
        return aliens;
    }
}
