using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenGrid : MonoBehaviour
{
    /// <summary>
    /// 그리드 오브젝트
    /// </summary>
    public InvenSlot[,] slotGrid;

    /// <summary>
    /// 그리드 프리펩
    /// </summary>
    public GameObject slotPrefab;

    /// <summary>
    /// 그리드 사이즈
    /// </summary>
    public Vector2Int gridSize => GameManager.Instance.inven.gridSize;

    /// <summary>
    /// 슬롯의 크기
    /// </summary>
    public float slotSize => GameManager.Instance.inven.slotSize;

    /// <summary>
    /// 슬롯 사이의 거리
    /// </summary>
    public float edgePadding => GameManager.Instance.inven.edgePadding;

    public void GridInitialize()
    {
        slotGrid = new InvenSlot[gridSize.x, gridSize.y];
        CreateSlots();
        ResizePanel();
    }

    /// <summary>
    /// 슬롯 생성
    /// </summary>
    private void CreateSlots()
    {
        for(int y = 0; y < gridSize.y;  y++)
        {
            for(int x = 0; x < gridSize.x; x++)
            {
                GameObject obj = Factory.Instance.GetGridSlot(x, y, this.transform);

                RectTransform rect = obj.transform.GetComponent<RectTransform>();
                rect.localPosition = new Vector3(x * slotSize + edgePadding, y * slotSize + edgePadding, 0);
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);
                obj.GetComponent<RectTransform>().localScale = Vector3.one;
                obj.GetComponent<InvenSlot>().SlotInitialize(x, y);
                slotGrid[x, y] = obj.GetComponent<InvenSlot>();
            }
        }
        GameManager.Instance.inven.slotGrid = slotGrid;
    }

    /// <summary>
    /// 사이즈를 다시 조절
    /// </summary>
    private void ResizePanel()
    {
        float width = (gridSize.x * slotSize) + edgePadding * 2;
        float height = (gridSize.y * slotSize) + edgePadding * 2;

        RectTransform rect = GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        rect.localPosition = Vector3.one;
    }
}
