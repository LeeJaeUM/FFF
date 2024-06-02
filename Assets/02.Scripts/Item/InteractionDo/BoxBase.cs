using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
//using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

public class BoxBase : MonoBehaviour, IInteractable
{
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
    ItemCode itemcode;

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
       //start에서 위의 boxtype에 따라서 itemCode Itemcode enum타입의 맞게 코드 설정해주기
       switch(boxType)
       {
          case BoxType.None:
              Debug.Log("아무것도 선택되지 않은 상자");
              break;
          case BoxType.Gun:
              itemcode = ItemCode.Gun;
              break;
          case BoxType.Dynamite:
              itemcode = ItemCode.Dynamite;
              break;
          case BoxType.Magazine:
              itemcode = ItemCode.Magazine;
              break;
          default:    break;
       }
    }

    /// <summary>
    /// 인벤토리에 axe가 존재하는지 확인하기 위한 아이템코드
    /// </summary>
    readonly ItemCode axe = ItemCode.Axe;

    /// <summary>
    /// 상호작용 시도 시 호출될 함수
    /// </summary>
    public void Interact()
    {
        // interact했을때 인벤토리에 Axe가 있는지 확인해서
        if (inventoryUI.UseItemCheck(axe))   // true면
        {
            //조건에 충족되면 아이템 추가 또는 여타 상호작용
            BreakBox();
            //enum 타입을 설정해둔 것들을 프리팹으로 뺴두기
        }
        else    // false면
        {
            // 불충분시 안내 텍스트
            Debug.Log("부술 수 있는 도구가 필요하다.");
            Stage1Manager.Instance.BottomTMPText = "부술 수 있는 도구가 필요하다";
        }
    }

    /// <summary>
    /// 박스 부수는 상호작용 함수
    /// </summary>
    /// <param name="itemcode">박스의 이넘타입에 맞게 얻어질 아이템의 코드</param>
    private void BreakBox()
    {
        inventoryUI.GetItemToSlot(itemcode, 1);  // 인벤토리에 박스 종류에 해당되는 아이템 넣기
        Stage1Manager.Instance.BottomTMPText = $"{itemcode}를 얻었다.";
        Destroy(gameObject);                    // 박스를 없애고
    }
}
