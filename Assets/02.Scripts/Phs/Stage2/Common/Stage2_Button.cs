using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_Button : InteracableBase
{
    Animator animator;

    Stage2_Keypad _keypad;

    readonly int Use = Animator.StringToHash("Button");

    public int ButtonID;
    bool _isSuccess;
    [HideInInspector] public bool canUse = true;

    public Action<int> onTrigger;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        _keypad = FindObjectOfType<Stage2_Keypad>();
    }

    private void Start()
    {
        _keypad.onSuccess += OnKeypadAnwser;
    }

    private void OnKeypadAnwser(bool isSuccess)
    {
        _isSuccess = isSuccess;
    }

    protected override void OnUse()
    {
        if (canUse)
        {
            animator.SetTrigger(Use);

            if (!_isSuccess)
            {
                Stage1Manager.Instance.BottomTMPText = "경보";
            }

            onTrigger?.Invoke(ButtonID);

            canUse = false;
        }
        else
        {
            Stage1Manager.Instance.BottomTMPText = "이미 작동된다.";
        }
    }
}
