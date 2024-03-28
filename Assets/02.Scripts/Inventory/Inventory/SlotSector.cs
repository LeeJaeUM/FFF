using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotSector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// ����(���� ��ü)
    /// </summary>
    public GameObject slotParent;

    /// <summary>
    /// ���� ��ü �ѹ�
    /// </summary>
    public int QuadNum;

    public static Vector2Int posOffset;

    public static SlotSector Instance;

    [SerializeField]
    private InventoryUI inven => GameManager.Instance.inven;

    private GameObject itemContain => inven.containGrab;

    [SerializeField]
    /// <summary>
    /// �θ��� ��ũ��Ʈ
    /// </summary>
    private InvenSlot parentSlotScript;

    public void SlotSectorInitialize(GameObject obj, int id)
    {
        slotParent = obj;
        parentSlotScript = obj.GetComponent<InvenSlot>();
        QuadNum = id;
    }

    /// <summary>
    /// ���콺 ����Ʈ�� ���� ���
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        Instance = this;
        inven.highlightedSlot = slotParent;

        // ���콺 ��ġ�� � UI�� �ִ�.
        SetPosOffset();
        if (itemContain != null)
        {
            inven.RefrechColor(true);
        }
        if (parentSlotScript.storedItemObject != null && itemContain == null)
        {
            inven.ColorChangeLoop(SlotColorHighlights.Blue,
                parentSlotScript.storedItemSize, parentSlotScript.storedItemStartPos);
        }
        if (parentSlotScript != null)
        {
            //GameManager.Instance.inventory.tooltip.Open(parentSlotScript.data);
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
        if(parentSlotScript.storedItemObject != null && itemContain == null)
        {
            inven.ColorChangeLoop(SlotColorHighlights.Blue2, parentSlotScript.storedItemSize, parentSlotScript.storedItemStartPos);
        }
    }
}
