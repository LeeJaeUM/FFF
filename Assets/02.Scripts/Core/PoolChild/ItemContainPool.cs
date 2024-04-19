using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainPool : ObjectPool<ItemContain>
{
    public static int IDCount = 0;

    protected override void OnGetObject(ItemContain component)
    {
        component.id = IDCount;
        IDCount++;
    }
}
