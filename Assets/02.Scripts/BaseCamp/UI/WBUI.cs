using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WBUI : MonoBehaviour
{
    [SerializeField]Workbench workbench;
    Transform uiGroup;

    private void Awake()
    {
        workbench = BaseCampManager.Instance.Workbench;
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
