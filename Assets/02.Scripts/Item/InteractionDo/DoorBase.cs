using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorBase : MonoBehaviour, IInteractable
{
    public bool testBool = false;

    public TextMeshProUGUI bottomTMP;
    enum DoorType
    {
        None = 0,
        Normal,
        Masterkey,
        Firstkey,
        Metal,
        Clear
    }

    [SerializeField] DoorType doorType = DoorType.None;

    public int itemcode = 0;
    InventoryUI inventoryUI;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();

        //bottomTMP 를 불러오기 위한 매니저 생성해야함
    }

    private void Start()
    {
        Stage1Manager stage1Manager = FindAnyObjectByType<Stage1Manager>();
        bottomTMP = stage1Manager.bottomTMP;

        //start에서 위의 doorType에 따라서 itemCode Itemcode enum타입의 맞게 코드 설정해주기
        switch (doorType)
        {
            case DoorType.None:
                Debug.Log("아무것도 선택되지 않은 문");
                break;
            case DoorType.Normal:
                itemcode = (int)ItemCode.Axe;
                break;
            case DoorType.Masterkey:
                itemcode = (int)ItemCode.MasterKey;
                break;
            case DoorType.Firstkey:
                itemcode = (int)ItemCode.Key;
                break;
            case DoorType.Metal:
                itemcode = (int)ItemCode.OilBucket;
                break;
            case DoorType.Clear:
                itemcode = 0;
                Debug.Log("키패드로 비밀번호를 풀어야하는 문이다.");
                break;
            default: break;
        }
    }

    public virtual void Interact()
    {
        // interact했을때 인벤토리에 코드에 맞는 아이템이 있는지 확인해서
        //if (inventoryUI.UseItemCheck((ItemCode)itemcode))   // true면
        if (testBool)
        {
            //조건에 충족되면 아이템 추가 또는 여타 상호작용
            DoorOpen();
        }
        else    // false면
        {
            // 불충분시 안내 텍스트
            bottomTMP.text =  ("문을 열기 위한 도구가 필요하다.");
        }
    }

    protected void DoorOpen()
    {
        //문 성공적으로 열림
        Debug.Log("문 성공적으로 열림");
    }
}
