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

    [HideInInspector]
    /// <summary>
    /// 아이템 컨테이너를 담는 부모 개체
    /// </summary>
    public Transform DropParent;

    [HideInInspector]
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
            bool isShiftPress = (Keyboard.current.shiftKey.ReadValue() > 0);

            if (highlightedSlot != null && containGrab != null && !isOverEdge)
            {
                // 마우스가 슬롯 위에 있다 / 마우스에 잡힌 컨테이너가 있다 / 
                ItemContain contain = containGrab.GetComponent<ItemContain>();
                Debug.Log(checkSatae);
                switch (checkSatae)
                {
                    case 0:
                        // 빈 슬롯에 저장
                        if (isShiftPress && contain.Count > 1)
                        {
                            StoreItem(contain.ItemSplit(), totalOffset);
                            ColorChangeLoop(SlotColorHighlights.Blue, contain.ItemSize, totalOffset);
                        }
                        else
                        {
                            StoreItem(containGrab, totalOffset);
                            ColorChangeLoop(SlotColorHighlights.Blue, contain.ItemSize, totalOffset);
                            contain.ResetSelectedItem();
                        }
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
                        if (isShiftPress && contain.Count > 1)
                        {
                            Debug.Log("같은 아이템 하나씩 저장");
                            contain.ItemDestack();
                            AddContain(containGrab, 1);
                            ColorChangeLoop(SlotColorHighlights.White, otherItemSize, otherItemPos);
                        }
                        else
                        {
                            Debug.Log("같은 아이템이므로 저장");
                            AddContain(containGrab, contain.Count);
                            ColorChangeLoop(SlotColorHighlights.White, otherItemSize, otherItemPos);
                            contain.ContainRemvoe();
                        }
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

                tooltip.IsPause = true;

                GameObject containObj = GetItem(highlightedSlot);
                ItemContain contain = containObj.GetComponent<ItemContain>();

                if (isShiftPress && contain.Count > 1)
                {
                    int _count = Mathf.FloorToInt(contain.Count/2);
                    GameObject returnObj = contain.ItemSplit(_count);
                    SetSelectedItem(GrabContain(returnObj));
                    StoreItem(containObj, totalOffset);
                }
                else
                {
                    SetSelectedItem(GrabContain(containObj));
                }
                SlotSector.Instance.SetPosOffset();
                RefrechColor(true);
            }
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
                    InvenSlot instance = null;
                    if(StartPos.x + x < _horizontalSlotCount && StartPos.y + y < _verticalSlotCount)
                        instance = slotGrid[StartPos.x + x, StartPos.y + y].GetComponent<InvenSlot>();
                    else return 4;

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
                instance.SlotStore(item, startPos);
                slotGrid[totalOffset.x + x, totalOffset.y + y].GetComponent<Image>().color = SlotColorHighlights.White;
            }
        }

        Debug.Log(containList);
        TotalWeight += contain.item.itemWeight;
        containList.Add(contain);

        // 게임 오브젝트 재설정
        Vector2 position = slotGrid[startPos.x, startPos.y].transform.position;
        contain.StoreContain(DropParent, position);
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

        for (int i = 0; i < containList.Count; i++)
        {
            if (containList[i].id == returnItemContain.GetComponent<ItemContain>().id)
            {
                containList.RemoveAt(i);
            }
        }

        for (int y = 0; y < itemSize.y; y++)
        {
            for (int x = 0; x < itemSize.x; x++)
            {
                // 인벤 슬롯에 정보 삭제
                InvenSlot instance = slotGrid[tempItemPos.x + x, tempItemPos.y + y].GetComponent<InvenSlot>();
                containGrab = null;
                StackStartPos = instance.storedItemStartPos;
                instance.SlotRemove();
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
    public void AddContain(GameObject obj, int _count = 0)
    {
        GameObject storeObj = GetItem(slotGrid[otherItemPos.x, otherItemPos.y]);

        // 들어간 컨테이너
        ItemContain InContain = obj.GetComponent<ItemContain>();

        /// 저장된 컨테이너
        ItemContain storeContain = storeObj.GetComponent<ItemContain>();

        int remainCount = storeContain.ItemStack(_count);
        
        if(remainCount > 0)
        {
            InContain.Count = remainCount;
            containGrab = GrabContain(obj); 
            StoreItem(storeContain.gameObject, otherItemPos);
        }
        else if(remainCount == 0)
        {
            StoreItem(storeContain.gameObject, otherItemPos);
        }
    }

    public Action<ItemData, int> onDontGetItem;

    public void GetItemToSlot(ItemData data, int _count)
    {
        List<Vector2Int> emptyList = new List<Vector2Int>();
        List<ItemContain> sameItemContainList = new List<ItemContain>();
        
        // 탐색을 하고
        for (int y = 0; y < _verticalSlotCount; y++)
        {
            for (int x = 0; x < _horizontalSlotCount; x++)
            {
                InvenSlot instance = slotGrid[x, y].GetComponent<InvenSlot>();

                if (instance.isEmpty)
                {
                    int state = SlotCheck(instance.gridPos, data.Size);

                    if(state == 0)
                    {
                        emptyList.Add(new Vector2Int(x, y));
                    }
                }
                else
                {
                    foreach(var contain in containList)
                    {
                        if(contain.item == data && !contain.FullCount)
                        {
                            sameItemContainList.Add(contain);
                        }
                    }
                }
            }
        }

        int remain = _count;

        // 넣는다.
        if (sameItemContainList.Count > 0)
        {
            Debug.Log("같은 아이템이 있다.");
            foreach (var item in sameItemContainList)
            {
                remain = item.ItemStack(remain);
                if (remain > 0)
                    break;
            }
        }
        if(emptyList.Count > 0 && remain > 0)
        {
            foreach(var grid in emptyList)
            {
                GameObject obj = null;
                // 데이터의 최대 수량보다 크며
                if (_count > data.maxItemCount)
                {
                    remain -= data.maxItemCount;
                    obj = Factory.Instance.GetItemContain(data, data.maxItemCount);
                    if (SlotCheck(grid, data.Size) == 0)
                    {
                        StoreItem(obj, grid);
                        break;
                    }
                }
                else
                {
                    obj = Factory.Instance.GetItemContain(data, remain);
                    if (SlotCheck(grid, data.Size) == 0)
                    {
                        StoreItem(obj, grid);
                        break;
                    }
                }
            }
        }

        sameItemContainList.Clear();
        emptyList.Clear();
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