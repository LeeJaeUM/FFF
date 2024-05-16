using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteracableBase : MonoBehaviour, IInteractable
{
    
    public void Interact()
    {
        Debug.Log("동작");
        OnUse();
    }


    protected virtual void OnUse()
    {
    }
}
