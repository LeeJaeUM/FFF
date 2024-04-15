using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템의 데이터를 담아두는 스크러터블 오브젝트
[CreateAssetMenu(fileName = "ItemData", menuName = "Data/ItemData", order = 0)]
public class ItemData : ScriptableObject
{
    /// <summary>
    /// 아이템의 고유코드
    /// </summary>
    public ItemCode itemCode;

    /// <summary>
    /// 아이템의 아이콘
    /// </summary>
    public Sprite itemIcon;

    /// <summary>
    /// 아이템의 이름
    /// </summary>
    public string itemName;

    [TextArea]
    /// <summary>
    /// 아이템의 설명
    /// </summary>
    public string itemDescription;

    /// <summary>
    /// 아이템의 가로크기
    /// </summary>
    [Range(1, 3)]
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
            if(Size != value)
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
    /// 아이템 가격
    /// </summary>
    public float itemPrice;

    /// <summary>
    /// 아이템의 무게
    /// </summary>
    public float itemWeight;
}
