using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage1Clear : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어가 트리거에 들어갔는지 확인
        {
            Clear();
        }
    }

    private void Clear()
    {
        // StageDataSave 스크립트 인스턴스를 찾아 ClearStage 호출
        StageDataSave stageDataSave = FindObjectOfType<StageDataSave>();
        if (stageDataSave != null)
        {
            stageDataSave.ClearStage(1);
        }

        // 스테이지 클리어 후 로비로 이동
        SceneManager.LoadScene("Lobby");
    }
}
