using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Medicine : InteracableBase
{
    Animator animator;

    readonly int Open_Hash = Animator.StringToHash("Open");

    private bool isOpen = false;

    public override bool CanUse => true;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    protected override void OnUse()
    {
        isOpen = !isOpen;
        animator.SetBool(Open_Hash, isOpen);
    }
}
