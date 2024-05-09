using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TreeObj : MonoBehaviour, IInteractable
{
    private ItemCode oldAxe = ItemCode.OldAxe;   
    [SerializeField]
    private int breakCount = 0; //나무가 도끼질을 버티는 횟수카운트 // 3회
    private InventoryUI inventoryUI;

    readonly int Interact_Hash = Animator.StringToHash("Interact");
    private Animator animator;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
        animator = GetComponent<Animator>();
    }

    public virtual void Interact()
    {
        if (inventoryUI.UseItemCheck(oldAxe))
        {
            //조건에 충족되면 아이템 추가 또는 여타 상호작용
            Debug.Log("나무 테스트");
            BreakTree();
        }
        else    // false면
        {
            // 불충분시 안내 텍스트
            Stage1Manager.Instance.BottomTMPText = ("도끼가 필요하다.");
            Debug.Log("나무 테스트");
        }

    }

    /// <summary>
    /// 도끼로 나무를 패는 상호작용 함수
    /// </summary>
    private void BreakTree()
    {
        animator.SetTrigger(Interact_Hash);
        //inventoryUI.GetItemToSlot(ItemCode.Wood, 8);  //Wood 얻기 : 아이템코드로  getItem에 담기
        breakCount++;
        if(breakCount > 2)
        {
            Destroy(gameObject);                    // 나무를 없애고
        }
    }

}
