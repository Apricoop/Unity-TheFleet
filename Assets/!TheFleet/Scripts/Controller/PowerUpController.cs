using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    [SerializeField] PowerUpUI powerUpUIPrefab;
    Dictionary<EPowerUp, PowerUpUI> powerUpUIs = new Dictionary<EPowerUp, PowerUpUI>();
    PoolManager<PowerUpUI> uiPool; 
    IEnumerator Start()
    {
        while (!PlayerController.Instance)
            yield return null;

        uiPool = new PoolManager<PowerUpUI>(powerUpUIPrefab.gameObject);
        PlayerController.Instance.OnPowerUpCollect = (p) =>
        {
            if(p.powerUpType == EPowerUp.Nuke)
            {
                Nuke();
                return;
            }
            PowerUpUI ui;
            if (powerUpUIs.ContainsKey(p.powerUpType))
                ui = powerUpUIs[p.powerUpType];
            else
                ui = powerUpUIs[p.powerUpType] = uiPool.Get(transform);
            ui.powerUpData = p.powerUpData;
            ui.DePool();
        };
    }

    private void Nuke()
    {
        LevelManager.Instance.StopAliens();
    }
}
