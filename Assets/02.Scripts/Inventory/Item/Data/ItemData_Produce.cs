using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData_Produce", menuName = "Data/ItemData_Produce", order = 1)]
public class ItemData_Produce : ItemData
{
    [Header("조합 아이템 부모")]
    public parentCode[] parentCodes;

}

[Serializable]
public struct parentCode
{
    [SerializeField]
    private ItemCode code;

    public ItemCode Code => code;

    [SerializeField]
    [Range(1, 5)]
    private int count;

    public int Count => count;

    [SerializeField]
    private bool notConsume;

    public bool NotConsume => notConsume;

    public parentCode(ItemCode _code, int _count, bool isConsume)
    {
        this.code = _code;
        this.count = _count;
        this.notConsume = isConsume;
    }
}
