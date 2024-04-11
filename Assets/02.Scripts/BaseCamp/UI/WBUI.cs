using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WBUI : MonoBehaviour
{
    [SerializeField]Workbench workbench;
    Transform uiGroup;

    [SerializeField] BlockSpwaner blockSpwaner;


    private void Start()
    {
        workbench = BaseCampManager.Instance.Workbench;
        if(workbench != null )
            workbench.onTrigger += Workbench_UI_Use;
        uiGroup = transform.GetChild(0);

        blockSpwaner = BaseCampManager.instance.BlockSpwaner;
    }

    private void Workbench_UI_Use(bool isUse)
    {
        if (isUse)
        {
            blockSpwaner.ProhibitSpawn(isUse);
            uiGroup.gameObject.SetActive(isUse);
        }
        else
        {
            blockSpwaner.ProhibitSpawn(isUse);
            uiGroup.gameObject.SetActive(isUse);
        }
    }
}
