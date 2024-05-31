using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holodilnik : DoorBase
{
    bool isPickUp = false;

    [SerializeField] PickUpItem brain;
    [SerializeField] PickUpItem entrail;


    public override void Interact()
    {
        if(!isPickUp)
        {
            base.Interact();
            if(brain != null && entrail != null)
            {
                isPickUp = true;
            }
        }
        else
        {
            if(brain != null)
            {
                brain.GetItem();
            }
            else if(entrail != null)
            {
                entrail.GetItem();
                isPickUp = false;
            }
        }
    }
}
