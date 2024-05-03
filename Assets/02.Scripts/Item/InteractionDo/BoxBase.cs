using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

public class BoxBase : MonoBehaviour, IInteractable
{
    /// <summary>
    /// 하단 자막용 텍스트
    /// </summary>
    TextMeshProUGUI bottomTMP;

    /// <summary>
    /// 박스 타입의 이넘
    /// </summary>
    enum BoxType
    {
        None = 0,
        Gun,
        Dynamite,
        Magazine
    }
    [SerializeField]BoxType boxType = BoxType.None;

    /// <summary>
    /// 아이템 코드
    /// </summary>
    public int itemcode = 0;

    /// <summary>
    /// 인벤토리
    /// </summary>
    InventoryUI inventoryUI;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();   // 인벤토리UI 참조
    }

    private void Start()
    {
        bottomTMP = Stage1Manager.Instance.bottomTMP;

       //start에서 위의 boxtype에 따라서 itemCode Itemcode enum타입의 맞게 코드 설정해주기
       switch(boxType)
        {
            case BoxType.None:
                Debug.Log("아무것도 선택되지 않은 상자");
                break;
            case BoxType.Gun:
                itemcode = (int)ItemCode.Gun;
                break;
            case BoxType.Dynamite:
                itemcode = (int)ItemCode.Dynamite;
                break;
            case BoxType.Magazine:
                itemcode = (int)ItemCode.Magazine;
                break;
            default:    break;
        }
    }

    /// <summary>
    /// 테스트용 bool 변수
    /// </summary>
    readonly bool isAxeInInven = false;

    /// <summary>
    /// 상호작용 시도 시 호출될 함수
    /// </summary>
    public void Interact()
    {
        // interact했을때 인벤토리에 Axe가 있는지 확인해서
        if (/*inventoryUI.UseItemCheck(ItemCode.Axe)*/ isAxeInInven)   // true면
        {
            //조건에 충족되면 아이템 추가 또는 여타 상호작용
            BreakBox(itemcode);
            //enum 타입을 설정해둔 것들을 프리팹으로 뺴두기
        }
        else    // false면
        {
            // 불충분시 안내 텍스트
            Debug.Log("부술 수 있는 도구가 필요하다.");
            bottomTMP.text = "부술 수 있는 도구가 필요하다";
        }
    }

    /// <summary>
    /// 박스 부수는 상호작용 함수
    /// </summary>
    /// <param name="itemcode">박스의 이넘타입에 맞게 얻어질 아이템의 코드</param>
    private void BreakBox(int itemcode)
    {
        ItemCode getItem = (ItemCode)itemcode;  // int갑으로 받아온 아이템을 아이템코드로 변환해서 getItem에 담기
        Debug.Log($"getItem : {getItem}");
        Destroy(gameObject);                    // 박스를 없애고
        inventoryUI.GetItemToSlot(getItem, 1);  // 인벤토리에 박스 종류에 해당되는 아이템 넣기
        Debug.Log($"얻은 아이템 : {getItem}");
    }
}
