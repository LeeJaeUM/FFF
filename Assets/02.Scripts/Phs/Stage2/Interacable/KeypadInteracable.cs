using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadInteracable : InteracableBase
{
    public Keypad keypad;

    private void Awake()
    {
        keypad = FindObjectOfType<Keypad>();
    }

    public override bool CanUse => true;

    protected override void OnUse()
    {
        keypad.OnShowKeypad();
    }
}
