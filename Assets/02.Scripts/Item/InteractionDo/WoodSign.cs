using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WoodSign : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject hint;

    [SerializeField]
    private bool isUse = false;

    private void Start()
    {
        Transform child = Stage1Manager.Instance.TipsUI.transform.GetChild(2);
        hint = child.gameObject;
    }

    public void Interact()
    {
        isUse = !isUse;
        //inventoryUI.GetItemToSlot(ItemCode.Book, 1);
        Stage1Manager.Instance.BottomTMPText = "책 안쪽에 쪽지가 ...";
        // 1852가 적힌 쪽지 보이게하기
        hint.SetActive(isUse);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isUse = false;
        }
    }
}
