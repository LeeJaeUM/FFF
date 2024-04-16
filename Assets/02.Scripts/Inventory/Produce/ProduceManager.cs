using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceManager : MonoBehaviour
{
    public List<ItemData_Produce> produceList;

    public InventoryUI inven;

    private void Awake()
    {
        inven = GameManager.Instance.inven;

        foreach (ItemData data in inven.itemDatas)
        {
            if(data is ItemData_Produce)
            {
                produceList.Add((ItemData_Produce)data);
            }
        }
    }
}
