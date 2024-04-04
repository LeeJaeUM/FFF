using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;
using Unity.VisualScripting;
using System.ComponentModel;

public class InventoryUI : MonoBehaviour
{
    /// <summary>
    /// 아이템 정보를 담는 컨테이너 리스트
    /// </summary>
    public List<ItemContain> containList;

    /// <summary>
    /// 슬롯 그리드를 저장하는 객체
    /// </summary>
    public GameObject[,] slotGrid;

    [HideInInspector]
    /// <summary>
    /// 그리드 사이즈
    /// </summary>
    public Vector2Int gridSize => new Vector2Int(_horizontalSlotCount, _verticalSlotCount);

    /// <summary>
    /// 현재 잡힌 아이템 컨테이너의 현재 좌표, 사이즈, 시작 좌표
    /// </summary>
    private Vector2Int totalOffset, checkSize, checkStartPos;

    /// <summary>
    /// 슬롯에 있는 정보 저장
    /// </summary>
    private Vector2Int otherItemPos, otherItemSize;

    /// <summary>
    /// 같은 아이템 스탯 증가시 사용하는 변수
    /// </summary>
    private Vector2Int StackStartPos;

    private int checkSatae;
    private bool isOverEdge = false;

    /// <summary>
    /// 아이템 컨테이너를 담는 부모 개체
    /// </summary>
    public Transform DropParent;

    /// <summary>
    /// 현재 손에 있는 아이템 컨테어너의 부모 개체
    /// </summary>
    public Transform DragParent;

    public InvenGrid invenGrid;

    CanvasGroup canvasGroup;

    /// <summary>
    /// 현재 손에 있는 아이템 컨테이너
    /// </summary>
    public GameObject containGrab = null;

    /// <summary>
    /// 현재 마우스가 있는 슬롯
    /// </summary>
    public GameObject highlightedSlot;

    [Header("옵션")]

    [Range(5, 8)]
    public int _horizontalSlotCount = 5;

    [Range(2, 10)]
    public int _verticalSlotCount = 5;

    /// <summary>
    /// 슬롯의 크기
    /// </summary>
    public float slotSize = 64;

    /// <summary>
    /// 슬롯 사이의 거리
    /// </summary>
    public float edgePadding = 8;

    public ItemData[] itemDatas = null;

    public Action<float> onWeightChange;

    /// <summary>
    /// 인벤토리 무게
    /// </summary>
    private float weight;

    public float TotalWeight
    {
        get => weight;
        set
        {
            if(weight != value)
            {
                weight = value;
                onWeightChange?.Invoke(weight);
            }
        }
    }

    /// <summary>
    /// 아이템 내용을 나타내는 클래스
    /// </summary>
    public ItemTooltip tooltip;

    private InvenUI UI;

    /// <summary>
    /// 입력
    /// </summary>
    MouseInputAction inputAction;

    private void Awake()
    {
        inputAction = new MouseInputAction();
        canvasGroup = GetComponent<CanvasGroup>();

        Transform child = transform.GetChild(0);
        invenGrid = child.GetComponentInChildren<InvenGrid>();
        child = child.GetChild(3);
        DropParent = child.GetChild(0);
        DragParent = child.GetChild(1);
        tooltip = GetComponentInChildren<ItemTooltip>();
        UI = GetComponentInChildren<InvenUI>();
    }

    private void Start()
    {
        TotalWeight = 0;
        UI.Initialized();
        invenGrid.GridInitialize();
    }

    private void OnEnable()
    {
        inputAction.UI.Enable();
        inputAction.UI.LClick.performed += OnLeftClick;
    }

    private void OnDisable()
    {
        inputAction.UI.LClick.performed -= OnLeftClick;
        inputAction.UI.Disable();
    }

