using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemContain : RecycleObject, IPointerClickHandler
{
    public int id;

    public ItemData item;

    Image itemIcon;
    TextMeshProUGUI itemCount;

    public Vector2Int ItemSize;

    public bool isDragging = false;

    private float slotSize;

    /// <summary>
    /// 아이템 갯수
    /// </summary>
    private int count;
    public int Count
    {
        get => count;
        set
        {
            if(count != value)
            {
                count = value;
                Mathf.Clamp(count, 0, item.maxItemCount);
            }
        }
    }

    /// <summary>
    /// 드래그시 부모 개체
    /// </summary>
    private Transform DragParent => GameManager.Instance.inven.DragParent;

    private void Awake()
    {
        itemIcon = transform.GetChild(1).GetComponent<Image>();
        itemCount = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        //Debug.Log(item.Size);
    }

    /// <summary>
    /// 컨테이너 시작 함수
    /// </summary>
    public void ContainInitialize(ItemData data, int _count = 1)
    {
        slotSize = GameManager.Instance.inven.slotSize;

        SetItemObject(data, _count);
    }

    private void Start()
    {
        ContainInitialize(item);
    }

    private void Update()
    {
        if(isDragging)
        {
            transform.position = Input.mousePosition;
        }
    }

    /// <summary>
    /// 컨테이너에 아이템 정보 넣기
    /// </summary>
    /// <param name="_data">들어갈 아이템 정보</param>
    public void SetItemObject(ItemData _data, int _count = 1)
    {
        item = _data;
        ItemSize = item.Size;
        Count = _count;
        isDragging = false;

        RectTransform rect = GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ItemSize.x * slotSize);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ItemSize.y * slotSize);
        itemIcon.sprite = _data.itemIcon;
        itemCount.text = Count.ToString();
    }

    /// <summary>
    /// 선택된 아이템 컨테이너
    /// </summary>
    /// <param name="obj"></param>
    public GameObject SetSelectedItem(GameObject obj)
    {
        isDragging = true;
        obj.transform.SetParent(DragParent);
        obj.GetComponent<RectTransform>().localScale = Vector3.one;
        GameManager.Instance.inven.containGrab = obj;

        return obj;
    }

    /// <summary>
    /// 아이템 컨테어너 내용 지우기
    /// </summary>
    public void ResetSelectedItem()
    {
        isDragging = false;
        GameManager.Instance.inven.containGrab = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SetSelectedItem(this.gameObject);
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = !isDragging;
        canvasGroup.alpha = 0.5f;
    }

    public void ItemStack(int _count = 1)
    {
        Count += _count;
        itemCount.text = Count.ToString();
    }

    public void ItemDestack(int _count = 1)
    {
        Count -= _count;
        itemCount.text = Count.ToString();
    }

    public void ItemSplit(int _count = 1)
    {
        ItemDestack(_count);

        GameManager.Instance.inven.containGrab = Factory.Instance.ItemContain(item, _count);
    }

    public void ContainRemvoe()
    {
        // 아이템 정보 삭제
        ResetSelectedItem();

        transform.SetParent(Factory.Instance.containChild);

        // 게임 오브젝트 비활성화
        gameObject.SetActive(false);
    }
}
