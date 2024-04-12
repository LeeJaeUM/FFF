using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class InvenSlot : RecycleObject, IPointerEnterHandler, IPointerExitHandler
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
    public ItemContain storedItemContain;

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


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (data != null)
        {
            GameManager.Instance.inven.tooltip.Open(data);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.inven.tooltip.Close();
    }

    public void SlotRemove()
    {
        storedItemSize = Vector2Int.zero;
        storedItemStartPos = Vector2Int.zero;
        data = null;
        isEmpty = true;
    }

    public void SlotStore(ItemContain Item, Vector2Int startPosition)
    {
        storedItemContain = Item;
        data = Item.item;
        storedItemSize = data.Size;
        storedItemStartPos = startPosition;
        isEmpty = false;
    }
}
