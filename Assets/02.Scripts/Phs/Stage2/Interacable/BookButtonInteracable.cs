using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookButtonInteracable : InteracableBase
{
    public bool isCanUse = false;

    ChooseUI choose;

    private void Start()
    {
        choose = FindAnyObjectByType<ChooseUI>();

        choose.onBookButtonActive += OnBookButtonActive;
    }

    private void OnBookButtonActive()
    {
        isCanUse = true;
    }

    protected override void OnUse()
    {
        choose.Open(ChooseUI.ChooseType.BookButton);
        //if (isCanUse)
        //{
        //    BookShelf_Unlock unlock = FindAnyObjectByType<BookShelf_Unlock>();
        //    if(unlock != null)
        //    {
        //        unlock.Open();
        //    }
        //}
        //else
        //{
        //    choose.Open(ChooseUI.ChooseType.BookButton);
        //    Stage1Manager.Instance.BottomTMPText = "눌러도 반응이 없다.";
        //}
    }
}
