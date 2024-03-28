using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// ������Ʈ Ǯ�� ����ϴ� ������Ʈ�� ����
/// </summary>
public enum PoolObjectType
{
    ItmeContain,
    GridSlot
}

public class Factory : Singleton<Factory>
{
    // ������Ʈ Ǯ��
    GridSlotPool SlotPool;
    ItemContainPool ContainPool;

    public Transform containChild;

    /// <summary>
    /// ���� �ε� �Ϸ�� ������ ����Ǵ� �ʱ�ȭ �Լ�
    /// </summary>
    protected override void OnInitialize()
    {
        base.OnInitialize();

        ContainPool = GetComponentInChildren<ItemContainPool>();
        if (ContainPool != null)
        {
            ContainPool.Initialized();
            containChild = ContainPool.transform;
        }

        SlotPool = GetComponentInChildren<GridSlotPool>();
        if(SlotPool != null ) SlotPool.Initialized();
    }

    /// <summary>
    /// Ǯ�� �ִ� ���� ������Ʈ �ϳ� ��������
    /// </summary>
    /// <param name="type">������ ������Ʈ�� ����</param>
    /// <param name="position">������Ʈ�� ��ġ�� ��ġ</param>
    /// <returns>Ȱ��ȭ�� ������Ʈ</returns>
    public GameObject GetObject(PoolObjectType type, Vector3? position = null, Vector3? eular = null)
    {
        GameObject result = null;

        switch (type)
        {
            case PoolObjectType.ItmeContain:
                result = ContainPool.GetObject(position, eular).gameObject;
                break;
            case PoolObjectType.GridSlot:
                result = SlotPool.GetObject(position, eular).gameObject;
                break;
        }

        return result;
    }

    public GameObject ItemContain(ItemData data, int _count = 1)
    {
        GameObject obj = ContainPool.GetObject().gameObject;

        obj.GetComponent<ItemContain>().ContainInitialize(data, _count);
        obj.transform.SetParent(GameManager.Instance.inven.DragParent);

        return obj;
    }

    public GameObject GridSlot()
    {
        return SlotPool.GetObject().gameObject;
    }

    public GameObject GridSlot(int x, int y, Transform parent)
    {
        GameObject result = SlotPool.GetObject().gameObject;

        result.transform.name = $"slot[{x},{y}]";
        result.transform.SetParent(parent);

        return result;
    }
}