    /// <summary>
    /// 좌 클릭 이벤트
    /// </summary>
    /// <param name="context">클릭이벤트</param>
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
                        // 빈 슬롯에 저장
                        StoreItem(containGrab, totalOffset);
                        ColorChangeLoop(SlotColorHighlights.Blue, contain.ItemSize, totalOffset);
                        contain.ResetSelectedItem();
                        RemoveSelectedButton();
                        break;
                    case 1:
                        // 다른 아이템 스왑
                        SetSelectedItem(SwapItem(containGrab));
                        SlotSector.Instance.SetPosOffset();
                        ColorChangeLoop(SlotColorHighlights.White, otherItemSize, otherItemPos);
                        RefrechColor(true);
                        break;
                    case 3:
                        // 같은 아이템이면 저장
                        AddContain(containGrab);
                        ColorChangeLoop(SlotColorHighlights.White, otherItemSize, otherItemPos);
                        break;
                }
                tooltip.IsPause = false;
            }
            else if (highlightedSlot != null && containGrab == null
                && !highlightedSlot.GetComponent<InvenSlot>().isEmpty)
            {
                InvenSlot highlightedSlotScript = highlightedSlot.GetComponent<InvenSlot>();
                ColorChangeLoop(SlotColorHighlights.White,
                    highlightedSlotScript.storedItemSize, highlightedSlotScript.storedItemStartPos);
                SetSelectedItem(GrabContain(GetItem(highlightedSlot)));
                SlotSector.Instance.SetPosOffset();
                RefrechColor(true);

                tooltip.IsPause = true;
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
    /// 영역 안 컨테이너 체크
    /// </summary>
    /// <param name="itemSize">아이템 크기</param>
    private void CheckArea(Vector2Int itemSize)
    {
        // 아이템 컨테이너의 가운데 위치
        Vector2Int halfOffset = Vector2Int.zero;
        halfOffset.x = (itemSize.x - (itemSize.x % 2 == 0 ? 0 : 1)) / 2;
        halfOffset.y = (itemSize.y - (itemSize.y % 2 == 0 ? 0 : 1)) / 2;

        // 아이템 컨테이너의 시작 위치 = 마우스 위치 그리드 - (가운데 위치 + 슬롯의 제 4분면위치)
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
    
    /// <summary>
    /// 슬롯 문제 해결
    /// </summary>
    /// <param name="itemSize">아이템 사이즈</param>
    /// <returns>0: 빈 슬롯, 1: 아이템이 들어간 슬롯</returns>
    private int SlotCheck(Vector2Int StartPos, Vector2Int itemSize)
    {
        GameObject SlotObj = null;
        ItemContain SlotContain = null;
        if (!isOverEdge)
        {
            for (int y = 0; y < itemSize.y; y++)
            {
                for (int x = 0; x < itemSize.x; x++)
                {
                    InvenSlot instance = slotGrid[StartPos.x + x, StartPos.y + y].GetComponent<InvenSlot>();

                    if (!instance.isEmpty)  // slot이 비어있지 않으며 
                    {
                        if (SlotObj == null)
                        {
                            // 슬롯에 담긴 컨테이너 정보 추출
                            SlotObj = instance.storedItemObject;
                            otherItemPos = instance.storedItemStartPos;
                            SlotContain = SlotObj.GetComponent<ItemContain>();
                            otherItemSize = SlotContain.item.Size;
                        }
                        else if(containGrab != null)
                        {
                            if (SlotObj != instance.storedItemObject && instance.data != containGrab.GetComponent<ItemContain>().item)
                                // 슬롯 오브젝트와 저장된 아이템하고 다른때, 아이템 정보가 서로 다른때
                                return 2;
                            else if (instance.data == containGrab.GetComponent<ItemContain>().item)
                                // 저장된 아이템이 서로 같을때
                                return 3;
                        }
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
            checkSatae = SlotCheck(checkStartPos, checkSize);
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
    /// 색깔이 달라지는 컬러
    /// </summary>
    /// <param name="color">변할 컬러</param>
    /// <param name="size">크기</param>
    /// <param name="startPos">시작 좌표</param>
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
    /// 색깔이 달라지는 컬러
    /// </summary>
    /// <param name="size">크기</param>
    /// <param name="startPos">시작 좌표</param>
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

    /// <summary>
    /// 슬롯에 아이템 저장
    /// </summary>
    /// <param name="item">들어갈 아이템 오브젝트</param>
    /// <param name="isStack"></param>
    private void StoreItem(GameObject item, Vector2Int startPos)
    {
        ItemContain contain = item.GetComponent<ItemContain>();
        contain.isDragging = false;
        Vector2Int itemSize = contain.item.Size;

        for (int y = 0; y < itemSize.y; y++)
        {
            for (int x = 0; x < itemSize.x; x++)
            {
                // 인벤 슬롯에 정보 저장
                InvenSlot instance = slotGrid[startPos.x + x, startPos.y + y].GetComponent<InvenSlot>();
                instance.storedItemObject = item;                           // 오브젝트 저장
                instance.data = item.GetComponent<ItemContain>().item;      // 아이템 정보 저장
                instance.storedItemSize = itemSize;                         // 사이즈 저장
                instance.storedItemStartPos = startPos;
                instance.isEmpty = false;
                slotGrid[totalOffset.x + x, totalOffset.y + y].GetComponent<Image>().color = SlotColorHighlights.White;
            }
        }

        contain.id = containList.Count;
        TotalWeight += contain.item.itemWeight;
        containList.Add(contain);
        // 게임 오브젝트 재설정
        item.transform.SetParent(DropParent);
        item.GetComponent<RectTransform>().pivot = Vector2.zero;
        item.transform.position = slotGrid[startPos.x, startPos.y].transform.position;
        item.GetComponent<CanvasGroup>().alpha = 1.0f;
        //tooltip.Open(highlightedSlot.GetComponent<InvenSlot>().data);
    }

    /// <summary>
    /// 게임 오브젝트 얻는 함수
    /// </summary>
    /// <param name="slotObject">슬롯에 저장된 아이템 컨테이너 정보</param>
    /// <returns></returns>
    private GameObject GetItem(GameObject slotObject)
    {
        StackStartPos = Vector2Int.zero;
        InvenSlot invenSlot = slotObject.GetComponent<InvenSlot>();
        Vector2Int tempItemPos = invenSlot.storedItemStartPos;

        GameObject returnItem = invenSlot.storedItemObject;
        ItemContain returnItemContain = returnItem.GetComponent<ItemContain>();
        Vector2Int itemSize = returnItemContain.item.Size;
        TotalWeight -= returnItemContain.item.itemWeight;
        containList.RemoveAt(returnItemContain.id);

        for (int y = 0; y < itemSize.y; y++)
        {
            for (int x = 0; x < itemSize.x; x++)
            {
                // 인벤 슬롯에 정보 삭제
                InvenSlot instance = slotGrid[tempItemPos.x + x, tempItemPos.y + y].GetComponent<InvenSlot>();
                containGrab = null;
                StackStartPos = instance.storedItemStartPos;
                instance.storedItemSize = Vector2Int.zero;
                instance.storedItemStartPos = Vector2Int.zero;
                instance.data = null;
                instance.isEmpty = true;

            }
        }

        return returnItem;
    }

    /// <summary>
    /// 아이템 컨테이너 잡기
    /// </summary>
    /// <param name="invenSlot"></param>
    /// <returns></returns>
    public GameObject GrabContain(GameObject invenSlot)
    {

        invenSlot.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        invenSlot.GetComponent<CanvasGroup>().alpha = 0.5f;
        invenSlot.transform.position = Input.mousePosition;

        //tooltip.Close();

        return invenSlot;
    }

    private GameObject SwapItem(GameObject item)
    {
        GameObject tempItem = GrabContain(GetItem(slotGrid[otherItemPos.x, otherItemPos.y]));
        StoreItem(item, totalOffset);
        return tempItem;
    }

    /// <summary>
    /// 같은 아이템을 경우 갯수 증가
    /// </summary>
    public void AddContain(GameObject obj)
    {
        GameObject storeObj = GetItem(slotGrid[otherItemPos.x, otherItemPos.y]);

        // 들어간 컨테이너
        ItemContain InContain = obj.GetComponent<ItemContain>();

        /// 저장된 컨테이너
        ItemContain Outcontain = storeObj.GetComponent<ItemContain>();
        
        int remainCount = Outcontain.ItemStack(InContain.GetComponent<ItemContain>().Count);
        Debug.Log(StackStartPos);
        Debug.Log(otherItemPos);
        Debug.Log(remainCount);
        
        if (remainCount == 0)
        {
            StoreItem(Outcontain.gameObject, otherItemPos);
            InContain.ContainRemvoe();  // 컨테이너 삭제
        }
        else
        {
            InContain.Count = remainCount;
            InContain.SetCount();
            containGrab = GrabContain(obj);
        }
        StoreItem(Outcontain.gameObject, otherItemPos);
    }

    public Action<ItemData, int> onDontGetItem;

    public void GetItemToSlot(ItemData data, int _count)
    {
        for (int y = 0; y < _verticalSlotCount; y++)
        {
            for (int x = 0; x < _horizontalSlotCount; x++)
            {
                InvenSlot instance = slotGrid[x, y].GetComponent<InvenSlot>();

                int remainCount = 0;
                if (!instance.isEmpty && instance.data == data && remainCount == 0)
                {
                    ItemContain contain = instance.storedItemObject.GetComponent<ItemContain>();
                    remainCount = contain.ItemStack(_count);

                    if (remainCount == 0)
                    {
                        return;
                    }
                }
                // 슬롯이 비어 있다.
                else if (instance.isEmpty)
                {
                    int state = SlotCheck(instance.gridPos, data.Size);

                    if(state == 0)
                    {
                        GameObject obj = Factory.Instance.GetItemContain(data, _count);
                        StoreItem(obj, instance.gridPos);
                        return;
                    }
                }

            }
        }
    }


    /// <summary>
    /// 컨테이너 제거
    /// </summary>
    public void RemoveSelectedButton()
    {
        containGrab = null;
    }

    /// <summary>
    /// 인벤토리를 키는 함수
    /// </summary>
    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// 인벤토리를 끝는 함수
    /// </summary>
    public void Close()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0;
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