using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_EnviromentIndex : TestBase
{
    BlockSpwaner blockSpwaner;

    private void Start()
    {
        blockSpwaner = FindAnyObjectByType<BlockSpwaner>(); 
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        blockSpwaner.EnviromentIndex = 0;
    }
    protected override void OnTest2(InputAction.CallbackContext context)
    {
        blockSpwaner.EnviromentIndex = 1;
    }
    protected override void OnTest3(InputAction.CallbackContext context)
    {
        blockSpwaner.EnviromentIndex = 2;
    }
    protected override void OnTest4(InputAction.CallbackContext context)
    {
        blockSpwaner.EnviromentIndex = 3;
    }
    protected override void OnTest5(InputAction.CallbackContext context)
    {
        blockSpwaner.EnviromentIndex = 4;
    }
}
