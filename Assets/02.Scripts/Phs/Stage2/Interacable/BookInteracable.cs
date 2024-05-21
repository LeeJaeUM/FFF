using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookInteracable : InteracableBase
{
    ChooseUI chooseUI;

    private void Awake()
    {
        chooseUI = FindObjectOfType<ChooseUI>();
    }

    protected override void OnUse()
    {
        if(GameManager.Instance.inven.UseItemCheck(ItemCode.Book_6) &&
            GameManager.Instance.inven.UseItemCheck(ItemCode.Book_29))
        {
            chooseUI.Open(ChooseUI.ChooseType.Book);
        }
        else
        {
            Stage1Manager.Instance.BottomTMPText = "책이 꽂혀 있다";
        }
    }
}
