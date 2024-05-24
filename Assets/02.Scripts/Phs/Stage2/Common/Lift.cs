using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Lift : InteracableBase
{
    public float waitTime = 3.0f;
    float currTime = 0.0f;

    bool canUse = false;
    bool move = false;

    private void Start()
    {
        GameManager.Instance.buttonManager.AllAccess = OnAllAccess;
    }

    private void OnAllAccess()
    {
        canUse = true;
    }

    protected override void OnUse()
    {
        if (canUse)
        {
            StopAllCoroutines();
            StartCoroutine(LiftOn());
        }
    }

    IEnumerator LiftOn()
    {
        while(currTime > waitTime)
        {
            currTime += Time.deltaTime;
            Stage1Manager.Instance.BottomTMPText = $"{currTime:f1}";
            yield return null;
        }

        move = true;
    }

    private void FixedUpdate()
    {
        if (move)
        {
            transform.Translate(Vector3.down * 5f);
        }
    }
}
