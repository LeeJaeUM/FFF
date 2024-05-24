using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilDrum : MonoBehaviour, IInteractable
{
    InventoryUI inventoryUI;

    readonly ItemCode axe = ItemCode.Axe;
    readonly ItemCode bucket = ItemCode.Bucket;

    bool canUse = true;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
    }

    public void Interact()
    {
        if(canUse)
        {
            // 인벤토리에 Axe가 있는지 확인
            if (inventoryUI.UseItemCheck(axe))
            {
                // 특수 상호작용: 기름통을 부수는 동작 실행
                BreakOilDrum();
            }
            else
            {
                // 일반 상호작용: 메시지 표시
                Stage1Manager.Instance.BottomTMPText = "윗면을 부술만한 것이 필요하다.";
            }
        }
        else
        {
            Stage1Manager.Instance.BottomTMPText = "더 이상 기름이 남아있지 않은 것 같다.";
        }
    }

    private void BreakOilDrum()
    {
        // 특수 상호작용: 드럼 부수는 동작 실행
        Debug.Log("캉!!"); // 사운드 재생

        // 인벤토리에 Bucket이 있는지 확인
        if (inventoryUI.UseItemCheck(bucket))
        {
            // Bucket을 OilBucket으로 변경
            inventoryUI.UseItem(bucket);
            inventoryUI.GetItemToSlot(ItemCode.OilBucket, 1);
        }
        else
        {
            // Bucket이 없을 경우 메시지 표시
            Stage1Manager.Instance.BottomTMPText = "기름을 담을 만한 그릇이 필요하다.";
        }
        canUse = false;
    }
}
