using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;

public class WB_Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GameObject tooltipObj;

    PlayerInputAction inputAction;

    bool canQuit = false;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        tooltipObj = child.gameObject;

        inputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputAction.UI.Enable();
        inputAction.UI.Click.performed += OnClickUI;
    }

    private void OnDisable()
    {
        inputAction.UI.Click.performed -= OnClickUI;
        inputAction.UI.Disable();
    }

    /// <summary>
    /// 마우스 왼쪽 버튼 클릭 감지
    /// </summary>
    /// <param name="context"></param>
    private void OnClickUI(InputAction.CallbackContext context)
    {
        if(canQuit)
        {
            tooltipObj.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipObj.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canQuit = true;
    }
}
