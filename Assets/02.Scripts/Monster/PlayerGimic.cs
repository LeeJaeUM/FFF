using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerGimic : MonoBehaviour
{
    [SerializeField] private bool isPlayerIn = false;

    //기본적으론 null 상태
    IInteractable cur_Interactable = null;

    public bool testIInteracHave = false;

    TipsUI tipsUI;
    PlayerInputAction inputActions;
    Collider otherSave;

    private void Awake()
    {
        inputActions = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Use.performed += OnPressE_UI;
    }

    private void OnDisable()
    {
        inputActions.UI.Use.performed -= OnPressE_UI;
        inputActions.UI.Disable();
    }

    private void Start()
    {
        tipsUI = Stage1Manager.Instance.TipsUI;
    }

    private void Update()
    {
        if (otherSave == null)
        {
            ExitInteract();
        }

        // || otherSave.gameObject 이걸로 사라졌는지 판단
        if (cur_Interactable == null)
        {
            testIInteracHave = false;
        }
        else
        {
            testIInteracHave = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        cur_Interactable = other.GetComponent<IInteractable>();
        if (cur_Interactable != null)
        {
            otherSave = other;
            isPlayerIn = true;
            tipsUI.CanUse_InteractObj = isPlayerIn;
        }

        if (other.gameObject.CompareTag("MONSTER"))
        {
            Stage1Manager.Instance.BottomTMPText = "플레이어 사망";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        cur_Interactable = other.GetComponent<IInteractable>();
        if (cur_Interactable != null)
        {
            ExitInteract();
        }
    }

    private void ExitInteract()
    {
        isPlayerIn = false;
        cur_Interactable = null;
        tipsUI.CanUse_InteractObj = isPlayerIn;
    }

    /// <summary>
    /// 트리거 범위 내부일때 e를 눌러서 UI 활성화 가능함
    /// </summary>
    /// <param name="_"></param>
    private void OnPressE_UI(InputAction.CallbackContext _)
    {
        if (cur_Interactable != null)
        {
            cur_Interactable.Interact();
        }
    }
}
