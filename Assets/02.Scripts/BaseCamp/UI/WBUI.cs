using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WBUI : MonoBehaviour
{
    Transform uiGroup;
    [SerializeField] BlockSpwaner blockSpwaner;

    [SerializeField] private bool isUIActive = false;
    [SerializeField] private bool canUse_Workbench = false;

    public bool CanUse_Workbench
    {
        get => canUse_Workbench;
        set
        {
            if(canUse_Workbench != value)
            {
                canUse_Workbench = value;

                // 트리거 밖으로 나가서 flase가 되면 자동으로 종료
                if(canUse_Workbench == false)
                {
                    Workbench_UI_Use(canUse_Workbench);
                    isUIActive = false;
                }
            }
        }
    }

    PlayerInputAction inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Use.performed += OnWorkBench;
    }

    private void OnDisable()
    {
        inputActions.UI.Use.performed -= OnWorkBench;
        inputActions.UI.Disable();
    }

    private void Start()
    {
        uiGroup = transform.GetChild(0);
        blockSpwaner = BaseCampManager.Instance.BlockSpwaner;
    }

    /// <summary>
    /// 트리거 범위 내부일때 e를 눌러서 UI 활성화 가능함
    /// </summary>
    /// <param name="_"></param>
    private void OnWorkBench(InputAction.CallbackContext _)
    {
        if (canUse_Workbench)
        {
            isUIActive = !isUIActive;
            Workbench_UI_Use(isUIActive);
        }
    }

    private void Workbench_UI_Use(bool isUse)
    {
        blockSpwaner.ProhibitSpawn(isUse);
        uiGroup.gameObject.SetActive(isUse);
    }
}
