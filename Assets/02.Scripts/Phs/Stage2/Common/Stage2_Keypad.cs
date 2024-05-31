using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stage2_Keypad : MonoBehaviour, IInteractable
{
    KeypadSystem.KeyPad keyPad;
    [SerializeField] private bool isUse = false;

    /// <summary>
    /// 키패드 입력이 성공해는지 안했는지 확인하는 델리게이트
    /// </summary>
    public Action<bool> onSuccess;

    private void Start()
    {
        keyPad = Stage1Manager.Instance.KeyPad;
        keyPad.onAnswerCheck += OnSuccess;
        keyPad.onAnswerFail += OnFail;
    }
    
    /// <summary>
    /// 성공시 동작할 함수
    /// </summary>
    private void OnSuccess()
    {
        onSuccess?.Invoke(true);
    }

    /// <summary>
    /// 실패시 동작할 함수
    /// </summary>
    private void OnFail()
    {
        onSuccess?.Invoke(false);
    }

    public void Interact()
    {
        isUse = !isUse;
        keyPad.gameObject.SetActive(isUse);
    }
}
