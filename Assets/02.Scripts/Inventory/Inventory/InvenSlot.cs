using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenSlot : RecycleObject
{
    /// <summary>
    /// �׸��忡 ����� ������ ����
    /// </summary>
    public ItemData data;

    /// <summary>
    /// �׸��� ��ǥ
    /// </summary>
    public Vector2Int gridPos;

    /// <summary>
    /// ����� ��ü ���� ������Ʈ
    /// </summary>
    public GameObject storedItemObject;

    /// <summary>
    /// ����� ��ü�� ������
    /// </summary>
    public Vector2Int storedItemSize;

    /// <summary>
    /// ����� ��ü�� ���� ��ǥ
    /// </summary>
    public Vector2Int storedItemStartPos;

    /// <summary>
    /// �� ������ ��Ÿ���� ����
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
}
