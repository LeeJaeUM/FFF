using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageDataSave : MonoBehaviour
{
    public Button stage1Button;
    public Button stage2Button;
    public Button stage3Button;

    void Start()
    {
        LoadStageStatus();
    }

    public void ClearStage(int stageNumber)
    {
        PlayerPrefs.SetInt("Stage" + stageNumber + "Cleared", 1);
        PlayerPrefs.Save();
    }

    void LoadStageStatus()
    {
        // 1스테이지는 기본적으로 활성화
        stage1Button.gameObject.SetActive(true);

        // 2스테이지 활성화 여부
        if (PlayerPrefs.GetInt("Stage1Cleared", 0) == 1)
        {
            stage2Button.gameObject.SetActive(true);
        }
        else
        {
            stage2Button.gameObject.SetActive(false);
        }

        // 3스테이지 활성화 여부
        if (PlayerPrefs.GetInt("Stage2Cleared", 0) == 1)
        {
            stage3Button.gameObject.SetActive(true);
        }
        else
        {
            stage3Button.gameObject.SetActive(false);
        }
    }
}
