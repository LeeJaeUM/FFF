using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPickUpItem : PickUpItem
{
    private void Start()
    {
        CanPickUp = false;
    }

    public void SetCanPickUp()
    {
        CanPickUp = true;
    }
}
