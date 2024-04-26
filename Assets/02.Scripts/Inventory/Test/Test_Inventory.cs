using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Inventory : TestBase
{
    public DropItem DropItem;
    public ItemCode code;
    public int count;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        GameManager.Instance.inven.GetItemToSlot(code, count);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        DropItem.SetData(code, count);
    }
}
