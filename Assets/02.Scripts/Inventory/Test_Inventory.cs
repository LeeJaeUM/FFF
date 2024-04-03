using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Inventory : TestBase
{
    public ItemData data;
    public int count;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        GameManager.Instance.inven.GetItemToSlot(data, count);
    }
}
