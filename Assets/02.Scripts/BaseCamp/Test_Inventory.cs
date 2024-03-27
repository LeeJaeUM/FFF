using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Inventory : TestBaseCamp
{
    public ItemData data;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.ItemContain(data);
    }
}
