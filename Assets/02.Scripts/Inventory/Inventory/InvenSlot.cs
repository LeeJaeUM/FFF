using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
//using static UnityEditor.Progress;

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

    ///// <summary>
    ///// 저장된 물체 아이템 컨테이너
    ///// </summary>
    public ItemContain storedContain;

    ///// <summary>
    ///// 저장된 물체의 사이즈
    ///// </summary>
    public Vector2Int storedItemSize;

    ///// <summary>
    ///// 저장된 물체의 시작 좌표
    ///// </summary>
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
            slotsSector[i].SlotSectorInitialize(this, i + 1);
        }

        isEmpty = true;
    }


    public void SlotRemove()
    {
        storedContain = null;
        data = null;
        storedItemSize = Vector2Int.zero;
        storedItemStartPos = Vector2Int.zero;
        isEmpty = true;
    }

    public void SlotStore(ItemContain contain)
    {
        storedContain = contain;
        data = storedContain.item;
        storedItemSize = data.Size;
        storedItemStartPos = contain.storeSlots[0].gridPos;
        isEmpty = false;
    }
}
