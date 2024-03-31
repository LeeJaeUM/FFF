using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData_Produce", menuName = "Data/ItemData_Produce", order = 1)]
public class ItemData_Produce : ItemData
{
    [Header("조합 아이템 부모")]
    public ItemData parentData_1;
    public ItemData parentData_2;
}
