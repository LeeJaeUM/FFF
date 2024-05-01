using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using System.ComponentModel;

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

    private int checkState;
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

    /// <summary>
    /// 현재 손에 있는 아이템 컨테이너
    /// </summary>
    public ItemContain containGrab = null;

    public ItemContain ContainGrab
    {
        get => containGrab;
        set
        {
            if (containGrab != value)
            {
                containGrab = value;
                RefreshGrab();
            }
        }
    }

    /// <summary>
    /// 마우스 포인터 위에 있는 컨테이너
    /// </summary>
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

    public ItemData[] itemDatas;

    public Action<float> onWeightChange;

    /// <summary>
    /// 인벤토리 무게
    /// </summary>
    private float weight = 0.0f;

    public float TotalWeight
    {
        get => weight;
        set
        {
            if (weight != value)
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

    /// <summary>
    /// 현재 마우스 위에 있는 슬롯의 위치
    /// </summary>
    public SlotSector slotSector;

    /// <summary>
    /// 디자인용 UI
    /// </summary>
    private InvenUI UI;

    /// <summary>
    /// 제작용 매니저
    /// </summary>
    public ProduceManager produceManager;

    /// <summary>
    /// 드랍시 위치
    /// </summary>
    public Transform target;

    /// <summary>
    /// 입력
    /// </summary>
    MouseInputAction inputAction;

    #region Awake(), start()
    private void Awake()
    {
        inputAction = new MouseInputAction();

        Transform child = transform.GetChild(0);
        invenGrid = child.GetComponentInChildren<InvenGrid>();
        child = child.GetChild(3);
        DropParent = child.GetChild(0);
        DragParent = child.GetChild(1);
        tooltip = GetComponentInChildren<ItemTooltip>();
        UI = GetComponentInChildren<InvenUI>();
        produceManager = GetComponentInChildren<ProduceManager>();

        containList = new ItemContainList[itemDatas.Length];

        for (int i = 0; i < containList.Length; i++)
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
        inputAction.UI.InventroyOnOff.performed += OnOpenClose;
    }

    private void OnDisable()
    {
        inputAction.UI.InventroyOnOff.performed += OnOpenClose;
        inputAction.UI.LClick.performed -= OnLeftClick;
        inputAction.UI.Disable();
    }

    #endregion

    #region 입력 함수
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

            if (containGrab != null)
            {
                if (highlightedSlot != null && !isOverEdge)
                {
                    switch (checkState)
                    {
                        case 0:
                            // 빈 슬롯에 저장
                            if (isShiftPress && containGrab.Count > 1)
                            {
                                StoreItem(ContainGrab.ItemSplit(), totalOffset);
                                ColorChangeLoop(SlotColorHighlights.Blue, enterContain.ItemSize, totalOffset);
                            }
                            else
                            {
                                StoreItem(containGrab, totalOffset);
                                ColorChangeLoop(SlotColorHighlights.Blue, highlightedSlot.storedItemSize, totalOffset);
                            }
                            break;
                        case 1:
                            // 다른 아이템 스왑
                            ContainGrab = SwapItem(ContainGrab).GrabContain();
                            slotSector.SetPosOffset(otherItemSize);
                            ColorChangeLoop(SlotColorHighlights.White, otherItemSize, otherItemPos);
                            RefrechColor(true);
                            break;
                        case 3:
                            // 같은 아이템이면 저장
                            // 같은 아이템이면 저장
                            if (isShiftPress && ContainGrab.Count > 1)
                            {
                                Debug.Log("같은 아이템 하나씩 저장");
                                ContainGrab.ItemDestack();
                                AddContain(ContainGrab, 1);
                                ColorChangeLoop(SlotColorHighlights.White, otherItemSize, otherItemPos);
                            }
                            else
                            {
                                Debug.Log("같은 아이템이므로 저장");
                                AddContain(containGrab, ContainGrab.Count);
                                ColorChangeLoop(SlotColorHighlights.White, otherItemSize, otherItemPos);
                                ContainGrab.ContainRemvoe();
                            }
                            break;
                    }

                    tooltip.IsPause = false;
                }
                else if (highlightedSlot == null)
                {
                    Drop(target);
                }
            }
            else if (highlightedSlot == null && ContainGrab == null
                && enterContain != null)
            {
                // 컨테이너의 저장된 아이템 색 바꾸기
                enterContain.SlotColorChange(SlotColorHighlights.White);

                tooltip.IsPause = true;

                if (isShiftPress && enterContain.Count > 1)
                {
                    int _count = Mathf.FloorToInt(enterContain.Count / 2);
                    ItemContain returnContain = enterContain.ItemSplit(_count);
                    returnContain.GrabContain();
                }
                else
                {
                    ContainGrab = GrabContain(enterContain);
                }
            }
        }
    }

    private void OnOpenClose(InputAction.CallbackContext context)
    {
        if (UI != null)
        {
            UI.OnOff();
        }
    }

    #endregion

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
        ItemContain SlotContain = null;
        if (!isOverEdge)
        {
            for (int y = 0; y < itemSize.y; y++)
            {
                for (int x = 0; x < itemSize.x; x++)
                {
                    InvenSlot instance = null;
                    if (startPos.x + x < _horizontalSlotCount && startPos.y + y < _verticalSlotCount)
                        instance = slotGrid[startPos.x + x, startPos.y + y];
                    else return 2;

                    if (!instance.isEmpty)  // slot이 비어있지 않으며 
                    {
                        if (SlotContain == null)
                        {
                            // 슬롯에 담긴 컨테이너 정보 추출
                            SlotContain = instance.storedContain;
                            otherItemPos = instance.storedItemStartPos;
                            otherItemSize = SlotContain.item.Size;
                        }
                        else if (containGrab != null)
                        {
                            if (SlotContain != instance.storedContain && instance.data != containGrab.item)
                                // 슬롯 오브젝트와 저장된 아이템하고 다른때, 아이템 정보가 서로 다른때
                                return 2;
                            else if (instance.data == containGrab.GetComponent<ItemContain>().item)
                                // 저장된 아이템이 서로 같을때
                                return 3;
                        }
                    }
                }
            }
            if (SlotContain == null)
                return 0;
            else
                return 1;
        }
        return 2;
    }

    #region 색깔 변경용 함수들
    public void RefrechColor(bool enter)
    {
        if (enter)
        {
            if (containGrab != null)
            {
                CheckArea(containGrab.ItemSize);
            }

            if (highlightedSlot != null)
            {
                checkState = SlotCheck(checkStartPos, checkSize);
            }

            switch (checkState)
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
            if (checkState == 1)
            {
                ColorChangeLoop(SlotColorHighlights.White, otherItemSize, otherItemPos);
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

        ContainGrab = null;
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
        ItemContain tempItem = GrabContain(slotGrid[otherItemPos.x, otherItemPos.y].storedContain);
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

        if (remainCount > 0)
        {
            InContain.Count = remainCount;
        }
        else if (remainCount == 0)
        {
            StoreItem(InContain, otherItemPos);
        }
    }

    public Action<ItemData, int> onDontGetItem;

    /// <summary>
    /// 아이템을 얻는 함수
    /// </summary>
    /// <param name="code">얻은 아이템 코드</param>
    /// <param name="_count">얻은 아이템의 갯수</param>
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

                    if (state == 0)
                    {
                        emptyList.Add(new Vector2Int(x, y));
                    }
                }
                else
                {
                    foreach (var contain in containList)
                    {
                        if (contain.itemCode == data.itemCode)
                        {
                            foreach (var _contain in contain.containList)
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
                if (remain == 0)
                    break;
            }
        }
        if (emptyList.Count > 0 && remain > 0)
        {
            foreach (var grid in emptyList)
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

    #region 아이템 관련 함수
    /// <summary>
    /// 아이템의 코드를 사용하여 아이템 정보 출력
    /// </summary>
    /// <param name="_code">찾을 아이템 코드</param>
    /// <returns>찾은 아이템 정보</returns>
    public ItemData FindCodeData(ItemCode _code)
    {
        foreach (var data in itemDatas)
        {
            if (data.itemCode == _code)
            {
                return data;
            }
        }

        return null;
    }

    public Action<ItemData, int> onUseItem;

    /// <summary>
    /// 아이템을 사용 가능 여부 
    /// </summary>
    /// <param name="code">사용할 아이템 코드</param>
    /// <param name="useCount">사용할 아이템의 갯수</param>
    /// <returns>true면 사용 성공, false면 사용 실패</returns>
    /// <returns></returns>
    public bool UseItemCheck(ItemCode code, int useCount = 1)
    {
        bool isOk = false;

        foreach (var List in containList)
        {
            if (List.itemCode == code && List.itemCount >= useCount)
            {
                isOk = true;
            }
        }

        return isOk;
    }

    /// <summary>
    /// 아이템을 사용하는 함수
    /// </summary>
    /// <param name="code">사용할 아이템 코드</param>
    /// <param name="useCount">사용할 아이템의 갯수</param>
    /// <returns>true면 사용 성공, false면 사용 실패</returns>
    public bool UseItem(ItemCode code, int useCount = 1)
    {
        int remain = useCount;

        if (UseItemCheck(code, useCount))
        {
            for (int i = 0; i < containList.Length; i++)
            {
                if (containList[i].itemCode == code && containList[i].itemCount >= useCount)
                {
                    for (int j = containList[i].containList.Count - 1; j > -1; j--)
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
        }

        return false;
    }

    /// <summary>
    /// 리스트에 아이템 정보 추가
    /// </summary>
    /// <param name="add"></param>
    public void AddList(ItemContain add)
    {
        for (int i = 0; i < containList.Length; i++)
        {
            if (containList[i].itemCode == add.item.itemCode)
            {
                containList[i].containList.Add(add);
                containList[i].itemCount += add.Count;
            }
        }

        RefreshList();
    }

    /// <summary>
    /// 리스트에 아이템 정보 삭제
    /// </summary>
    /// <param name="remove"></param>
    public void RemoveList(ItemContain remove)
    {
        for (int i = 0; i < containList.Length; i++)
        {
            containList[i].itemCount -= remove.Count;
            containList[i].containList.Remove(remove);
        }

        RefreshList();
    }

    /// <summary>
    /// 인벤토리 안에 정보 재출력
    /// </summary>
    public void RefreshList()
    {
        for (int i = 0; i < containList.Length; i++)
        {
            containList[i].itemCount = 0;

            foreach (var contain in containList[i].containList)
            {
                containList[i].itemCount += contain.Count;
            }
        }

        TotalWeight = WeightCheck();

        onContainListChange?.Invoke();
    }

    /// <summary>
    /// 컨테이너 그립 시 여부에 따라 저장된 컨테이너의 UI 레이캐스트 OnOff
    /// </summary>
    private void RefreshGrab()
    {
        foreach (var _containList in containList)
        {
            foreach (var contain in _containList.containList)
            {
                if (ContainGrab == null)
                {
                    contain.RecastOn();
                }
                else
                {
                    contain.RecastOff();
                }
            }
        }
    }

    /// <summary>
    /// 인벤토리 내의 무게 증감 여부
    /// </summary>
    /// <returns>인벤토리 내의 총 무게</returns>
    private float WeightCheck()
    {
        float result = 0;

        foreach (var List in containList)
        {
            ItemData data = FindCodeData(List.itemCode);
            Debug.Log($"{data.itemWeight}_{List.itemCount}");
            result += data.itemWeight * List.itemCount;
        }

        return result;
    }

    /// <summary>
    /// 아이템을 드랍하는 함수
    /// </summary>
    private void Drop(Transform position)
    {
        Factory.Instance.GetDropItem(containGrab, position);
        containGrab.ContainRemvoe();
    }
    #endregion
}