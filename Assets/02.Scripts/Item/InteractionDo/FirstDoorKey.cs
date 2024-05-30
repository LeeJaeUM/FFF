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
        Stage1Manager.Instance.BottomTMPText = "열쇠를 얻었다.";
        Destroy(gameObject);
    }
}
