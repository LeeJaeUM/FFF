using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : InteracableBase
{
    public ItemCode itemCode;

    public override bool CanUse => true;

    protected override void OnUse()
    {
        GameManager.Instance.inven.GetItemToSlot(itemCode);
        Destroy(gameObject);
    }
}
