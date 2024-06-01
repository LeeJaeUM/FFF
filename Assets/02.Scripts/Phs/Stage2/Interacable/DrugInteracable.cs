using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrugInteracable : InteracableBase
{
    protected override void OnUse()
    {
        Stage1Manager.Instance.BottomTMPText = "마약이다, 여기의 분진들은 마약에 의한 분진이었다";
    }
}
