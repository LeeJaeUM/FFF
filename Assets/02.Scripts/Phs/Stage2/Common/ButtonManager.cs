using KeypadSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public Stage2_Button[] buttons;
    public GameObject[] Battery;

    public Material successMaterial;
    public Material failMaterial;

    public Action onWarning;
    public Action AllAccess;

    bool isSuccess;

    Stage2_Keypad _keypad;

    private void Awake()
    {
        _keypad = FindObjectOfType<Stage2_Keypad>();
    }

    private void Start()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].ButtonID = i;
            buttons[i].onTrigger += OnTrigger;
        }

        _keypad.onSuccess += OnKeypadAnwser;
    }

    private void OnKeypadAnwser(bool isSuccess)
    {
        Stage1Manager.Instance.BottomTMPText = "리포트가 작동한 것 같다.";
        this.isSuccess = isSuccess;
    }

    private void OnTrigger(int id)
    {
        Battery[id].GetComponent<MeshRenderer>().material = successMaterial;
        if (!isSuccess)
        {
            Stage1Manager.Instance.BottomTMPText = "경보";
            onWarning?.Invoke();
        }
        Refresh();
    }

    /// <summary>
    /// 버튼 동작시 작동된 버튼을 확인하는 함수
    /// </summary>
    private void Refresh()
    {
        foreach(var button in buttons)
        {
            if (!button.canUse)
            {
                return;
            }
        }

        AllAccess?.Invoke();
    }

#if UNITY_EDITOR
    public void AllButtonTrigger()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            OnTrigger(i);
        }
    }
#endif
}
