using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteracableBase : MonoBehaviour, IInteracable
{
    bool canUse = false;

    public virtual bool CanUse => canUse;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(other.name);
            canUse = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log(other.name);
            canUse = false;
        }
    }

    public void Use()
    {
        if(canUse)
        {
            Debug.Log("동작");
            OnUse();
        }
    }

    protected virtual void OnUse()
    {
    }
}
