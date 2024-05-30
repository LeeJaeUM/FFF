using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// fileName = 이 에셋을 생성하게 되면 기본적으로 지어질 이름
/// menuName = 유니티 에셋 - 우클릭 - Create - 메뉴에 보여질 이름
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Data/ItemData", order = 0)]
public class ItemData : ScriptableObject    // 게임 오브젝트에 붙일필요 없음
{
    /// <summary>
    /// 아이템의 유형 정의
    /// </summary>
    public enum ItemType
    {
        Equipment,      // 장비
        Used,           // 소모품
        Ingredient,     // 재료
    }

    /// <summary>
    /// 아이템의 고유코드
    /// </summary>
    public ItemCode itemCode;

    ///// <summary>
    ///// 아이템의 고유번호
    ///// </summary>
    //public int itemID;

    /// <summary>
    /// 아이템의 아이콘
    /// </summary>
    public Sprite itemIcon;

    /// <summary>
    /// 아이템의 이름
    /// </summary>
    public string itemName;

    /// <summary>
    /// 아이템의 유형
    /// </summary>
    public ItemType itemType;

    /// <summary>
    /// 아이템의 프리팹(아이템 생성시 프리팹으로 찍어내기 위함)
    /// </summary>
    public GameObject itemPrefab;

    [TextArea]
    /// <summary>
    /// 아이템의 설명
    /// </summary>
    public string itemDescription;

    /// <summary>
    /// 아이템의 가로크기
    /// </summary>
    [Range(1, 5)]
    public int SizeX = 1;

    /// <summary>
    /// 아이템의 세로크기
    /// </summary>
    [Range(1, 5)]
    public int SizeY = 1;

    public Vector2Int Size
    {
        get => new Vector2Int(SizeX, SizeY);
        set
        {
            if (Size != value)
            {
                Size = value;
                Debug.Log(Size);
            }
        }
    }

    /// <summary>
    /// 아이템의 최대 수량
    /// </summary>
    public int maxItemCount;

    /// <summary>
    /// 아이템의 무게
    /// </summary>
    public float itemWeight;


     /* ScriptableObject를 상속받는다면
     * 아이템들이 가지는 기본적인 데이터들을 관리함.
     * 에셋으로서 만들어 둘 수 있음.
     * 다른 스크립트와 달리 오브젝트에 컴포넌트로서 붙일 수 없게됨.(MonoBehavior를 상속받지 못했으므로)
     * 이벤트는 OnEnable, OnDisable, OnDestroy만 받을 수 있음.*/
}
