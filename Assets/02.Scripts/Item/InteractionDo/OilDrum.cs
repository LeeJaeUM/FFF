using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilDrum : MonoBehaviour, IInteractable
{
    InventoryUI inventoryUI;

    readonly ItemCode axe = ItemCode.Axe;
    readonly ItemCode bucket = ItemCode.Bucket;

    bool canUse = true;

    bool isOpen = false;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
    }

    public void Interact()
    {
        if(canUse && !isOpen)
        {
            BreakOilDrum();
        }
        else if(canUse && isOpen)
        {
            PourOil();
        }
        else if(!canUse && isOpen)
        {
            Stage1Manager.Instance.BottomTMPText = "더 이상 기름이 남아있지 않은 것 같다.";
        }
    }

    private void BreakOilDrum()
    {
        // 인벤토리에 Axe가 있는지 확인
        if (inventoryUI.UseItemCheck(axe))
        {
            // 특수 상호작용: 기름통을 부수는 동작 실행
            Stage1Manager.Instance.BottomTMPText = "뚜껑을 부쉈다.";
            isOpen = true;
        }
        else
        {
            // 일반 상호작용: 메시지 표시
            Stage1Manager.Instance.BottomTMPText = "윗면을 부술만한 것이 필요하다.";
        }
    }
    
    private void PourOil()
    {
        if (inventoryUI.UseItemCheck(bucket))
        {
            // Bucket을 OilBucket으로 변경
            inventoryUI.UseItem(bucket);
            inventoryUI.GetItemToSlot(ItemCode.OilBucket, 1);
            Stage1Manager.Instance.BottomTMPText = "기름을 담았다.";
            canUse = false;
        }
        else
        {
            // Bucket이 없을 경우 메시지 표시
            Stage1Manager.Instance.BottomTMPText = "기름을 담을 만한 그릇이 필요하다.";
        }
    }
}
