using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilBarret : InteracableBase
{
    protected override void OnUse()
    {
        InventoryUI inven = GameManager.Instance.inven;

        if (inven.UseItemCheck(ItemCode.Syringe))
        {
            // 비어 있는 주사기 사용
            inven.UseItem(ItemCode.Syringe);

            inven.GetItemToSlot(ItemCode.OilSyringe);
        }
        else
        {
            Stage1Manager.Instance.BottomTMPText = "기름을 담는 도구가 있어야한다.";
        }
    }
}
