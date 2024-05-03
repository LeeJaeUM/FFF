using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TipsUI : MonoBehaviour
{
    GameObject PressEGroup;

    private bool isUIActive = false;
    private bool canUse_InteractObj = false;

    public bool CanUse_InteractObj
    {
        get => canUse_InteractObj;
        set
        {
            if (canUse_InteractObj != value)
            {
                canUse_InteractObj = value;
                PressEGroup.gameObject.SetActive(value); // E버튼 안내 ui 비/활성화

                // 트리거 밖으로 나가서 flase가 되면 자동으로 종료
                if (canUse_InteractObj == false)
                {
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

}
