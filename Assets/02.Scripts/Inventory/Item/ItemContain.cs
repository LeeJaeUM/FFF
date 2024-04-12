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

    [SerializeField]
    /// <summary>
    /// 아이템 갯수
    /// </summary>
    private int count = 0;
    public int Count
    {
        get => count;
        set
        {
            if(count != value)
            {
                count = value;
                SetCount(count);
            }
        }
    }

    public bool FullCount => Count >= item.maxItemCount;

    /// <summary>
    /// 드래그시 부모 개체
    /// </summary>
    private Transform DragParent => GameManager.Instance.inven.DragParent;

    RectTransform rect; 
    CanvasGroup canvas;

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

        item = data;
        ItemSize = item.Size;
        Count = _count;
        isDragging = false;

        rect = GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ItemSize.x * slotSize);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ItemSize.y * slotSize);
        itemIcon.sprite = data.itemIcon;

        canvas = GetComponent<CanvasGroup>();
        canvas.blocksRaycasts = false;
    }

    private void Start()
    {
        //ContainInitialize(item);
    }

    private void Update()
    {
        if(isDragging)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void StoreContain(Transform parent, Vector2 position)
    {
        transform.SetParent(parent);
        rect.pivot = Vector2.zero;
        transform.position = position;
        canvas.alpha = 1.0f;
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
        canvas.blocksRaycasts = !isDragging;
        canvas.alpha = 0.5f;
    }

    public int ItemStack(int _count = 1)
    {
        Count += _count;

        //Debug.Log(item.maxItemCount);

        if (Count > item.maxItemCount)
        {
            int result = Count - item.maxItemCount;
            Count = item.maxItemCount; 
            return result;
        }
        else
        {
            return 0;
        }
    }

    public void ItemDestack(int _count = 1)
    {
        Count -= _count;
    }

    public GameObject ItemSplit(int _count = 1)
    {
        ItemDestack(_count);

        return Factory.Instance.GetItemContain(item, _count);
    }

    public void ContainRemvoe()
    {
        Debug.Log("삭제");
        // 아이템 정보 삭제
        ResetSelectedItem();

        transform.SetParent(Factory.Instance.containChild);

        // 게임 오브젝트 비활성화
        gameObject.SetActive(false);
    }

    private void SetCount(int _count)
    {
        //Debug.Log(_count);
        itemCount.text = _count.ToString();
    }

    public void Grab()
    {
        rect.pivot = new Vector2(0.5f, 0.5f);
        canvas.alpha = 0.5f;
        transform.position = Input.mousePosition;
    }
}
