using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TreeObj : MonoBehaviour, IInteractable
{
    protected TextMeshProUGUI bottomTMP;

    [SerializeField]
    private ItemCode oldAxe = ItemCode.OldAxe;   
    [SerializeField]
    private int breakCount = 0;
    private InventoryUI inventoryUI;

    readonly int Interact_Hash = Animator.StringToHash("Interact");
    private Animator animator;
    private Stage1Manager stage1Manager;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        stage1Manager = Stage1Manager.Instance;
        bottomTMP = stage1Manager.bottomTMP;
    }

    public virtual void Interact()
    {
        //if (inventoryUI.UseItemCheck((ItemCode)itemcode))
        if(true)
        {
            //조건에 충족되면 아이템 추가 또는 여타 상호작용
            Debug.Log("나무 테스트");
            BreakTree();
        }
        else    // false면
        {
            // 불충분시 안내 텍스트
            bottomTMP.text = ("도끼가 필요하다.");
        }

    }

    /// <summary>
    /// 도끼로 나무를 패는 상호작용 함수
    /// </summary>
    /// <param name="itemcode">박스의 이넘타입에 맞게 얻어질 아이템의 코드</param>
    private void BreakTree()
    {
        animator.SetTrigger(Interact_Hash);
        ItemCode getItem = (ItemCode)0;  //Wood 얻기 : int값으로 받아온 아이템을 아이템코드로 변환해서 getItem에 담기
        Debug.Log($"getItem : {getItem}");
        breakCount++;
        if(breakCount > 2)
        {
            Destroy(gameObject);                    // 나무를 없애고
        }
        //inventoryUI.GetItemToSlot(getItem, 1);  // 인벤토리에 박스 종류에 해당되는 아이템 넣기
        Debug.Log($"얻은 아이템 : {getItem}");
    }

}
