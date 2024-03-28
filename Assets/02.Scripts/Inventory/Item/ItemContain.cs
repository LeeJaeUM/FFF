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
    /// ������ ����
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
    /// �巡�׽� �θ� ��ü
    /// </summary>
    private Transform DragParent => GameManager.Instance.inven.DragParent;

    private void Awake()
    {
        itemIcon = transform.GetChild(1).GetComponent<Image>();
        itemCount = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        //Debug.Log(item.Size);
    }

    /// <summary>
    /// �����̳� ���� �Լ�
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
    /// �����̳ʿ� ������ ���� �ֱ�
    /// </summary>
    /// <param name="_data">�� ������ ����</param>
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
    /// ���õ� ������ �����̳�
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
    /// ������ ���׾�� ���� �����
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
        // ������ ���� ����
        ResetSelectedItem();

        transform.SetParent(Factory.Instance.containChild);

        // ���� ������Ʈ ��Ȱ��ȭ
        gameObject.SetActive(false);
    }
}
