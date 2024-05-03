using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldAxe : MonoBehaviour, IInteractable
{
    InventoryUI inventoryUI;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
    }

    public void Interact()
    {
        inventoryUI.GetItemToSlot(ItemCode.OldAxe, 1);
    }
}
