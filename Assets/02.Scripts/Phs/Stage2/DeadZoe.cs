using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadZoe : MonoBehaviour
{
    public ItemCode itemCode;

    Stage1Manager manager;

    private void Awake()
    {
        manager = Stage1Manager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance.inven.UseItemCheck(itemCode))
            {
                Debug.Log($"{other.gameObject.name}은 가스마스크 덕분에 살았다.");
                manager.BottomTMPText = $"{other.name}은 가스마스크 덕분에 살았다.";
            }
            else
            {
                Debug.Log($"{other.gameObject.name}이 죽었다.");

                SceneManager.LoadScene("GameOverScene2");
                manager.BottomTMPText = $"{other.name}이 가스에 중독되어 죽었다.";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.BottomTMPText = "";
        }
    }
}
