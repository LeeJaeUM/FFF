using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemContain : RecycleObject, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
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
                //if(count <= 0)
                //{
                //    ContainRemvoe();
                //}
            }
        }
    }

    public bool FullCount => Count >= item.maxItemCount;

    /// <summary>
    /// 드래그시 부모 개체
    /// </summary>
    private Transform DragParent => GameManager.Instance.inven.DragParent;

    private bool isGrab;

    public List<InvenSlot> storeSlots;

    RectTransform rect; 
    CanvasGroup canvas;

    private void Awake()
    {
        itemIcon = transform.GetChild(1).GetComponent<Image>();
        itemCount = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        //Debug.Log(item.Size);
    }

    protected override void OnDisable()
    {
        item = null;

        base.OnDisable();
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
        storeSlots = new List<InvenSlot>((ItemSize.x + 1) * (ItemSize.y + 1));

        rect = GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ItemSize.x * slotSize);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ItemSize.y * slotSize);
        itemIcon.sprite = data.itemIcon;

        canvas = GetComponent<CanvasGroup>();
        canvas.blocksRaycasts = false;
    }

    private void Update()
    {
        if(isDragging)
        {
            transform.position = Input.mousePosition;
        }
    }

    /// <summary>
    /// 저장시 동작하는 함수
    /// </summary>
    /// <param name="parent">저장시 오브젝트의 부모</param>
    /// <param name="position">저장할 위치</param>
    public void StoreContain(Transform parent, Vector2 position)
    {
        transform.SetParent(parent);
        transform.position = position;

        isDragging = false;

        rect.pivot = Vector2.zero;

        canvas.alpha = 1.0f;
        canvas.blocksRaycasts = true;

        foreach (InvenSlot slot in storeSlots)
        {
            Debug.Log(slot.name);
            slot.SlotStore(this);
        }
        
        SlotColorChange(SlotColorHighlights.White);
    }

    /// <summary>
    /// 선택된 아이템 컨테이너
    /// </summary>
    /// <param name="contain"></param>
    public ItemContain GrabContain()
    {
        isGrab = true;
        isDragging = true;
        transform.SetParent(DragParent);
        transform.position = Input.mousePosition;

        canvas.alpha = 0.5f;
        canvas.blocksRaycasts = false;

        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.localScale = Vector3.one;
        ResetContain();

        return this;
    }

    /// <summary>
    /// 아이템 컨테어너 내용 지우기
    /// </summary>
    public void ResetContain()
    {
        SlotColorChange(SlotColorHighlights.White);

        foreach (var slot in storeSlots)
        {
            slot.SlotRemove();
        }

        storeSlots.Clear();
        GameManager.Instance.inven.containGrab = null;
    }


    #region 컨테이너 아이템 갯수 변화
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

    public ItemContain ItemSplit(int _count = 1)
    {
        ItemDestack(_count);

        return Factory.Instance.GetItemContain(item, _count);
    }
    #endregion

    /// <summary>
    /// 컨테이너 삭제 함수
    /// </summary>
    public void ContainRemove()
    {
        Debug.Log("삭제");
        // 아이템 정보 삭제
        ResetContain();

        transform.SetParent(Factory.Instance.containChild);

        // 게임 오브젝트 비활성화
        gameObject.SetActive(false);
    }

    private void SetCount(int _count)
    {
        //Debug.Log(_count);
        itemCount.text = _count.ToString();
    }

    /// <summary>
    /// 아이템의 저장된 슬롯의 색을 바꾸는 함수
    /// </summary>
    /// <param name="color">바꿀 색깔</param>
    public void SlotColorChange(Color color)
    {
        foreach(var slot in storeSlots)
        {
            slot.GetComponent<Image>().color = color;
        }
    }

    public void StoreSlot(InvenSlot slot)
    {
        storeSlots.Add(slot);
    }

    public void RecastOn()
    {
        canvas.blocksRaycasts = true;
    }

    public void RecastOff()
    {
        canvas.blocksRaycasts = false;
    }

    #region UI 이벤트
    public void OnPointerClick(PointerEventData eventData)
    {
        //if (isGrab)
        //{
        //    isGrab = false;
        //    foreach (var slot in storeSlots)
        //    {
        //        slot.SlotRemove();
        //    }

        //    storeSlots.Clear();
        //    SlotColorChange(SlotColorHighlights.White);
        //}
        //else
        //{
        //    GrabContain();
        //    canvas.blocksRaycasts = !isDragging;
        //    canvas.alpha = 0.5f;

            
        //}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryUI inven = GameManager.Instance.inven;
        inven.enterContain = this;
        inven.tooltip.Open(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryUI inven = GameManager.Instance.inven;
        SlotColorChange(SlotColorHighlights.White);
        inven.enterContain = null;
        inven.tooltip.Close();
    }
    #endregion
}
