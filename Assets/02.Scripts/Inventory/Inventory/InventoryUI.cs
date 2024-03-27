using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    /// <summary>
    /// ������ ������ ��� �����̳� ����Ʈ
    /// </summary>
    public List<ItemContain> containList;

    /// <summary>
    /// ���� �׸��带 �����ϴ� ��ü
    /// </summary>
    public GameObject[,] slotGrid;

    [HideInInspector]
    /// <summary>
    /// �׸��� ������
    /// </summary>
    public Vector2Int gridSize => new Vector2Int(_horizontalSlotCount, _verticalSlotCount);

    /// <summary>
    /// ���� ���� ������ �����̳��� ���� ��ǥ, ������, ���� ��ǥ
    /// </summary>
    private Vector2Int totalOffset, checkSize, checkStartPos;

    /// <summary>
    /// ���Կ� �ִ� ���� ����
    /// </summary>
    private Vector2Int otherItemPos, otherItemSize;

    /// <summary>
    /// ���� ������ ���� ������ ����ϴ� ����
    /// </summary>
    private Vector2Int StackStartPos;

    private int checkSatae;
    private bool isOverEdge = false;

    /// <summary>
    /// ������ �����̳ʸ� ��� �θ� ��ü
    /// </summary>
    public Transform DropParent;

    /// <summary>
    /// ���� �տ� �ִ� ������ ���׾���� �θ� ��ü
    /// </summary>
    public Transform DragParent;

    public InvenGrid invenGrid;

    /// <summary>
    /// ������ ������ ��Ÿ���� Ŭ����
    /// </summary>
    public ItemTooltip tooltip;

    /// <summary>
    /// ���� �տ� �ִ� ������ �����̳�
    /// </summary>
    public GameObject containGrab = null;

    /// <summary>
    /// ���� ���콺�� �ִ� ����
    /// </summary>
    public GameObject highlightedSlot;

    [Header("�ɼ�")]
    private int _horizontalSlotCount = 8;

    [Range(1, 10)]
    public int _verticalSlotCount = 5;

    /// <summary>
    /// ������ ũ��
    /// </summary>
    public float slotSize = 64;

    /// <summary>
    /// ���� ������ �Ÿ�
    /// </summary>
    public float edgePadding = 8;

    public ItemData[] itemDatas = null;

    PlayerInputAction inputAction;

    private void Awake()
    {
        inputAction = new PlayerInputAction();

        Transform child = transform.GetChild(0);
        invenGrid = child.GetComponentInChildren<InvenGrid>();
        child = transform.GetChild(1);
        DropParent = child.GetChild(0);
        DragParent = child.GetChild(1);
    }

    private void Start()
    { 
        invenGrid.GridInitialize();
    }

    private void OnEnable()
    {
        inputAction.UI.Enable();
        inputAction.UI.onLeftClick.performed += OnLeftClick;
    }

    private void OnDisable()
    {
        inputAction.UI.onLeftClick.performed -= OnLeftClick;
        inputAction.UI.Disable();
    }

    /// <summary>
    /// �� Ŭ�� �̺�Ʈ
    /// </summary>
    /// <param name="context">Ŭ���̺�Ʈ</param>
    private void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (highlightedSlot != null && containGrab != null && !isOverEdge)
            {
                ItemContain contain = containGrab.GetComponent<ItemContain>();
                Debug.Log(checkSatae);
                switch (checkSatae)
                {
                    case 0:
                        StoreItem(containGrab);
                        ColorChangeLoop(SlotColorHighlights.Blue, contain.ItemSize, totalOffset);
                        contain.ResetSelectedItem();
                        RemoveSelectedButton();
                        break;
                    case 1:
                        SetSelectedItem(SwapItem(containGrab));
                        SlotSector.Instance.SetPosOffset();
                        ColorChangeLoop(SlotColorHighlights.White, otherItemSize, otherItemPos);
                        RefrechColor(true);
                        break;
                    case 3:
                        AddContain(containGrab);
                        ColorChangeLoop(SlotColorHighlights.White, contain.ItemSize, highlightedSlot.GetComponent<InvenSlot>().gridPos);
                        ColorChangeLoop(SlotColorHighlights.White, otherItemSize, otherItemPos);
                        break;
                }
            }
            else if (highlightedSlot != null && containGrab == null
                && !highlightedSlot.GetComponent<InvenSlot>().isEmpty)
            {
                InvenSlot highlightedSlotScript = highlightedSlot.GetComponent<InvenSlot>();
                ColorChangeLoop(SlotColorHighlights.White,
                    highlightedSlotScript.storedItemSize, highlightedSlotScript.storedItemStartPos);
                SetSelectedItem(GetItem(highlightedSlot));
                SlotSector.Instance.SetPosOffset();
                RefrechColor(true);
            }
            //Debug.Log(highlightedSlot != null && ItemContain.selectedItem == null);
            //if (highlightedSlot != null)
            //{
            //    Debug.Log(highlightedSlot.GetComponent<InvenSlot>().isOccupied == true);
            //    Debug.Log(highlightedSlot != null && ItemContain.selectedItem == null
            //    && highlightedSlot.GetComponent<InvenSlot>().isOccupied == true);
            //}
        }
    }

    public void SetSelectedItem(GameObject obj)
    {
        containGrab = obj;
        ItemContain contain = containGrab.GetComponent<ItemContain>();
        contain.isDragging = true;
        containGrab.transform.SetParent(DragParent);
        obj.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    /// <summary>
    /// ���� �� �����̳� üũ
    /// </summary>
    /// <param name="itemSize">������ ũ��</param>
    private void CheckArea(Vector2Int itemSize)
    {
        Vector2Int halfOffset = Vector2Int.zero;
        halfOffset.x = (itemSize.x - (itemSize.x % 2 == 0 ? 0 : 1)) / 2;
        halfOffset.y = (itemSize.y - (itemSize.y % 2 == 0 ? 0 : 1)) / 2;

        totalOffset = highlightedSlot.GetComponent<InvenSlot>().gridPos - (halfOffset + SlotSector.posOffset);

        checkStartPos = totalOffset;
        checkSize = itemSize;
        Vector2Int overCheck = totalOffset + itemSize;
        isOverEdge = false;

        if (overCheck.x > gridSize.x)
        {
            checkSize.x = gridSize.x - totalOffset.x;
            isOverEdge = true;
        }
        if (totalOffset.x < 0)
        {
            checkSize.x = itemSize.x + totalOffset.x;
            checkStartPos.x = 0;
            isOverEdge = true;
        }
        if (overCheck.y > gridSize.y)
        {
            checkSize.y = gridSize.y - totalOffset.y;
            isOverEdge = true;
        }
        if (totalOffset.y < 0)
        {
            checkSize.y = itemSize.y + totalOffset.y;
            checkStartPos.y = 0;
            isOverEdge = true;
        }
    }

    private int SlotCheck(Vector2Int itemSize)
    {
        GameObject SlotObj = null;
        if (!isOverEdge)
        {
            for (int y = 0; y < itemSize.y; y++)
            {
                for (int x = 0; x < itemSize.x; x++)
                {
                    InvenSlot instance = slotGrid[checkStartPos.x + x, checkStartPos.y + y].GetComponent<InvenSlot>();

                    if (!instance.isEmpty)  // slot�� ������� ������ 
                    {
                        if (SlotObj == null)
                        {
                            SlotObj = instance.storedItemObject;
                            otherItemPos = instance.storedItemStartPos;
                            otherItemSize = SlotObj.GetComponent<ItemContain>().item.Size;
                        }
                        else if (SlotObj != instance.storedItemObject && instance.data != containGrab.GetComponent<ItemContain>().item)
                            return 2;
                        else if (instance.data == containGrab.GetComponent<ItemContain>().item)
                            return 3;
                    }
                }
            }
            if (SlotObj == null)
                return 0;
            else
                return 1;
        }
        return 2;
    }


    public void RefrechColor(bool enter)
    {
        if (enter)
        {
            CheckArea(containGrab.GetComponent<ItemContain>().ItemSize);
            checkSatae = SlotCheck(checkSize);
            switch (checkSatae)
            {
                case 0:
                    ColorChangeLoop(SlotColorHighlights.Green, checkSize, checkStartPos);
                    break;
                case 1:
                    ColorChangeLoop(SlotColorHighlights.Yellow, otherItemSize, otherItemPos);
                    ColorChangeLoop(SlotColorHighlights.Green, checkSize, checkStartPos);
                    break;
                case 2:
                    ColorChangeLoop(SlotColorHighlights.Red, checkSize, checkStartPos);
                    break;
                case 3:
                    //ColorChangeLoop(SlotColorHighlights.Blue3, checkSize, checkStartPos);
                    ColorChangeLoop(SlotColorHighlights.Blue3, otherItemSize, otherItemPos);
                    break;
            }
        }
        else
        {
            isOverEdge = false;

            ColorChangeLoop(checkSize, checkStartPos);
            if (checkSatae == 1)
            {
                ColorChangeLoop(SlotColorHighlights.Blue2, otherItemSize, otherItemPos);
            }
        }
    }

    /// <summary>
    /// ������ �޶����� �÷�
    /// </summary>
    /// <param name="color">���� �÷�</param>
    /// <param name="size">ũ��</param>
    /// <param name="startPos">���� ��ǥ</param>
    public void ColorChangeLoop(Color color, Vector2Int size, Vector2Int startPos)
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                slotGrid[startPos.x + x, startPos.y + y].GetComponent<Image>().color = color;
            }
        }
    }

    /// <summary>
    /// ������ �޶����� �÷�
    /// </summary>
    /// <param name="size">ũ��</param>
    /// <param name="startPos">���� ��ǥ</param>
    public void ColorChangeLoop(Vector2Int size, Vector2Int startPos)
    {
        GameObject slot;
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                slot = slotGrid[startPos.x + x, startPos.y + y];
                if (!slot.GetComponent<InvenSlot>().isEmpty)
                {
                    slotGrid[startPos.x + x, startPos.y + y].GetComponent<Image>().color = SlotColorHighlights.Blue2;
                }
                else
                {
                    slotGrid[startPos.x + x, startPos.y + y].GetComponent<Image>().color = SlotColorHighlights.White;
                }
            }
        }
    }

    private void StoreItem(GameObject item, bool isStack = false)
    {
        ItemContain contain = item.GetComponent<ItemContain>();
        contain.isDragging = false;
        Vector2Int itemSize = contain.item.Size;

        Vector2Int startPos = totalOffset;

        if (isStack)
            startPos = otherItemPos;

        for (int y = 0; y < itemSize.y; y++)
        {
            for (int x = 0; x < itemSize.x; x++)
            {
                // �κ� ���Կ� ���� ����
                InvenSlot instance = slotGrid[startPos.x + x, startPos.y + y].GetComponent<InvenSlot>();
                instance.storedItemObject = item;                           // ������Ʈ ����
                instance.data = item.GetComponent<ItemContain>().item;      // ������ ���� ����
                instance.storedItemSize = itemSize;                         // ������ ����
                instance.storedItemStartPos = startPos;
                instance.isEmpty = false;
                slotGrid[totalOffset.x + x, totalOffset.y + y].GetComponent<Image>().color = SlotColorHighlights.White;
            }
        }

        contain.id = containList.Count;
        containList.Add(contain);
        // ���� ������Ʈ �缳��
        item.transform.SetParent(DropParent);
        item.GetComponent<RectTransform>().pivot = Vector2.zero;
        item.transform.position = slotGrid[startPos.x, startPos.y].transform.position;
        item.GetComponent<CanvasGroup>().alpha = 1.0f;
        //tooltip.Open(highlightedSlot.GetComponent<InvenSlot>().data);
    }

    /// <summary>
    /// ���� ������Ʈ ��� �Լ�
    /// </summary>
    /// <param name="slotObject">���Կ� ����� ������ �����̳� ����</param>
    /// <returns></returns>
    private GameObject GetItem(GameObject slotObject)
    {
        StackStartPos = Vector2Int.zero;
        InvenSlot invenSlot = slotObject.GetComponent<InvenSlot>();
        Vector2Int tempItemPos = invenSlot.storedItemStartPos;

        GameObject returnItem = invenSlot.storedItemObject;
        Vector2Int itemSize = returnItem.GetComponent<ItemContain>().item.Size;

        containList.RemoveAt(returnItem.GetComponent<ItemContain>().id);

        for (int y = 0; y < itemSize.y; y++)
        {
            for (int x = 0; x < itemSize.x; x++)
            {
                // �κ� ���Կ� ���� ����
                InvenSlot instance = slotGrid[tempItemPos.x + x, tempItemPos.y + y].GetComponent<InvenSlot>();
                containGrab = null;
                StackStartPos = instance.storedItemStartPos;
                instance.storedItemSize = Vector2Int.zero;
                instance.storedItemStartPos = Vector2Int.zero;
                instance.data = null;
                instance.isEmpty = true;

            }
        }
        returnItem.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        returnItem.GetComponent<CanvasGroup>().alpha = 0.5f;
        returnItem.transform.position = Input.mousePosition;

        //tooltip.Close();

        return returnItem;
    }

    private GameObject SwapItem(GameObject item)
    {
        GameObject tempItem = GetItem(slotGrid[otherItemPos.x, otherItemPos.y]);
        StoreItem(item);
        return tempItem;
    }

    /// <summary>
    /// ���� �������� ��� ���� ����
    /// </summary>
    public void AddContain(GameObject obj)
    {
        GameObject storeObj = GetItem(slotGrid[otherItemPos.x, otherItemPos.y]);

        ItemContain InContain = obj.GetComponent<ItemContain>();
        ItemContain Outcontain = storeObj.GetComponent<ItemContain>();
        
        Outcontain.ItemStack(InContain.GetComponent<ItemContain>().Count);
        StoreItem(Outcontain.gameObject, true);
        Debug.Log(StackStartPos);
        Debug.Log(otherItemPos);
        InContain.ContainRemvoe();
    }

    /// <summary>
    /// �����̳� ����
    /// </summary>
    public void RemoveSelectedButton()
    {
        containGrab = null;
    }
}

public struct SlotColorHighlights
{
    public static Color Green
    { get { return new Color32(127, 223, 127, 255); } }
    public static Color Yellow
    { get { return new Color32(223, 223, 63, 255); } }
    public static Color Red
    { get { return new Color32(223, 127, 127, 255); } }
    public static Color Blue
    { get { return new Color32(159, 159, 223, 255); } }
    public static Color Blue2
    { get { return new Color32(191, 191, 223, 255); } }
    public static Color White
    { get { return new Color32(255, 255, 255, 255); } }
    public static Color Blue3
    {
        get { return new Color32(100, 160, 255, 255); }
    }
}