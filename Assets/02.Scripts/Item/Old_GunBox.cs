using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class Old_GunBox : MonoBehaviour
{
    /// <summary>
    /// 인벤토리에 Axe 가지고 있는지 확인용 변수
    /// </summary>
    bool onAxe = false;

    [SerializeField]
    GameObject gunBox;
    [SerializeField]
    GameObject dynamite;
    [SerializeField]
    ItemData itemData;

    private void Awake()
    {
        itemData = FindAnyObjectByType<ItemData>();
    }

    public void BreakGunBox()
    {
        /*if (onAxe && itemData.itemID != 5) // ID 5번은 gunBox 이다
        {
            DropDynamite();
        }*/
    }

    void DropDynamite()
    {
        Destroy(gunBox);
        //실행 된 자리에 Dynamite아이템을 생성
        Instantiate(dynamite, transform.position, Quaternion.identity);
    }
}
