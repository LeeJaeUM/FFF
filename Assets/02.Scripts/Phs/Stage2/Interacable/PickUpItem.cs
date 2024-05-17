using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : InteracableBase
{
    public ItemCode itemCode;

    protected override void OnUse()
    {
        GameManager.Instance.inven.GetItemToSlot(itemCode);

        TipsUI tips = Stage1Manager.Instance.TipsUI;

        tips.CanUse_InteractObj = false;

        Destroy(gameObject);
    }

    public void GetItem()
    {
        OnUse();
    }
}
