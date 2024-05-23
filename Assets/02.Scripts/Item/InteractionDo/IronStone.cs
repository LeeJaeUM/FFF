using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IronStone : MonoBehaviour, IInteractable
{
    readonly ItemCode pick = ItemCode.Pick;
    InventoryUI inventoryUI;

    [SerializeField]
    private int breakCount = 0; //나무가 도끼질을 버티는 횟수카운트 // 3회
    readonly int Interact_Hash = Animator.StringToHash("Interact");
    private Animator animator;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        if(inventoryUI.UseItemCheck(pick))
        {
            BreakStone();
            inventoryUI.GetItemToSlot(ItemCode.Ironstone, 5);
        }
        else
        {
            Stage1Manager.Instance.BottomTMPText = "곡괭이가 필요하다.";
        }
    }
    private void BreakStone()
    {
        animator.SetTrigger(Interact_Hash);
        //AudioManager.instance.PlaySfx(AudioManager.Sfx.ChopTree);
        inventoryUI.GetItemToSlot(ItemCode.Ironstone, 5);  //stone 얻기 
        inventoryUI.GetItemToSlot(ItemCode.IronPlanks, 1);  //stone 얻기 
        breakCount++;
        if (breakCount > 2)
        {
            //AudioManager.instance.PlaySfx(AudioManager.Sfx.FallingTree);
            Destroy(gameObject);                    // 돌 없애기
        }
    }

}
