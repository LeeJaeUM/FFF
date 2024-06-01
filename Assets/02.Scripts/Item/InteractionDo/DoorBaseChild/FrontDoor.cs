using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontDoor : DoorBase
{
    /// <summary>
    /// 출입구가 개방됨을 알리는 델리게이트
    /// </summary>
    public Action onFrontDoorOpen; 

    protected override void DoorOpen()
    {
        base.DoorOpen();
        onFrontDoorOpen();
    }
}
