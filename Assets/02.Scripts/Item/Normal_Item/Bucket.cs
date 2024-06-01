using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour,IInteractable
{
    InventoryUI inventoryUI;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
    }

    public void Interact()
    {
        inventoryUI.GetItemToSlot(ItemCode.Bucket, 1);
        Stage1Manager.Instance.BottomTMPText = "통을 얻었다.";
        Destroy(gameObject);
    }
}
