using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] Text scoreText;

    [SerializeField] CanvasGroup gameOverUICanvasGroup;
    [SerializeField] Text finalScore;

    bool didShow = false;
    IEnumerator Start()
    {
        gameOverUICanvasGroup.gameObject.SetActive(false);
        while (LevelManager.Instance == null)
            yield return null;
        LevelManager.Instance.OnScoreChanged += (s) =>
        {
            scoreText.text = s.ToString();
        };
        LevelManager.Instance.OnLevelFinished += ShowLevelEnd;
        PlayerController.Instance.OnDie += ShowLevelEnd;
    }

    void ShowLevelEnd()
    {
        if (didShow)
            return;
        didShow = true;
        scoreText.gameObject.SetActive(false);
        gameOverUICanvasGroup.gameObject.SetActive(true);
        finalScore.text = scoreText.text;
        gameOverUICanvasGroup.DOFade(1, .5f);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    {
        PlayerPrefs.SetInt("CameFromGameScene", 1);
        SceneManager.LoadScene(0);
    }
}
