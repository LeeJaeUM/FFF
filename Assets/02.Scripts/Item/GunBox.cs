using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBox : MonoBehaviour
{
    public GameObject hammer;

    public GameObject gunBox;

    public Action hammerToInventory;

    bool hasHammer = false;

    public void BreakGunBox()
    {
        if (hasHammer && gunBox != null)
        {
            Destroy(gunBox);
        }
    }
}
