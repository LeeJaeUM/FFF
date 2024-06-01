using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearDoor : DoorBase
{
    KeypadSystem.KeyPad keyPad;
    [SerializeField] private bool isUse = false;

    protected override void Start()
    {
        keyPad = Stage1Manager.Instance.KeyPad;
        keyPad.onAnswerCheck += DoorOpen;
    }

    public override void Interact()
    {
        isUse = !isUse;
        keyPad.gameObject.SetActive(isUse);
    }
}
