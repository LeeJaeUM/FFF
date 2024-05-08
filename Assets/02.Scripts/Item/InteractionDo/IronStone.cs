using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IronStone : MonoBehaviour, IInteractable
{
    InventoryUI inventoryUI;

    TextMeshProUGUI bottomTMP;

    ItemCode oldAxe = ItemCode.OldAxe;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
    }

    private void Start()
    {
        bottomTMP = Stage1Manager.Instance.bottomTMP;
    }

    public void Interact()
    {
        if (inventoryUI.UseItemCheck(oldAxe))
        {
            Destroy(gameObject);
            inventoryUI.GetItemToSlot(ItemCode.Wood, 8);
        }
        else
        {
            bottomTMP.text = "도끼가 필요할 것 같다.";
        }
    }
}
