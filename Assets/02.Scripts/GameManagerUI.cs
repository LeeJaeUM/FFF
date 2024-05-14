using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerUI : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseUI; // 게임을 일시정지 했을 때 나타나는 UI
    [SerializeField]
    private GameObject retryCaution; // 리트라이 버트을 누를 때 뜨는 주의 오브젝트
    [SerializeField]
    private GameObject lobbyCaution; // 로비 버튼을 누를 때 뜨는 주의 오브젝트
    private bool isGameStop = true; // 게임이 멈췄는지에 대한 여부

    private void Update()
    {
        // ESC를 눌렀을 때 일시정지
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isGameStop)
            {
                pauseUI.SetActive(true); // pauseUI 활성화
                Time.timeScale = 0f;
            }

            else if(!isGameStop)
            {
                pauseUI.SetActive(false); // pauseUI 제거
                Time.timeScale = 1f;
            }

            isGameStop = !isGameStop;
        }
    }

    // Retry버튼을 눌렀을 때
    public void RetryButton()
    {
        retryCaution.SetActive(true); // 리트라이를 한번 더 확인하는 UI생성
    }

    // 리트라이를 한번 확인하는 UI에서 다시 한번 리트라이를 눌렀을 때
    public void RetryButton1()
    {
        SceneManager.LoadScene("Stage3"); // 스테이지 재시작
        isGameStop = true;
        Time.timeScale = 1f;
    }

    // 리트라이를 한번 확인하는 UI에서 No버튼을 눌렀을 때
    public void RetryButton2()
    {
        retryCaution.SetActive(false);
    }

    // Lobby버튼을 눌렀을 때
    public void LobbyButton()
    {
        lobbyCaution.SetActive(true);
    }

    // Lobby로 가는 것을 한번 더 확인하는 버튼을 눌렀을 때
    public void LobbyButton1()
    {
        SceneManager.LoadScene("Lobby");
        isGameStop = true;
        Time.timeScale = 1f;
    }

    // Lobby로 가는 것을 한번 더 확인하는 UI에서 No를 눌렀을 때
    public void LobbyButton2()
    {
        lobbyCaution.SetActive(false);
    }
}
