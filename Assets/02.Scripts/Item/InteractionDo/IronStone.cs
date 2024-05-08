using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IronStone : MonoBehaviour, IInteractable
{
    InventoryUI inventoryUI;

    TextMeshProUGUI bottomTMP;

    ItemCode oldAxe = ItemCode.OldAxe;

    bool isInInven = false;
    bool IsInInven
    {
        get => isInInven;
        set
        {
            isInInven = value;
            if(inventoryUI.UseItemCheck(oldAxe))
            {
                isInInven = true;
            }
        }
    }

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
        if(IsInInven)
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
