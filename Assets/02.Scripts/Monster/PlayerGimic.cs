using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGimic : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Stage 1 몬스터에게 닿았을 경우 게임 오버
        if(other.gameObject.CompareTag("MONSTER"))
        {
            SceneManager.LoadScene("GameOverScene1");
        }
    }
}   
