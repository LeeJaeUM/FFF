using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 오브젝트 풀을 사용하는 오브젝트의 종류
/// </summary>
public enum PoolObjectType
{
    ItmeContain,
    GridSlot
}

public class Factory : Singleton<Factory>
{
    // 오브젝트 풀들
    GridSlotPool SlotPool;
    ItemContainPool ContainPool;
    ProduceSlotPool ProducePool;

    public Transform containChild;

    /// <summary>
    /// 씬이 로딩 완료될 때마다 실행되는 초기화 함수
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
    /// 풀에 있는 게임 오브젝트 하나 가져오기
    /// </summary>
    /// <param name="type">가져울 오브젝트의 종류</param>
    /// <param name="position">오브젝트가 배치될 위치</param>
    /// <returns>활성화된 오브젝트</returns>
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

    public ItemContain GetItemContain(ItemData data, int _count = 1)
    {
        ItemContain contain = ContainPool.GetObject();

        contain.GetComponent<ItemContain>().ContainInitialize(data, _count);
        contain.transform.SetParent(GameManager.Instance.inven.DragParent);

        return contain;
    }

    public InvenSlot GetGridSlot()
    {
        return SlotPool.GetObject();
    }

    public InvenSlot GetGridSlot(int x, int y, Transform parent)
    {
        InvenSlot result = GetGridSlot();

        result.transform.name = $"slot[{x},{y}]";
        result.transform.SetParent(parent);

        return result;
    }

    public ProduceSlot GetProduceSlot(ItemCode _code, int _count, int _total)
    {
        ProduceSlot result = ProducePool.GetObject();

        result.SetData(_code, _count, _total);

        return result;
    }
}
