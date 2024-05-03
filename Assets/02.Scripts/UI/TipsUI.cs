using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TipsUI : MonoBehaviour
{
    GameObject PressEGroup;

    private bool isUIActive = false;
    private bool canUse_InteracObj = false;

    public bool CanUse_Workbench
    {
        get => canUse_InteracObj;
        set
        {
            if (canUse_InteracObj != value)
            {
                canUse_InteracObj = value;
                PressEGroup.gameObject.SetActive(value); // E버튼 안내 ui 비/활성화

                // 트리거 밖으로 나가서 flase가 되면 자동으로 종료
                if (canUse_InteracObj == false)
                {
                    InteracObj_UI_Use(canUse_InteracObj);
                    isUIActive = false;
                }
            }
        }
    }

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        PressEGroup = child.gameObject;
    }


    /// <summary>
    /// 트리거 범위 내부일때 e를 눌러서 UI 활성화 가능함
    /// </summary>
    /// <param name="_"></param>
    private void OnPressE_UI(InputAction.CallbackContext _)
    {
        if (canUse_InteracObj)
        {
            isUIActive = !isUIActive;
            InteracObj_UI_Use(isUIActive);
        }
    }
    private void InteracObj_UI_Use(bool isUse)
    {
        
    }
}
