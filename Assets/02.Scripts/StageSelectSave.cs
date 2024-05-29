using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectSave : MonoBehaviour
{
    [SerializeField]
    private Button stage2Button;
    [SerializeField]
    private Button stage3Button;

    private void Start()
    {
        stage2Button.interactable = false; // 첫 시작 시 스테이지 2 버튼 비활성화
        stage3Button.interactable = false; // 첫 시작 시 스테이지 3 버튼 비활성화
    }
}
