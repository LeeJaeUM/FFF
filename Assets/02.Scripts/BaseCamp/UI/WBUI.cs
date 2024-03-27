using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WBUI : MonoBehaviour
{
    [SerializeField]Workbench workbench;
    Transform uiGroup;


    private void Start()
    {
        workbench = BaseCampManager.Instance.Workbench;
        if(workbench != null )
            workbench.onTrigger += Workbench_UI_Use;
        uiGroup = transform.GetChild(0);
    }

    private void Workbench_UI_Use(bool isUse)
    {
        if (isUse)
        {
            uiGroup.gameObject.SetActive(true);
        }
        else
        {
            uiGroup.gameObject.SetActive(false);
        }
    }
}
