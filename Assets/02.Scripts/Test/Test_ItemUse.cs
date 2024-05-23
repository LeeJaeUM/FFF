using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ItemUse : TestBase
{
    public ItemCode code;
    public int count;
    /// <summary>
    /// 인벤토리
    /// </summary>
    InventoryUI inventoryUI;
    Stage1Manager stage1Manager;

    private void Start()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();   // 인벤토리UI 참조
        
    }


    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Debug.Log("11");
        GameManager.Instance.inven.GetItemToSlot(code, count);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Debug.Log("22");
        inventoryUI.UseItem(ItemCode.Wood,  5);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Debug.Log("33");
        if (inventoryUI.UseItem(ItemCode.Wood, 5))
        {
            Stage1Manager.Instance.BottomTMPText = ("나무를 5개 씀");
        }
        else
        {
            Stage1Manager.Instance.BottomTMPText = ("나무가 없어서 못써요");
        }
    }
    protected override void OnTest4(InputAction.CallbackContext context)
    {
        inventoryUI.GetItemToSlot(ItemCode.Matches, 1);
        inventoryUI.GetItemToSlot(ItemCode.Dynamite, 1);

    }
}
