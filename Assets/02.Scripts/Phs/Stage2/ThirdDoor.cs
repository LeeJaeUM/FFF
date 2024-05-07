using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdDoor : InteracableBase
{
    Animator animator;

    readonly int Open_Hash = Animator.StringToHash("open");

    private bool isOpen = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void OnUse()
    {
        isOpen = !isOpen;
        animator.SetBool(Open_Hash, isOpen);
    }
}
