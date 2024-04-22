using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 제작에 아이템을 보여주는 클래스
public class ProduceLine : RecycleObject
{
    #region 변수
    private ItemData_Produce data;

    public ItemData_Produce Data => data;

    public GameObject IngredientSlot;

    public ProduceSlot[] produceSlots;
    #endregion

    #region UI 컴포넌트
    Image itemIcon;
    TextMeshProUGUI itemName;
    Transform ingredient;
    Button produceButton;
    #endregion

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        itemIcon = child.GetComponent<Image>();
        child = transform.GetChild(1);
        itemName = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        ingredient = child.GetComponent<Transform>();
        child = transform.GetChild(3);
        produceButton = child.GetComponent<Button>();
    }

    #region 제작아이템 리스트
    /// <summary>
    /// 시작시 동작하는 함수
    /// </summary>
    /// <param name="data">들어갈 아이템 정보</param>
    public void Initialize(ItemData_Produce _data)
    {
        this.data = _data;

        itemIcon.sprite = _data.itemIcon;
        itemName.text = _data.itemName;

        produceSlots = new ProduceSlot[_data.parentCodes.Length];
        int index = 0;
        foreach(var code in _data.parentCodes)
        {
            produceSlots[index] = Factory.Instance.GetProduceSlot(code.Code, code.Count, ingredient);
            index++;
        }

        // 아이템 갯수 체크
        Refresh();

        // 버튼 클릭시 
        produceButton.onClick.AddListener(() =>
        {
            InventoryUI inven = GameManager.Instance.inven;
            foreach(var slot in produceSlots)
            {
                slot.UseItem();
            }
            ItemContain contain = Factory.Instance.GetItemContain(data);
            contain.GrabContain();
        });
    }

    /// <summary>
    /// 아이템 갯수가 맞는지 확인하는 함수
    /// </summary>
    public void Refresh()
    {
        foreach(var slot in produceSlots)
        {
            foreach(var ItemContain in GameManager.Instance.inven.containList)
            {

                if(ItemContain.itemCode == slot.Data.itemCode)
                {
                    slot.SetIngredient(ItemContain.itemCount);
                }
            }
        }

        Debug.Log(checkProduce());
        produceButton.interactable = checkProduce();
    }

    public bool checkProduce()
    {
        foreach(var slot in produceSlots)
        {
            //Debug.Log(slot.IsProduceOk);
            if (!slot.IsProduceOk)
            {
                return false;
            }
        }

        return true;
    }
    #endregion

}
