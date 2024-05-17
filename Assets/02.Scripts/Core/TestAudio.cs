using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestAudio : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Debug.Log("효과음 1");
        //AudioManager.instance.PlaySfx(AudioManager.Sfx.Knock);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Debug.Log("효과음 22");
    }
}
