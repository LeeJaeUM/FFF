using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdDoor : InteracableBase
{
    public Animator animator;

    readonly int Open_Hash = Animator.StringToHash("open");

    private bool isOpen = false;

    public override bool CanUse { get => true; }

    private void Awake()
    {
        Transform parent = transform.parent;
        animator = parent.GetComponentInParent<Animator>();
    }

    protected override void OnUse()
    {
        Debug.Log(isOpen);
        isOpen = !isOpen;
        animator.SetBool(Open_Hash, isOpen);
    }
}
