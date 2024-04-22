using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;
using Unity.VisualScripting;
using System.ComponentModel;
using Unity.Collections.LowLevel.Unsafe;

public class InventoryUI : MonoBehaviour
{
    /// <summary>
    /// 아이템 정보를 담는 컨테이너 리스트
    /// </summary>
    public ItemContainList[] containList;

    public Action onContainListChange;

    /// <summary>
    /// 슬롯 그리드를 저장하는 객체
    /// </summary>
    public InvenSlot[,] slotGrid;

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
    public ItemContain containGrab = null;

    public ItemContain enterContain = null;

    /// <summary>
    /// 현재 마우스가 있는 슬롯
    /// </summary>
    public InvenSlot highlightedSlot;

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

    [HideInInspector]
    /// <summary>
    /// 아이템 내용을 나타내는 클래스
    /// </summary>
    public ItemTooltip tooltip;

    public SlotSector slotSector;

    private InvenUI UI;

    private ProduceManager produceManager;

    /// <summary>
    /// 입력
    /// </summary>
    MouseInputAction inputAction;

    #region Awake(), start()
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
        produceManager = GetComponentInChildren<ProduceManager>();

        containList = new ItemContainList[itemDatas.Length];

        for(int i =  0; i < containList.Length; i++)
        {
            containList[i].itemCode = itemDatas[i].itemCode;
            containList[i].containList = new List<ItemContain>();
            containList[i].itemCount = 0;
        }
    }

    private void Start()
    {
        TotalWeight = 0;
        UI.Initialized();
        invenGrid.GridInitialize();
        produceManager.Initialize();
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
    #endregion

    /// <summary>
    /// 좌 클릭 이벤트
    /// </summary>
    /// <param name="context">클릭이벤트</param>
    private void OnLeftClick(InputAction.CallbackContext context)
    {
        /*
         * 리스트에서 삭제가 안 일어난...
         * 좌 클릭 이벤트 대규모 수정
         */
        if (context.performed)
        {
            bool isShiftPress = (Keyboard.current.shiftKey.ReadValue() > 0);

            if (highlightedSlot != null && containGrab != null && !isOverEdge)
            {
                // 마우스가 슬롯 위에 있다 / 마우스에 잡힌 컨테이너가 있다 / 
                ItemContain contain = containGrab.GetComponent<ItemContain>();
                Debug.Log(checkSatae);
                if(containGrab != null)
                {
                    if(highlightedSlot != null)
                    {
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
                            //contain.ResetContain();
                        }
                    }
                    else
                    {
                        if(enterContain.item == containGrab.item)
                        {
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
                        }
                        else
                        {
                            // 다른 아이템 스왑
                            SwapItem(containGrab).GrabContain();
                            slotSector.SetPosOffset(containGrab.ItemSize);
                            ColorChangeLoop(SlotColorHighlights.White, otherItemSize, otherItemPos);
                            RefrechColor(true);
                        }
                    }
                }
                tooltip.IsPause = false;
            }
            else if (highlightedSlot == null && containGrab == null
                && enterContain != null)
            {
                // 컨테이너의 저장된 아이템 색 바꾸기
                enterContain.SlotColorChange(SlotColorHighlights.White);

                tooltip.IsPause = true;

                if (isShiftPress && enterContain.Count > 1)
                {
                    int _count = Mathf.FloorToInt(enterContain.Count/2);
                    ItemContain returnContain = enterContain.ItemSplit(_count);
                    returnContain.GrabContain();
                }
                else
                {
                    containGrab = GrabContain(enterContain);
                }

                Debug.Log(containGrab);

                while (containGrab == null && slotSector == null)
                {
                    continue;
                }

                Debug.Log(slotSector == null);
                //slotSector.SetPosOffset(containGrab.ItemSize);
                //RefrechColor(true);
            }
        }
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
        totalOffset = highlightedSlot.gridPos - (halfOffset + slotSector.posOffset);

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
    private int SlotCheck(Vector2Int startPos, Vector2Int itemSize)
    {
        if (!isOverEdge)
        {
            if (containGrab != null && enterContain != null)
            {
                if (containGrab.item == enterContain.item)
                {
                    return 3;
                }
                else
                {
                    return 1;
                }
            }

            for (int y = 0; y < itemSize.y; y++)
            {
                for (int x = 0; x < itemSize.x; x++)
                {
                    InvenSlot instance = null;

                    if (startPos.x + x < _horizontalSlotCount && startPos.y + y < _verticalSlotCount)
                        instance = slotGrid[startPos.x + x, startPos.y + y];
                    else return 4;

                    if (!instance.isEmpty)  // slot이 비어있지 않으며 
                    {
                        return 2;
                    }
                }
            }

            return 0;
        }
        // 현재 인벤토리 범위를 넘어있을 때
        return 2;
    }

    #region 색깔 변경용 함수들
    public void RefrechColor(bool enter)
    {
        if (enter)
        {
            CheckArea(containGrab.ItemSize);

            switch (checkSatae)
            {
                case 0:
                    ColorChangeLoop(SlotColorHighlights.Green, checkSize, checkStartPos);
                    break;
                case 1:
                    enterContain.SlotColorChange(SlotColorHighlights.Yellow);
                    ColorChangeLoop(SlotColorHighlights.Green, checkSize, checkStartPos);
                    break;
                case 2:
                    ColorChangeLoop(SlotColorHighlights.Red, checkSize, checkStartPos);
                    break;
                case 3:
                    enterContain.SlotColorChange(SlotColorHighlights.Blue2);
                    break;
            }
        }
        else
        {
            isOverEdge = false;

            ColorChangeLoop(checkSize, checkStartPos);
            if (checkSatae == 1)
            {
                enterContain.SlotColorChange(SlotColorHighlights.Blue2);
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
        InvenSlot slot;
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
    #endregion

    /// <summary>
    /// 슬롯에 아이템 저장
    /// </summary>
    /// <param name="item">들어갈 아이템 오브젝트</param>
    private void StoreItem(ItemContain contain, Vector2Int startPos)
    {
        contain.isDragging = false;
        Vector2Int itemSize = contain.item.Size;

        for (int y = 0; y < itemSize.y; y++)
        {
            for (int x = 0; x < itemSize.x; x++)
            {
                // 컨테이너의 슬롯 정보 저장
                contain.StoreSlot(slotGrid[startPos.x + x, startPos.y + y]);
            }
        }

        contain.SlotColorChange(SlotColorHighlights.White);

        Debug.Log(containList);
        TotalWeight += contain.item.itemWeight;
        AddList(contain);

        // 게임 오브젝트 재설정
        Vector2 position = slotGrid[startPos.x, startPos.y].transform.position;
        contain.StoreContain(DropParent, position);

        containGrab = null;
    }

    /// <summary>
    /// 아이템 컨테이너 잡기
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public ItemContain GrabContain(ItemContain contain)
    {
        // 리스트에서 컨테이너 삭제
        RemoveList(contain);

        return contain.GrabContain(); ;
    }

    private ItemContain SwapItem(ItemContain item)
    {
        ItemContain tempItem = GrabContain(enterContain);
        StoreItem(item, totalOffset);
        return tempItem;
    }

    /// <summary>
    /// 같은 아이템을 경우 갯수 증가
    /// </summary>
    public void AddContain(ItemContain InContain, int _count = 0)
    {
        ItemContain storeContain = enterContain;

        int remainCount = storeContain.ItemStack(_count);
        
        if(remainCount > 0)
        {
            InContain.Count = remainCount;
        }
        else if(remainCount == 0)
        {
            StoreItem(InContain, otherItemPos);
        }
    }

    public Action<ItemData, int> onDontGetItem;

    public void GetItemToSlot(ItemCode code, int _count)
    {
        ItemData data = FindCodeData(code);

        List<Vector2Int> emptyList = new List<Vector2Int>();
        List<ItemContain> sameItemContainList = new List<ItemContain>();
        
        // 탐색을 하고
        for (int y = 0; y < _verticalSlotCount; y++)
        {
            for (int x = 0; x < _horizontalSlotCount; x++)
            {
                InvenSlot instance = slotGrid[x, y];

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
                    foreach (var contain in containList)
                    {
                        if(contain.itemCode == data.itemCode)
                        {
                            foreach(var _contain in contain.containList)
                            sameItemContainList.Add(_contain);
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
                ItemContain contain = null;
                // 데이터의 최대 수량보다 크며
                if (_count > data.maxItemCount)
                {
                    remain -= data.maxItemCount;
                    contain = Factory.Instance.GetItemContain(data, data.maxItemCount);
                    if (SlotCheck(grid, data.Size) == 0)
                    {
                        StoreItem(contain, grid);
                        break;
                    }
                }
                else
                {
                    contain = Factory.Instance.GetItemContain(data, remain);
                    if (SlotCheck(grid, data.Size) == 0)
                    {
                        StoreItem(contain, grid);
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

    public ItemData FindCodeData(ItemCode _code)
    {
        foreach(var data in itemDatas)
        {
            if(data.itemCode == _code)
            {
                return data;
            }
        }

        return null;
    }

    public Action<ItemData, int> onUseItem;

    public bool UseItem(ItemCode code, int useCount = 1)
    {
        int remain = useCount;

        for (int i = 0; i < containList.Length; i++)
        {
            if (containList[i].itemCode == code && containList[i].itemCount >= useCount)
            {
                for(int j = containList[i].containList.Count - 1; j > -1; j--)
                {

                    if (remain == 0)
                    {
                        RefreshList();
                        return true;
                    }

                    if (containList[i].containList[j].Count >= remain)
                    {
                        containList[i].containList[j].Count -= remain;
                        containList[i].itemCount -= remain;
                        remain = 0;
                    }
                    else
                    {
                        remain -= containList[i].containList[j].Count;
                    }

                    Debug.Log($"{remain}");

                    if (containList[i].containList[j].Count <= 0)
                    {
                        containList[i].containList.RemoveAt(j);
                    }
                }
            }
        }

        return false;
    }

    public void AddList(ItemContain add)
    {
        for(int i = 0; i < containList.Length; i++)
        {
            if (containList[i].itemCode == add.item.itemCode)
            {
                containList[i].containList.Add(add);
                containList[i].itemCount += add.Count;
            }
        }

        RefreshList();
    }

    public void RemoveList(ItemContain remove)
    {
        for (int i = 0; i < containList.Length; i++)
        {
            for(int j = 0; j < containList[i].containList.Count; j++)
            {
                if (containList[i].containList[j].id == remove.id)
                {
                    TotalWeight -= remove.item.itemWeight;
                    containList[i].itemCount -= remove.Count;
                    containList[i].containList.RemoveAt(j);
                }
            }
        }

        RefreshList();
    }

    public void RefreshList()
    {
        for(int i = 0; i < containList.Length; i++)
        {
            containList[i].itemCount = 0;

            foreach (var contain in containList[i].containList)
            {
                containList[i].itemCount += contain.Count;
            }
        }

        onContainListChange?.Invoke();
    }
}
