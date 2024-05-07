using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearDoor : DoorBase
{
    KeyPad keyPad;
    [SerializeField] private bool isUse = false;

    private void Start()
    {
        keyPad = Stage1Manager.Instance.KeyPad;
        keyPad.onAnswerCheck += DoorOpen;
    }

    public override void Interact()
    {
        isUse = !isUse;
        keyPad.gameObject.SetActive(isUse);


        //if ()
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
