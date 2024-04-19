using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceManager : MonoBehaviour
{
    public List<ItemData_Produce> produceList;

    public InventoryUI inven;

    private Transform lineTr;

    public ProduceLine[] lines;

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

        Transform child = transform.GetChild(0);    // scrool view
        child = child.GetChild(0);                  // viewport
        lineTr = child.GetChild(0);                  // content

        lines = new ProduceLine[produceList.Count];

    }

    public void Initialize()
    {
        for (int i = 0; i < produceList.Count; i++)
        {
            lines[i] = Factory.Instance.GetProduceLine(lineTr);
            lines[i].Initialize(produceList[i]);
        }

        // 전달받은 리스트
        inven.onContainList += onRefresh;
    }

    private void onRefresh()
    { 
        foreach(var line in lines)
        {
            line.Refresh();
        }
    }
}

// 구조체 만들기