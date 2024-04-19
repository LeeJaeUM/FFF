using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenUI : MonoBehaviour
{
    // 하위 UI
    RectTransform invenHeadRect;
    RectTransform invenInfoRect;
    RectTransform invenGridRect;

    InventoryUI inven => GameManager.Instance.inven;



    private void Awake()
    {
        invenHeadRect = transform.GetChild(0).GetComponent<RectTransform>();
        invenInfoRect = transform.GetChild(1).GetComponent<RectTransform>();
        invenGridRect = transform.GetChild(2).GetComponent<RectTransform>();
    }

    private void Start()
    {
        InvenHead head = GetComponentInChildren<InvenHead>();
        head.Drag += (position) =>
        {
            transform.position = transform.position + (Vector3)position;
        };
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
    }
}
