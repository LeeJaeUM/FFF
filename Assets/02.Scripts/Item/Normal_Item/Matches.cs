using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matches : MonoBehaviour, IInteractable
{
    InventoryUI inventoryUI;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
    }

    public void Interact()
    {
        inventoryUI.GetItemToSlot(ItemCode.Matches, 1);
    }
}
