using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Book : MonoBehaviour, IInteractable
{
    InventoryUI inventoryUI;

    TextMeshProUGUI bottomTMP;
    Image hint;

    bool isUse = false;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
    }

    private void Start()
    {
        bottomTMP = Stage1Manager.Instance.bottomTMP;
        Transform child = Stage1Manager.Instance.TipsUI.transform.GetChild(1);
        hint = child.GetComponent<Image>(); 
    }

    public void Interact()
    {
        isUse = !isUse;
        //inventoryUI.GetItemToSlot(ItemCode.Book, 1);
        bottomTMP.text = "책 안쪽에 쪽지가 ...";
        // 1852가 적힌 쪽지 보이게하기
        hint.enabled = isUse;
    }
}
