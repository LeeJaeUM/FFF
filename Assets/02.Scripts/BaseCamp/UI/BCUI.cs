using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class BCUI : MonoBehaviour
{
    PlayerInputAction inputAction;

    [SerializeField]GameObject buildUI;
    [SerializeField] bool isActive = false;

    [SerializeField] BuildSelectUI[] buildSelectUIs = null;
    [SerializeField] BlockSpwaner spwaner = null;


    private void Awake()
    {
        inputAction = new PlayerInputAction();  
        Transform child = transform.GetChild(0);
        buildUI = child.gameObject;

        buildSelectUIs = GetComponentsInChildren<BuildSelectUI>(true);
        
    }

    private void Start()
    {
        spwaner = BaseCampManager.Instance.BlockSpwaner;
        for (int i = 0; i < buildSelectUIs.Length; i++)
        {
            buildSelectUIs[i].onClick += OnClickBuildObjIcon;
        }
    }

    /// <summary>
    /// 액션으로 BuildSelecUI에서 받아온 BuildMode를 설정
    /// </summary>
    /// <param name="index"></param>
    private void OnClickBuildObjIcon(int index)
    {
        spwaner.buildMode = (BlockSpwaner.BuildMode)index;
    }

    private void OnEnable()
    {
        inputAction.Enable();
        inputAction.Player.BuildMode.performed += OnBuildMode;
        //inputAction.Player.BuildMode.canceled += OnBuildMode;
    }
    private void OnDisable()
    {
        //inputAction.Player.BuildMode.canceled -= OnBuildMode;
        inputAction.Player.BuildMode.performed -= OnBuildMode;
        inputAction.Disable();
    }

    private void OnBuildMode(InputAction.CallbackContext context)
    {
        isActive = !isActive;
        buildUI.SetActive(isActive);
        spwaner.ProhibitSpawn(isActive);
        
    }

}
