using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteracableBase : MonoBehaviour, IInteracable
{
    [SerializeField]
    bool canUse = false;

    public virtual bool CanUse
    {
        get => canUse;
    }

    public void Use()
    {
        if(CanUse)
        {
            Debug.Log("동작");
            OnUse();
        }
    }

    protected virtual void OnUse()
    {
    }
}
