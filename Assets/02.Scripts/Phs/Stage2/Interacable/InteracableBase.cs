using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteracableBase : MonoBehaviour, IInteractable
{
    protected bool CanPickUp = true;

    public void Interact()
    {
        if(CanPickUp)
        {
            Debug.Log("동작");
            OnUse();
        }
    }


    protected virtual void OnUse()
    {
    }
}
