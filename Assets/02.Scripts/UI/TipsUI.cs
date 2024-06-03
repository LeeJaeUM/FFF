using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TipsUI : MonoBehaviour
{
    GameObject PressEGroup;
    GameObject bookImg;
    Image bookHint;
    GameObject woodSign;

    //private bool isUIActive = false;
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
                bookImg.gameObject.SetActive(value);
                bookHint.enabled = false;
            }
        }
    }

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        PressEGroup = child.gameObject;
        child = transform.GetChild(1);
        bookImg = child.gameObject;
        bookHint = bookImg.GetComponentInChildren<Image>(true);
        child = transform.GetChild(2);
        woodSign = child.gameObject;
    }

}
