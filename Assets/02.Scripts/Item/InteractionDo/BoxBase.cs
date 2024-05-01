using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

public class BoxBase : MonoBehaviour, IInteractable
{

    enum BoxType
    {
        None = 0,
        Gun,
        Dynamite,
        Magazine
    }
    [SerializeField]BoxType boxType = BoxType.None;

    public int itemcode = 0;
    InventoryUI inventoryUI;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
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

    public void Interact()
    {
        // interact했을때 인벤토리에 Axe가 있는지 확인해서
        if (inventoryUI.UseItemCheck(ItemCode.Axe))   // true면
        {
            //조건에 충족되면 아이템 추가 또는 여타 상호작용
            BreakBox(itemcode);
            //enum 타입을 설정해둔 것들을 프리팹으로 뺴두기
        }
        else    // false면
        {
            // 불충분시 안내 텍스트
            Debug.Log("부술 수 있는 도구가 필요하다.");
        }
    }

    private void BreakBox(int itemcode)
    {
        ItemCode getItem;
        getItem = (ItemCode)itemcode;
        Destroy(gameObject);
        inventoryUI.GetItemToSlot(getItem, 1);
        Debug.Log($"얻은 아이템 : {getItem}");
    }
}
