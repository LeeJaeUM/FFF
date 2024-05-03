using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearDoor : DoorBase
{
    KeyPad keyPad;

    private void Start()
    {
        keyPad = Stage1Manager.Instance.KeyPad;
    }

    public override void Interact()
    {
        //if (inventoryUI.UseItemCheck((ItemCode)itemcode))   // true면
        //if (testBool)
        //{
        //    //조건에 충족되면 아이템 추가 또는 여타 상호작용
        //    DoorOpen();
        //}
        //else    // false면
        //{
        //    // 불충분시 안내 텍스트
        //    bottomTMP.text = ("문을 열기 위한 도구가 필요하다.");
        //}
    }
}
