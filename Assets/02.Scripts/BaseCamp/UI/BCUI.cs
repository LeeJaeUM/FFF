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

    MaterialSelectUI materialSelectUI;


    private void Awake()
    {
        inputAction = new PlayerInputAction();  
        Transform child = transform.GetChild(0);
        buildUI = child.gameObject;

        buildSelectUIs = GetComponentsInChildren<BuildSelectUI>(true);

        materialSelectUI = GetComponentInChildren<MaterialSelectUI>();
    }

    private void Start()
    {
        //블록 스포너 불러오기
        spwaner = BaseCampManager.Instance.BlockSpwaner;
        for (int i = 0; i < buildSelectUIs.Length; i++)
        {
            buildSelectUIs[i].onClickBlock += OnClickBuildObjIcon;   //액션 등록
        }

        materialSelectUI.onSelecMAterial += OnBlockMatSetting;
    }

    private void OnEnable()
    {
        inputAction.Enable();
        inputAction.Player.BuildMode.performed += OnBuildMode;
    }
    private void OnDisable()
    {
        inputAction.Player.BuildMode.performed -= OnBuildMode;
        inputAction.Disable();
    }


    /// <summary>
    /// 액션으로 BuildSelecUI에서 받아온 BuildMode를 설정
    /// </summary>
    /// <param name="index"></param>
    private void OnClickBuildObjIcon(int index)
    {
        spwaner.buildMode = (BlockSpwaner.BuildMode)index;
    }


    /// <summary>
    /// 탭 누르면 켜지고 꺼지며 마우스를 제외한 플레이어 인풋 제한
    /// </summary>
    /// <param name="context"></param>
    private void OnBuildMode(InputAction.CallbackContext context)
    {
        isActive = !isActive;
        buildUI.SetActive(isActive);
        spwaner.ProhibitSpawn(isActive);
        
    }

    void OnBlockMatSetting(int btnIndex)
    {
        Debug.Log(btnIndex);
        // 환경요소가 아닐 때
        if(btnIndex != 3)
        {
            spwaner.materialType = (BlockSpwaner.MaterialType)btnIndex;
        }
        else
        {
            return;
        }
    }

}
