using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWall : MonoBehaviour, IInteractable
{
    private ItemCode dynamite = ItemCode.Dynamite; 
    private ItemCode matches = ItemCode.Matches; 
    private InventoryUI inventoryUI;

    public float dynamiteDelay = 2.0f;


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
            StartCoroutine(WallDestroy());
        }
        else if (inventoryUI.UseItemCheck(dynamite))
        {
            Stage1Manager.Instance.BottomTMPText = ("Dynamite에 불을 붙일 것이 필요하다");
        }
        else if(inventoryUI.UseItemCheck(ItemCode.Hammer) || inventoryUI.UseItemCheck(ItemCode.OldPick))
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.PickaxeMiningIronOre );
            Stage1Manager.Instance.BottomTMPText = ("더 강한 충격을 줘야할 거 같다");
        }

    }

    IEnumerator WallDestroy()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.DynamiteIgnition );

        yield return new WaitForSeconds(dynamiteDelay);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.DynamiteExplosion);


        yield return new WaitForSeconds(0.5f);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.BreakingWall);
        Destroy(gameObject);
    }

}
