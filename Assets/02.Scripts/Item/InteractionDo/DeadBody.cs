using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBody : MonoBehaviour, IInteractable
{
    InventoryUI inventoryUI;

    bool canUse = true;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
    }

    public void Interact()
    {
        if (canUse)
        {
            // 상호작용 하면 주머니에 무언가가 있다라는 텍스트와함께 MasterKey 인벤토리로
            Stage1Manager.Instance.BottomTMPText = "주머니에 무언가가 있다.";

            inventoryUI.GetItemToSlot(ItemCode.MasterKey, 1);
            canUse = false;
        }
        else
        {
            Stage1Manager.Instance.BottomTMPText = "더 이상 쓸만한 것은 없어보인다.";
        }
    }
}
