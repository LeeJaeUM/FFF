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

    [SerializeField] GameObject blockSelectGroup;
    [SerializeField] GameObject enviroSelectGroup;

    [SerializeField] BuildSelectUI[] buildSelectUIs = null;
    [SerializeField] BlockSpwaner spwaner = null;

    MaterialSelectUI materialSelectUI;


    private void Awake()
    {
        inputAction = new PlayerInputAction();  
        Transform child = transform.GetChild(0);
        buildUI = child.gameObject;

        Transform g_child = child.GetChild(1);
        blockSelectGroup = g_child.gameObject;

        g_child = child.GetChild(2);
        enviroSelectGroup = g_child.gameObject;

        materialSelectUI = GetComponent<MaterialSelectUI>();
        buildSelectUIs = GetComponentsInChildren<BuildSelectUI>(true);

    }

    private void Start()
    {
        //블록 스포너 불러오기
        spwaner = BaseCampManager.Instance.BlockSpwaner;
        for (int i = 0; i < buildSelectUIs.Length; i++)
        {
            buildSelectUIs[i].onClickBlock += OnClickBuildObjIcon;   //액션 등록
        }

        materialSelectUI.onSelectMaterial += OnBlockMatSetting;
    }

    private void OnEnable()
    {
        inputAction.Enable();
        inputAction.Player.BuildUI.performed += OnBuildUI;
    }
    private void OnDisable()
    {
        inputAction.Player.BuildUI.performed -= OnBuildUI;
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
    private void OnBuildUI(InputAction.CallbackContext context)
    {
        isActive = !isActive;
        buildUI.SetActive(isActive);
        spwaner.ProhibitSpawn(isActive);
        
    }

    void OnBlockMatSetting(int btnIndex)
    {
        Debug.Log(btnIndex);
        // 환경요소가 아닐 때
        if(btnIndex < 3)
        {
            blockSelectGroup.SetActive(true);
            enviroSelectGroup.SetActive(false);
            spwaner.materialType = (BlockSpwaner.MaterialType)btnIndex;
        }
        else
        {
            //Enviroment를 눌렀을 때
            blockSelectGroup.SetActive(false);
            enviroSelectGroup.SetActive(true);
            return;
        }
    }

}
