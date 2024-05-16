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

    // 1단계 게임 오버시에
    public void Restart1()
    {
        SceneManager.LoadScene("Stage1");
    }

    // 2단계 게임 오버시에
    public void Restart2()
    {
        SceneManager.LoadScene("Stage2");
    }

    // 3단계 게임 오버시에
    public void Restart3()
    {
        SceneManager.LoadScene("Stage3");
    }

    public void Lobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
