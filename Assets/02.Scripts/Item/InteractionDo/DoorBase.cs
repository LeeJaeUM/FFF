using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorBase : MonoBehaviour, IInteractable
{

    protected TextMeshProUGUI bottomTMP;
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

    readonly int Interact_Hash = Animator.StringToHash("Interact");
    Animator animator;
    Stage1Manager stage1Manager;
    TipsUI tipsUI;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
        animator = GetComponent<Animator>();  
    }

    private void Start()
    {
        stage1Manager = Stage1Manager.Instance;
        bottomTMP = stage1Manager.bottomTMP;
        tipsUI = stage1Manager.TipsUI;

        //start에서 위의 doorType에 따라서 itemCode Itemcode enum타입의 맞게 코드 설정해주기
        switch (doorType)
        {
            case DoorType.None:
                Debug.Log("아무것도 선택되지 않은 문");
                itemcode = 99;
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
                itemcode = -1;
                Debug.Log("키패드로 비밀번호를 풀어야하는 문이다.");
                break;
            default: break;
        }
    }

    public virtual void Interact()
    {
        //기본 문 조건
        if (itemcode == 99)
        {
            DoorOpen();
        }
        else if (true ) //아래 조건 여기에 사용
        {

        }
        // interact했을때 인벤토리에 코드에 맞는 아이템이 있는지 확인해서
        //if (inventoryUI.UseItemCheck((ItemCode)itemcode))   // true면
        //if (testBool)
        //{
        //    //조건에 충족되면 아이템 추가 또는 여타 상호작용
        //    DoorOpen();
        //}
        //else    // false면
        //{
        //    // 불충분시 안내 텍스트
        //    bottomTMP.text =  ("문을 열기 위한 도구가 필요하다.");
        //}


        Debug.Log("문과 interact함");
    }

    protected void DoorOpen()
    {
        //문 성공적으로 열림
        Debug.Log("문 성공적으로 열림");
        animator.SetTrigger(Interact_Hash);
    }
}
