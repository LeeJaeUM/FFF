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

    public Action AllAccess;

    private void Start()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].ButtonID = i;
            buttons[i].onTrigger += OnTrigger;
        }
    }

    private void OnTrigger(int id)
    {
        Battery[id].GetComponent<MeshRenderer>().material = successMaterial;
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
