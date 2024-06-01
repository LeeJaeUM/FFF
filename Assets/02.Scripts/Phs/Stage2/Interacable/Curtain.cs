using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Curtain : InteracableBase
{
    bool useCan => GameManager.Instance.inven.UseItemCheck(ItemCode.OldScissors);

    protected override void OnUse()
    {
        if(useCan || gameObject == null)
        {
            Destroy(gameObject);
        }
        else
        {
            Stage1Manager.Instance.BottomTMPText = "고정되어 있어 커튼을 찢어야한다, 찢어야하는 무언가가 필요하다.";
        }
    }
}
