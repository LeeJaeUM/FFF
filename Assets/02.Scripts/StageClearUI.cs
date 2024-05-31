using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageClearUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI stageText;
    [SerializeField]
    TextMeshProUGUI clearText;
    [SerializeField]
    Button lobbyButton;

    void Start()
    {

        // 시작할 때 텍스트와 버튼을 비활성화
        stageText.gameObject.SetActive(false);
        clearText.gameObject.SetActive(false);
        lobbyButton.gameObject.SetActive(false);

        // 순차적으로 UI를 활성화하는 코루틴 시작
        StartCoroutine(ShowStageClearSequence());

        // 버튼에 메서드 연결
        lobbyButton.onClick.AddListener(LoadLobby);
    }

    IEnumerator ShowStageClearSequence()
    {
        yield return new WaitForSeconds(0.4f);
        // Stage 텍스트를 먼저 표시
        stageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);

        // Clear 텍스트 표시
        clearText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);

        // 버튼 표시
        lobbyButton.gameObject.SetActive(true);
    }

    void LoadLobby()
    {
        SceneManager.LoadScene("AfterLobby");
    }
}
