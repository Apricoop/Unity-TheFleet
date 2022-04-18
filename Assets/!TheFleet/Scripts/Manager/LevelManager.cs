using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Burki.Util;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class LevelManager : MonoBehaviourSingleton<LevelManager>
{
    public LevelData levelData;

    [SerializeField] Vector2 alienSpawnXRange;
    private const float alienSpawnY = 7.5f;

    [SerializeField] Alien alienPrefab;
    PoolManager<Alien> alienPool;

    List<EAlien> alienSpawnList;
    private int score;

    [SerializeField] PowerUpPool powerUpList;
    [SerializeField] PowerUp powerUpPrefab;
    PoolManager<PowerUp> powerUpPool;

    public float powerUpDropChance = .05f;

    public UnityAction<int> OnScoreChanged;
    public UnityAction OnLevelFinished;

    List<AlienData> alienDatas;

    public List<Alien> activeAliens;

    [SerializeField] VolumeProfile volumeProfile;
    [SerializeField] SpriteRenderer BG;
    public void GainScore(int score, float alienY)
    {
        this.score += score * GetScoreMultiplier(alienY);
        OnScoreChanged?.Invoke(this.score);
    }

    private int GetScoreMultiplier(float alienY)
    {
        if (alienY >= 3f)
            return 3;
        if (alienY >= 0)
            return 2;
        return 1;
    }

    private void Start()
    {
        alienPool = new PoolManager<Alien>(alienPrefab.gameObject);
        powerUpPool = new PoolManager<PowerUp>(powerUpPrefab.gameObject);
        int levelId = 0;
        try
        {
            levelId = GameManager.Instance.levelId;
        }
        catch { } //Speeding Up Development
        var levels = Resources.LoadAll<LevelData>("ScriptableObjects/Levels").Where(l => l.levelId == levelId).ToList();
        alienDatas = Resources.LoadAll<AlienData>("ScriptableObjects/Aliens").ToList();
        alienSpawnList = (levelData = levels.RandomItem()).GetAliens();
        BG.sprite = levelData.bg;

        endlessMode = alienSpawnList.Count <= 0;

        SpawnAliens();
    }

    private void SpawnAliens()
    {
        StartCoroutine("SpawnAlienCoroutine");
    }

    bool endlessMode = false;
    int spawnedCount = 0;
    bool canSpawn = true;
    private IEnumerator SpawnAlienCoroutine()
    {
        while (alienSpawnList.Count > 0 || endlessMode)
        {
            while (!canSpawn)
                yield return null;
            var alien = alienPool.Get();
            alien.transform.position = new Vector3(Random.Range(alienSpawnXRange.x, alienSpawnXRange.y), alienSpawnY);
            EAlien alienTypeToSpawn = (EAlien)Random.Range(0, EAlien.COUNT.Int());
            if (!endlessMode)
            {
                alienTypeToSpawn = alienSpawnList.First();
                alienSpawnList.RemoveAt(0);
            }
            alien.alienData = alienDatas.FindAll(a => a.alienType == alienTypeToSpawn).RandomItem();

            alien.DePool(() =>
            {
                alien.onDie = () =>
                {
                    activeAliens.Remove(alien);
                    CheckLevelEnd();
                };

            });
            activeAliens.Add(alien);
            var secToWait = levelData.spawnDelay - (levelData.spawnDelayDecreaseBy * spawnedCount);
            secToWait = Mathf.Clamp(secToWait, levelData.spawnDelayMin, secToWait);
            yield return new WaitForSeconds(secToWait);
            spawnedCount++;
        }
    }

    public void StopAliens()
    {
        canSpawn = false;
        NukeEffect();
        foreach (var p in powerUpPool.GetAllActive())
        {
            p.Stop();
        }
        foreach (var a in activeAliens)
        {
            a.Kill();
        }
        this.SuperInvoke(() =>
        {
            canSpawn = true;
        }, 2f);
    }

    private void NukeEffect()
    {
        volumeProfile.TryGet(out ChromaticAberration cA);
        volumeProfile.TryGet(out Bloom bloom);
        volumeProfile.TryGet(out LensDistortion lD);
        volumeProfile.TryGet(out Vignette vignette);

        PlayerController.Instance.canMove = false;

        DOVirtual.Float(0, 1, .75f, a =>
        {
            cA.intensity.value = a;
        });
        DOVirtual.Float(0, 20, .75f, a =>
        {
            bloom.intensity.value = a;
        });
        DOVirtual.Float(0, .5f, .75f, a =>
        {
            lD.intensity.value = a;
        });
        DOVirtual.Float(0, .25f, .75f, a =>
        {
            vignette.intensity.value = a;
        });
        Invoke("RevertNukeEffect", 1.5f);
        foreach (var p in powerUpPool.GetAllActive())
        {
            p.FallDown();
        }
    }

    private void RevertNukeEffect()
    {

        CameraController.Instance.Shake(EZCameraShake.CameraShakePresets.Explosion);
        PlayerController.Instance.canMove = true;
        volumeProfile.TryGet(out ChromaticAberration cA);
        volumeProfile.TryGet(out Bloom bloom);
        volumeProfile.TryGet(out LensDistortion lD);
        volumeProfile.TryGet(out Vignette vignette);

        DOVirtual.Float(1, 0, .25f, a =>
        {
            cA.intensity.value = a;
        });
        DOVirtual.Float(20, 0, .25f, a =>
        {
            bloom.intensity.value = a;
        });
        DOVirtual.Float(.5f, 0, .25f, a =>
        {
            lD.intensity.value = a;
        });
        DOVirtual.Float(.25f, 0, .25f, a =>
        {
            vignette.intensity.value = a;
        });
        foreach (var p in powerUpPool.GetAllActive())
        {
            p.FallDown();
        }
    }

    private void CheckLevelEnd()
    {
        if (endlessMode)
            return;
        if (activeAliens.Count <= 0 && alienSpawnList.Count <= 0)
            OnLevelFinished?.Invoke();
    }

    public void TryDropPowerUp(Vector3 pos)
    {
        if (Random.Range(0f, 1f) > powerUpDropChance)
            return;
        var data = powerUpList.GetPowerUp();
        var p = powerUpPool.Get();
        p.transform.position = pos;
        p.powerUpData = data;
        p.DePool();
    }
}
