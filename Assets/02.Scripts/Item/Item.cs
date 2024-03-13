using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// fileName = 이 에셋을 생성하게 되면 기본적으로 지어질 이름
/// menuName = 유니티 에셋 - 우클릭 - Create - 메뉴에 보여질 이름
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject    // 게임 오브젝트에 붙일필요 없음
{
    public enum ItemType    // 아이템 유형 정의
    {
        Equipment,      // 장비
        Used,           // 소모품
        Ingredient,     // 재료
    }

    public string itemName;         // 아이템 이름
    public ItemType itemType;       // 아이템 유형
    public Sprite itemImage;        // 아이템 이미지(인벤토리 안에서 보일 이미지) , Sprite로 한 이유=> Sprite는 월드 어디에서든 이미지를 띄울 수 있음.
    public GameObject itemPrefab;   // 아이템 프리팹(아이템 생성시 프리팹으로 찍어낸다)

    public string weaponType;       // 무기 유형

    /* ScriptableObject를 상속받는다면
     * 아이템들이 가지는 기본적인 데이터들을 관리함.
     * 에셋으로서 만들어 둘 수 있음.
     * 다른 스크립트와 달리 오브젝트에 컴포넌트로서 붙일 수 없게됨.(MonoBehavior를 상속받지 못했으므로)
     * 이벤트는 OnEnable, OnDisable, OnDestroy만 받을 수 있음. */
}
