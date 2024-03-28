using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenGrid : MonoBehaviour
{
    /// <summary>
    /// �׸��� ������Ʈ
    /// </summary>
    public GameObject[,] slotGrid;

    /// <summary>
    /// �׸��� ������
    /// </summary>
    public GameObject slotPrefab;

    /// <summary>
    /// �׸��� ������
    /// </summary>
    public Vector2Int gridSize => GameManager.Instance.inven.gridSize;

    /// <summary>
    /// ������ ũ��
    /// </summary>
    public float slotSize => GameManager.Instance.inven.slotSize;

    /// <summary>
    /// ���� ������ �Ÿ�
    /// </summary>
    public float edgePadding => GameManager.Instance.inven.edgePadding;

    public void GridInitialize()
    {
        slotGrid = new GameObject[gridSize.x, gridSize.y];
        CreateSlots();
        ResizePanel();
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    private void CreateSlots()
    {
        for(int y = 0; y < gridSize.y;  y++)
        {
            for(int x = 0; x < gridSize.x; x++)
            {
                GameObject obj = Factory.Instance.GridSlot(x, y, this.transform);

                RectTransform rect = obj.transform.GetComponent<RectTransform>();
                rect.localPosition = new Vector3(x * slotSize + edgePadding, y * slotSize + edgePadding, 0);
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);
                obj.GetComponent<RectTransform>().localScale = Vector3.one;
                obj.GetComponent<InvenSlot>().SlotInitialize(x, y);
                slotGrid[x, y] = obj;
            }
        }
        GameManager.Instance.inven.slotGrid = slotGrid;
    }

    /// <summary>
    /// ����� �ٽ� ����
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
