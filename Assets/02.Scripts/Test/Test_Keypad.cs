using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Keypad : TestBase
{
    Keypad keypad;

    private void Start()
    {
        keypad = FindAnyObjectByType<Keypad>(); 
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        keypad.OnShowKeypad();
    }
}
