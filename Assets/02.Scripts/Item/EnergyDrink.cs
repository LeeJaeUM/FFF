using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrink : MonoBehaviour
{
    /// <summary>
    /// 에너지드링크 효과 지속 시간
    /// </summary>
    public float duration = 3.0f;

    private PlayerCtrl player;

    private ActionController controller;

    public Action OnGetDrink;

    /// <summary>
    /// 아이템 사용 여부를 나타내는 플래그
    /// </summary>
    private bool isUsed = false;

    private void Start()
    {
        player = FindObjectOfType<PlayerCtrl>();
        controller = FindObjectOfType<ActionController>();
    }


    /// <summary>
    /// 아이템 사용 시 호출되는 메서드
    /// </summary>
    public void UseItem()
    {
        if (!isUsed && controller)
        {
            // 이동 속도를 증가시킴
            player.ModifyMoveSpeed(2);
            Debug.Log($"{player.MoveSpeed}");

            // 아이템 효과 지속 시간 후에 이동 속도를 원래대로 복구시킴
            Invoke("RestoreMoveSpeed", duration);

            // 아이템 사용 여부 플래그 설정
            isUsed = true;

            OnGetDrink?.Invoke();
        }
    }

    // 아이템 효과 지속 시간 후에 호출되는 메서드
    private void RestoreMoveSpeed()
    {
        player.ModifyMoveSpeed(1);
        Debug.Log($"{player.MoveSpeed}");

        // 아이템 사용 여부 플래그 재설정
        isUsed = false;
    }
}
