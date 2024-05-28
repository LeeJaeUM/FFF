using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageClear : MonoBehaviour
{
    public int stageNumber = 1; // 클리어할 스테이지 번호

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어가 트리거에 들어갔는지 확인
        {
            ClearStage();
        }
    }

    private void ClearStage()
    {
        Lobby lobby = FindObjectOfType<Lobby>();
        if (lobby != null)
        {
            lobby.ClearStage(stageNumber);
        }

        // 스테이지 클리어 후 로비로 이동
        SceneManager.LoadScene("Lobby");
    }
}
