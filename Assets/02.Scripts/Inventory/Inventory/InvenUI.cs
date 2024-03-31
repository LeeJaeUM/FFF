using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenUI : MonoBehaviour
{
    // 하위 UI
    RectTransform InvenHead;
    RectTransform InvenInfo;
    RectTransform InvenGrid;

    InventoryUI inven => GameManager.Instance.inven;

    private void Awake()
    {
        InvenHead = transform.GetChild(0).GetComponent<RectTransform>();
        InvenInfo = transform.GetChild(0).GetComponent<RectTransform>();
        InvenGrid = transform.GetChild(0).GetComponent<RectTransform>();
    }

    private void Initialized()
    {
        InvenHead.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            inven.edgePadding * 2 + inven._horizontalSlotCount * inven.slotSize);
        InvenHead.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            inven.edgePadding * 2 + inven.slotSize);
        InvenInfo.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            inven.edgePadding * 2 + inven._horizontalSlotCount * inven.slotSize);
        InvenInfo.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            inven.edgePadding * 2 + inven.slotSize);
    }
}
