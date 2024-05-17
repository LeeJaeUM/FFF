using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstDoorKey : MonoBehaviour, IInteractable
{
    InventoryUI inventoryUI;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
    }

    public void Interact()
    {
        inventoryUI.GetItemToSlot(ItemCode.FirstDoorKey, 1);
        Destroy(gameObject);
    }
}
