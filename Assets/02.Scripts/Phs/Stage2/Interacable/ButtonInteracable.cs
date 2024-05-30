using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteracable : InteracableBase
{
    Animator animator;

    private Keypad keypad;

    readonly int buttonAction = Animator.StringToHash("Button");

    public override bool CanUse => true;

    private bool isSave = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        keypad = FindObjectOfType<Keypad>();
    }

    private void Start()
    {
        keypad.onNumberCheck += SetSave;
    }

    private void SetSave(bool save)
    {
        isSave = save;
    }

    protected override void OnUse()
    {
        if(keypad != null)
        {
            if(isSave)
            {
                Debug.Log("작동");
            }
            else
            {
                Debug.Log("경보");
            }
        }
    }
}
