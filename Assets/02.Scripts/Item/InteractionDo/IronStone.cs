using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IronStone : MonoBehaviour, IInteractable
{
    InventoryUI inventoryUI;

    readonly ItemCode oldPick = ItemCode.OldPick;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
    }

    public void Interact()
    {
        if(inventoryUI.UseItemCheck(oldPick))
        {
            Destroy(gameObject);
            inventoryUI.GetItemToSlot(ItemCode.Ironstone, 5);
        }
        else
        {
            Stage1Manager.Instance.BottomTMPText = "곡괭이가 필요하다.";
        }
    }
}
