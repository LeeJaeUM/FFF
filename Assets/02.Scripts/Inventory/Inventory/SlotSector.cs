using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotSector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// 슬롯(상위 개체)
    /// </summary>
    public InvenSlot slotParent;

    /// <summary>
    /// 고유 객체 넘버
    /// </summary>
    public int QuadNum;

    public static Vector2Int posOffset;

    public static SlotSector Instance;

    [SerializeField]
    private InventoryUI inven => GameManager.Instance.inven;

    private ItemContain itemContain => inven.containGrab;

    [SerializeField]
    /// <summary>
    /// 부모의 스크립트
    /// </summary>
    private InvenSlot parentSlotScript;

    public void SlotSectorInitialize(InvenSlot obj, int id)
    {
        slotParent = obj;
        parentSlotScript = obj.GetComponent<InvenSlot>();
        QuadNum = id;
    }

    /// <summary>
    /// 마우스 포인트가 들어올 경우
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        Instance = this;
        inven.highlightedSlot = slotParent;

        // 마우스 위치에 어떤 UI가 있다.
        SetPosOffset();
        if (itemContain != null)
        {
            inven.RefrechColor(true);
        }
        if (parentSlotScript.storedItemContain != null && itemContain == null)
        {
            inven.ColorChangeLoop(SlotColorHighlights.Blue,
                parentSlotScript.storedItemSize, parentSlotScript.storedItemStartPos);
        }
    }
    
    public void SetPosOffset()
    {
        Vector2Int size = Vector2Int.zero;

        if(itemContain != null)
        {
            size = itemContain.GetComponent<ItemContain>().ItemSize;
        }

        if (size.x != 0 && size.x % 2 == 0)
        {
            switch (QuadNum)
            {
                case 1:
                    posOffset.x = 0;
                    break;
                case 2:
                    posOffset.x = -1;
                    break;
                case 3:
                    posOffset.x = 0;
                    break;
                case 4:
                    posOffset.x = -1;
                    break;
            }
        }
        if (size.y != 0 && size.y % 2 == 0)
        {
            switch (QuadNum)
            {
                case 1:
                    posOffset.y = -1;
                    break;
                case 2:
                    posOffset.y = -1;
                    break;
                case 3:
                    posOffset.y = 0;
                    break;
                case 4:
                    posOffset.y = 0;
                    break;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Instance = null;
        inven.highlightedSlot = null;
        
        if(itemContain != null)
        {
            inven.RefrechColor(false);
        }
        posOffset = Vector2Int.zero;
        if(parentSlotScript.storedItemContain != null && itemContain == null)
        {
            inven.ColorChangeLoop(SlotColorHighlights.Blue2, parentSlotScript.storedItemSize, parentSlotScript.storedItemStartPos);
        }
    }
}
