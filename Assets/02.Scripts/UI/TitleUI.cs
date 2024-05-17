using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    private Button[] buttons;

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();

        buttons[0].onClick.AddListener(OnStartButton);
        buttons[1].onClick.AddListener(OnOptionButton);
        buttons[2].onClick.AddListener(OnExitButton);
    }

    private void OnStartButton()
    {        
        // 현재 씬의 빌드 인덱스를 가져옵니다.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // 다음 씬의 빌드 인덱스를 계산합니다.
        int nextSceneIndex = currentSceneIndex + 1;
        // 다음 씬을 로드합니다.
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void OnOptionButton()
    {
        throw new NotImplementedException();
    }
    private void OnExitButton()
    {
        // 게임 종료
        Application.Quit();
    }
}
