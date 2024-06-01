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
        // 스테이지 클리어시 클리어 씬으로 이동
        SceneManager.LoadScene("StageClear");
    }
}
