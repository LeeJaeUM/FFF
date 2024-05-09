using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearDoor : DoorBase
{
    KeyPad keyPad;
    [SerializeField] private bool isUse = false;

    private void Start()
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
