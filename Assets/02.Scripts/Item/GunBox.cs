using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBox : MonoBehaviour
{
    /// <summary>
    /// 인벤토리에 Hammer를 가지고 있는지 확인용 변수
    /// </summary>
    bool hasHammer = false;

    public GameObject gunBox;
    ItemData target;

    public void BreakGunBox()
    {
        if (hasHammer && target.itemID != 5) // ID 5번은 gunBox 이다
        {
            Destroy(gunBox);
        }
    }
}
