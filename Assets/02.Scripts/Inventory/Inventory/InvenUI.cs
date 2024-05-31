using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenUI : MonoBehaviour
{
    // 하위 UI
    CanvasGroup canvas;
    RectTransform invenHeadRect;
    RectTransform invenInfoRect;
    RectTransform invenGridRect;

    InventoryUI inven;



    private void Awake()
    {
        inven = transform.parent.GetComponent<InventoryUI>();
        canvas = GetComponent<CanvasGroup>();
        invenHeadRect = transform.GetChild(0).GetComponent<RectTransform>();
        invenInfoRect = transform.GetChild(1).GetComponent<RectTransform>();
        invenGridRect = transform.GetChild(2).GetComponent<RectTransform>();
    }

    public void Initialized()
    {
        // 헤드 부분 크기
        invenHeadRect.localPosition = new(0, inven.edgePadding * 2 + inven._verticalSlotCount * inven.slotSize + inven.edgePadding * 2 + inven.slotSize);
        invenHeadRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            inven.edgePadding * 2 + inven._horizontalSlotCount * inven.slotSize);
        invenHeadRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            inven.edgePadding * 2 + inven.slotSize);

        // 정보창 크기
        invenInfoRect.localPosition = new(0, inven.edgePadding * 2 + inven._verticalSlotCount * inven.slotSize);
        invenInfoRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            inven.edgePadding * 2 + inven._horizontalSlotCount * inven.slotSize);
        invenInfoRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            inven.edgePadding * 2 + inven.slotSize);

        // 그리드의 크기
        invenGridRect.localPosition = new(0, 0);
        invenGridRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            inven.edgePadding * 2 + inven._horizontalSlotCount * inven.slotSize);
        invenGridRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            inven.edgePadding * 2 + inven._verticalSlotCount * inven.slotSize);

        OnOff();
    }

    public void OnOff()
    {
        if(canvas.alpha == 0.0f)
        {
            Open();
        }
        else if(canvas.alpha == 1.0f)
        {
            Close();
        }
    }

    private void Open()
    {
        canvas.alpha = 1.0f;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
    }

    private void Close()
    {
        canvas.blocksRaycasts = false;
        canvas.interactable = false;
        canvas.alpha = 0;
    }
}
