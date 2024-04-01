using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        InvenInfo = transform.GetChild(1).GetComponent<RectTransform>();
        InvenGrid = transform.GetChild(2).GetComponent<RectTransform>();
    }

    public void Initialized()
    {
        // 헤드 부분 크기
        InvenHead.localPosition = new(0, inven.edgePadding * 2 + inven._verticalSlotCount * inven.slotSize + inven.edgePadding * 2 + inven.slotSize);
        InvenHead.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            inven.edgePadding * 2 + inven._horizontalSlotCount * inven.slotSize);
        InvenHead.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            inven.edgePadding * 2 + inven.slotSize);

        // 정보창 크기
        InvenInfo.localPosition = new(0, inven.edgePadding * 2 + inven._verticalSlotCount * inven.slotSize);
        InvenInfo.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            inven.edgePadding * 2 + inven._horizontalSlotCount * inven.slotSize);
        InvenInfo.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            inven.edgePadding * 2 + inven.slotSize);

        // 그리드의 크기
        InvenGrid.localPosition = new(0, 0);
        InvenGrid.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            inven.edgePadding * 2 + inven._horizontalSlotCount * inven.slotSize);
        InvenGrid.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            inven.edgePadding * 2 + inven._verticalSlotCount * inven.slotSize);
    }
}
