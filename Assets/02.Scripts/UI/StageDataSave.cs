using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageDataSave : MonoBehaviour
{
    public Button stage1Button;
    public Button stage2Button;
    public Button stage3Button;

    void Awake()
    {
        RefreshStageButton();
        LoadStageStatus();
    }

    private void Start()
    {
        RefreshStageButton();
        LoadStageStatus();
    }

    public void ClearStage(int stageNumber)
    {
        RefreshStageButton();
        PlayerPrefs.SetInt($"{stageNumber}", 1);
        PlayerPrefs.Save();
        LoadStageStatus();
    }

    void LoadStageStatus()
    {
        RefreshStageButton();

        // 1스테이지는 기본적으로 활성화
        stage1Button.gameObject.SetActive(true);

        // 2스테이지 활성화 여부
        if (PlayerPrefs.GetInt("1", 0) == 1)
        {
            stage2Button.gameObject.SetActive(true);
        }

        // 3스테이지 활성화 여부
        if (PlayerPrefs.GetInt("2", 0) == 1)
        {
            stage3Button.gameObject.SetActive(true);
        }
    }

    void RefreshStageButton()
    {
        stage1Button.gameObject.SetActive(false);
        stage2Button.gameObject.SetActive(false);
        stage3Button.gameObject.SetActive(false);
    }
}
