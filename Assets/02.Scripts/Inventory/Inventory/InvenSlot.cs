using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvenSlot : RecycleObject
{
    /// <summary>
    /// 그리드에 저장된 아이템 정보
    /// </summary>
    public ItemData data;

    /// <summary>
    /// 그리드 좌표
    /// </summary>
    public Vector2Int gridPos;

    /// <summary>
    /// 저장된 물체 게임 오브젝트
    /// </summary>
    public GameObject storedItemObject;

    /// <summary>
    /// 저장된 물체의 사이즈
    /// </summary>
    public Vector2Int storedItemSize;

    /// <summary>
    /// 저장된 물체의 시작 좌표
    /// </summary>
    public Vector2Int storedItemStartPos;

    /// <summary>
    /// 빈 슬롯을 나타내는 여부
    /// </summary>
    public bool isEmpty = true;

    public void SlotInitialize(int x, int y)
    {
        gridPos.x = x; 
        gridPos.y = y;

        SlotSector[] slotsSector = GetComponentsInChildren<SlotSector>();
        for (int i = 0; i < slotsSector.Length; i++)
        {
            slotsSector[i].SlotSectorInitialize(gameObject, i + 1);
        }

        isEmpty = true;
    }
}
