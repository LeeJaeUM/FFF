using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResultSlot : MonoBehaviour, IPointerClickHandler
{
    public ItemData_Produce data;

    // 컴포넌트
    Image itemIcon;

    public void Awake()
    {
        itemIcon = GetComponent<Image>();
    }

    /// <summary>
    /// 아이템 정보 입력
    /// </summary>
    /// <param name="_data"></param>
    public void SetData(ItemData_Produce _data)
    {
        this.data = _data;
        itemIcon.sprite = data.itemIcon;
    }

    /// <summary>
    /// 아이템 정보 제거
    /// </summary>
    public void RemoveData()
    {
        this.data = null;
        itemIcon.sprite = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(data != null)
        {
            GameManager.Instance.inven.GrabContain(Factory.Instance.GetItemContain(data));
            RemoveData();
        }
    }
}
