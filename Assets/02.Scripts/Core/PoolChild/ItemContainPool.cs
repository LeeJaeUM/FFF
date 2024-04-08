using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainPool : ObjectPool<ItemContain>
{
    protected override void OnGetObject(ItemContain component)
    {
        int childLength = transform.childCount;
        for(int i = 0; i < childLength; i++)
        {
            ItemContain contain = transform.GetChild(i).GetComponent<ItemContain>();
            contain.id = i;
        }
    }
}
