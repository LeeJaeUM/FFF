using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Inventory : TestBase
{
    public ItemCode code;
    public int count = 1;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        GameManager.Instance.inven.GetItemToSlot(code, count);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        //gamemanager.instance.inven.getitemtoslot(itemcode.book_6);
        //gamemanager.instance.inven.getitemtoslot(itemcode.book_29);

        GameManager.Instance.inven.UseItem(code, count);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        GameManager.Instance.inven.GetItemToSlot(ItemCode.BloodyKnife);
        GameManager.Instance.inven.GetItemToSlot(ItemCode.Brain);
        GameManager.Instance.inven.GetItemToSlot(ItemCode.Entrails);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        GameManager.Instance.buttonManager.AllButtonTrigger();
    }
}
