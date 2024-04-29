using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceManager : MonoBehaviour
{
    public List<ItemData_Produce> produceList;

    public InventoryUI inven;

    private Transform lineTr;

    public ProduceLine[] lines;

    private CanvasGroup canvas;

    private void Awake()
    {
        Transform child = transform.GetChild(1);    // scrool view
        child = child.GetChild(0);                  // viewport
        lineTr = child.GetChild(0);                  // content
        canvas = GetComponent<CanvasGroup>();    
    }

    public void Initialize()
    {
        inven = GameManager.Instance.inven;

        // 리스트에 따로 보관
        foreach (ItemData data in inven.itemDatas)
        {
            if (data is ItemData_Produce)
            {
                produceList.Add((ItemData_Produce)data);
            }
        }

        lines = new ProduceLine[produceList.Count];

        for (int i = 0; i < produceList.Count; i++)
        {
            lines[i] = Factory.Instance.GetProduceLine(lineTr);
            lines[i].Initialize(produceList[i]);
        }

        // 전달받은 리스트
        inven.onContainListChange += onRefresh;

        OnOff();
    }

    private void onRefresh()
    {
        Debug.Log("리스트 변화 감지");
        foreach(var line in lines)
        {
            line.Refresh();
        }
    }

    public void OnOff()
    {
        if (canvas.alpha == 0.0f)
        {
            Open();
        }
        else if (canvas.alpha == 1.0f)
        {
            Close();
        }
    }

    private void Open()
    {
        canvas.alpha = 1.0f;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
    }

    private void Close()
    {
        canvas.blocksRaycasts = false;
        canvas.interactable = false;
        canvas.alpha = 0;
    }
}