using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    [SerializeField] private bool isPlayerIn = false;

    /// <summary>
    /// ㅌ트리거 범위 내에 들어 왔을때 e를 눌러서 UI비/활성화 가능, 나갔을때 자동으로 UI 비활성화
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerIn = true;
            BaseCampManager.Instance.WBUI.CanUse_Workbench = isPlayerIn;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerIn = false;
            BaseCampManager.Instance.WBUI.CanUse_Workbench = isPlayerIn;

        }
    }
}
