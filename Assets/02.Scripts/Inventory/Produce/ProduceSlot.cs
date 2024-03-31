using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProduceSlot : MonoBehaviour, IPointerClickHandler
{
    public ItemData data;

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
    public void SetData(ItemData _data)
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
        GameObject containObj = GameManager.Instance.inven.containGrab;
        // 마우스에 아이템이 정보가 담겨있을 경우
        if (containObj != null)
        {
            ItemContain contain = containObj.GetComponent<ItemContain>();
            SetData(contain.item);
            if(contain.Count == 1)
            {
                contain.ContainRemvoe();        // 컨테이너 삭제
            }
            else
            {
                contain.ItemDestack();
            }
        }
        // 없을 경우
        else if(containObj == null && data != null)
        {
            GameManager.Instance.inven.GrabContain(Factory.Instance.GetItemContain(data));
        }
    }
}
