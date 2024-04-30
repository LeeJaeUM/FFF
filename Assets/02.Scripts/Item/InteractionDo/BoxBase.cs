using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBase : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        // interact했을때 
        if (true)
        {
            //조건에 충족되면 아이템 추가 또는 여타 상호작용
            //아래 enum 타입을 설정해둔 것들을 프리팹으로 뺴두기
        }
        else
        {
            //불 충분시 아무것도 안하기 / 안내문자 
        }
    }

    enum BoxType
    {
        None = 0,
        Gun,
        Dynamite,
        Magazine
    }
    [SerializeField]BoxType boxType = BoxType.None;

    public int itemcode = 0;

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
}
