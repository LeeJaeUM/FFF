using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_EnviromentIndex : TestBase
{
    public int a = 0;
    public List<int> b = new List<int>() { 0,1,2,3};


    protected override void OnTest1(InputAction.CallbackContext context)
    {
        a = (++a) % b.Count;
    }
    protected override void OnTest2(InputAction.CallbackContext context)
    {
    }

}
