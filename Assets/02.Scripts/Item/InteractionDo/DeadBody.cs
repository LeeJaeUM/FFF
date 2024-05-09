using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBody : MonoBehaviour, IInteractable
{
    InventoryUI inventoryUI;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
    }

    public void Interact()
    {
        // 상호작용 하면 주머니에 무언가가 있다라는 텍스트와함께 MasterKey 인벤토리로
        Stage1Manager.Instance.BottomTMPText = "주머니에 무언가가 있다.";

        inventoryUI.GetItemToSlot(ItemCode.MasterKey, 1);
    }
}
