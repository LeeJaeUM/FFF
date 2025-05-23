using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProduceSlot : RecycleObject
{
    ItemData data;

    public ItemData Data => data;

    int count = 0;
    int total = 0;

    public int Total
    {
        get => total;
        set
        {
            if(total != value)
            {
                total = value;

                Refresh();
            }
        }
    }

    public bool IsProduceOk => count <= Total;


    #region 컴포넌트
    private Image icon;
    private TextMeshProUGUI countText;
    #endregion

    private void Awake()
    {
        icon = GetComponentInChildren<Image>();
        countText = GetComponentInChildren<TextMeshProUGUI>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_code"></param>
    /// <param name="_count"></param>
    public void SetData(ItemCode _code, int _count)
    {
        this.data = GameManager.Instance.inven.FindCodeData(_code);
        this.count = _count;

        Refresh();
    }

    /// <summary>
    /// 필요한 재료 갯수 반환
    /// </summary>
    /// <param name="_value"></param>
    public void SetIngredient(int _value)
    {
        //Debug.Log($"갯수 변환{_value}");
        Total = _value;
    }

    private void Refresh()
    {
        Debug.Log(IsProduceOk);
        icon.sprite = data.itemIcon;
        countText.text = $"X{total}/{count}";
    }

    public void UseItem(bool isConsume)
    {
        bool isUse = false;
        
        Debug.LogWarning(isConsume);
        if (isConsume)
        {
            isUse = GameManager.Instance.inven.UseItem(data.itemCode, count);
        }

        Debug.Log(isUse);
    }
}
