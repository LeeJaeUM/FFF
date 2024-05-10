using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverUI; // 게임 오버 UI

    private void Update()
    {
        StartCoroutine(UIOpen());
    }

    // UI 오픈
    IEnumerator UIOpen()
    {
        yield return new WaitForSeconds(1.5f);

        gameOverUI.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Stage3");
    }

    public void Lobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
