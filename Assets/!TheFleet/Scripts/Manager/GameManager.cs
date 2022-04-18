using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingletonPersistent<GameManager>
{
    public int levelId;
    public void StartLevel(int levelId)
    {
        this.levelId = levelId;
        SceneManager.LoadScene(1);
    }
}
