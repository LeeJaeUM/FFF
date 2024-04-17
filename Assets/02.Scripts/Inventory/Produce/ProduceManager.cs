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
        
        // 리스트에 따로 보관
        foreach (ItemData data in inven.itemDatas)
        {
            if(data is ItemData_Produce)
            {
                produceList.Add((ItemData_Produce)data);
            }
        }
    }

    private void Start()
    {
        // 전달받은 리스트
        inven.onContainList = (list) =>
        {

        };
    }
}

// 구조체 만들기