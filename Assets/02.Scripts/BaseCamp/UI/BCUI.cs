using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Cinemachine;

public class BCUI : MonoBehaviour
{
    PlayerInputAction inputAction;

    [SerializeField]GameObject buildUI;
    [SerializeField] bool isActive = false;

    /// <summary>
    /// 블럭 설치 모드 변경 버튼 그룹
    /// </summary>
    [SerializeField] GameObject blockSelectGroup;
    /// <summary>
    /// 환경요소 선택 버튼 그룹
    /// </summary>
    [SerializeField] GameObject enviroSelectGroup;

    [SerializeField] BuildSelectUI[] buildSelectUIs = null;
    [SerializeField] BlockSpwaner spwaner = null;

    /// <summary>
    /// 벽 생성 시 재료 선택 UI 
    /// </summary>
    MaterialSelectUI materialSelectUI;

    /// <summary>
    /// 카메라
    /// </summary>
    public CinemachineVirtualCamera virtualCamera;

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
        materialSelectUI.onSelectEnviroment += OnSelectEnviroment;
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
        SetCameraFOV(60.0f);
        Debug.Log($"눌린 버튼은 buildmode선택 버튼 : {index}");
        if (index == 0)
        {
            //종료 버튼을 눌렀을 때
            isActive = !isActive;
            buildUI.SetActive(isActive);
            spwaner.ProhibitSpawn(isActive);
            SetCameraFOV(40.0f);
        }

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
            spwaner.MaterialTypeP = (BlockSpwaner.MaterialType)btnIndex;
        }
        else if (btnIndex == 3) 
        { 
            //Enviroment를 눌렀을 때

            blockSelectGroup.SetActive(false);
            enviroSelectGroup.SetActive(true);
            return;
        }
    }

    private void OnSelectEnviroment(int btnIndex)
    {
        Debug.Log(btnIndex);

        spwaner.EnviromentIndex = btnIndex;

    }

    public void SetCameraFOV(float fov)
    {
        if (virtualCamera != null)
        {
            virtualCamera.m_Lens.FieldOfView = fov;
        }
        else
        {
            Debug.LogError("Virtual Camera is not assigned!");
        }
    }
}
