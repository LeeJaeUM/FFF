using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookButtonInteracable : InteracableBase
{
    bool isCanUse = false;

    private void Start()
    {
        ChooseUI choose = FindAnyObjectByType<ChooseUI>();

        choose.onBookButtonActive += OnBookButtonActive;
    }

    private void OnBookButtonActive()
    {
        isCanUse = true;
    }

    protected override void OnUse()
    {
        if(isCanUse)
        {

        }
        else
        {
            Stage1Manager.Instance.BottomTMPText = "눌러도 반응이 없다.";
        }
    }
}
