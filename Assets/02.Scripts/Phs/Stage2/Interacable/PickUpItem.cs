using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : InteracableBase
{
    public ItemCode itemCode;

    public string descript;

    protected override void OnUse()
    {
        GameManager.Instance.inven.GetItemToSlot(itemCode);

        TipsUI tips = Stage1Manager.Instance.TipsUI;

        tips.CanUse_InteractObj = false;

        Stage1Manager.Instance.BottomTMPText = descript;

        Destroy(gameObject);
    }

    public void GetItem()
    {
        OnUse();
    }
}
