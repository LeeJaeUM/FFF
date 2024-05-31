using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Medicine : InteracableBase
{
    Animator animator;

    readonly int Open_Hash = Animator.StringToHash("Open");

    private bool isOpen = false;

    public PickUpItem oldScissors;
    ChooseUI choose;

    Stage1Manager manager; 
    
    bool isPickUp = false;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        manager = Stage1Manager.Instance;
        choose = FindObjectOfType<ChooseUI>();
    }

    protected override void OnUse()
    {
        Debug.Log(isPickUp);

        if(!isPickUp)
        {
            isOpen = !isOpen;
            animator.SetBool(Open_Hash, isOpen);
            isPickUp = true;
        }
        else
        {
            Debug.Log("문이 열였다.");
            if(oldScissors != null)
            {
                manager.BottomTMPText = "가위를 찾았습니다.";
                oldScissors.GetItem();
            }
            else
            {
                if(choose != null)
                {
                    choose.Open(ChooseUI.ChooseType.FakeKey);
                    isPickUp = false;
                }
            }
        }
    }
}
