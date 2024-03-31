using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProduceUI : MonoBehaviour
{
    List<ItemData_Produce> itemDatas;

    ProduceSlot slotParent_1;
    ProduceSlot slotParent_2;
    ResultSlot resultSlot;

    // 컴포넌트
    Button resultButton;

    private void Awake()
    {
        itemDatas = new List<ItemData_Produce>();

        slotParent_1 = transform.GetChild(0).GetComponent<ProduceSlot>();
        slotParent_2 = transform.GetChild(1).GetComponent<ProduceSlot>();
        resultSlot = transform.GetChild(2).GetComponent<ResultSlot>();
        resultButton = GetComponentInChildren<Button>();
    }

    private void Start()
    {
        for(int i = 0; i < GameManager.Instance.inven.itemDatas.Length; i++)
        {
            ItemData_Produce parent = GameManager.Instance.inven.itemDatas[i] as ItemData_Produce;
            if(parent != null)
            {
                Debug.Log(parent.name);
                itemDatas.Add(parent);
            }
        }

        resultButton.onClick.AddListener(() =>
        {
            if(slotParent_1.data != null && slotParent_2.data != null)
            {
                Combine();
            }
        });
    }

    void Combine()
    {
        ItemData_Produce result = ItemCheck();

        slotParent_1.RemoveData();
        slotParent_2.RemoveData();

        Debug.Log(result.name);

        resultSlot.SetData(result);
    }

    /// <summary>
    /// 아이템을 체크하여 합쳐지는 것을 확인하는 함수
    /// </summary>
    ItemData_Produce ItemCheck()
    {
        ItemData_Produce resultItem = null;

        for(int i = 0; i < itemDatas.Count; i++)
        {
            for (int j = 0; j < itemDatas.Count; j++)
            {
                if(slotParent_1.data == itemDatas[j].parentData_1 || slotParent_1.data == itemDatas[j].parentData_2)
                {
                    if(slotParent_2.data == itemDatas[i].parentData_1 || slotParent_2.data == itemDatas[i].parentData_2)
                    {
                        resultItem = itemDatas[i];
                    }
                }
            }
        }

        Debug.Log(resultItem);

        return resultItem;
    }
}
