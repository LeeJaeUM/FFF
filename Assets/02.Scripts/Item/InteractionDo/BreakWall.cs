using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWall : MonoBehaviour, IInteractable
{
    private ItemCode dynamite = ItemCode.Dynamite; 
    private ItemCode matches = ItemCode.Matches; 
    private InventoryUI inventoryUI;

    public bool testbool = false;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
    }

    public virtual void Interact()
    {
        if(inventoryUI.UseItemCheck(dynamite) && inventoryUI.UseItemCheck(matches))
        {
            //인벤토리에서 다이너마이트와 성냥 삭제 = 사용 하는 함수임
            inventoryUI.UseItem(dynamite);
            inventoryUI.UseItem(matches);
            // 코드 추가 예정

            Stage1Manager.Instance.BottomTMPText = ("길을 막은 벽을 처리했다");
            
            // 벽을 없애고
            Destroy(gameObject);
            
        }
        else if (inventoryUI.UseItemCheck(dynamite))
        {
            Stage1Manager.Instance.BottomTMPText = ("Dynamite에 불을 붙일 것이 필요하다");
        }
        else if(inventoryUI.UseItemCheck(ItemCode.Hammer))
        {
            int random = Random.Range(0, 2);
            if(random > 0)
                Stage1Manager.Instance.BottomTMPText = ("망치로도 부술 수 없을 거 같다");
            else
                Stage1Manager.Instance.BottomTMPText = ("더 강한 충격을 줘야할 거 같다");
        }

    }

}
