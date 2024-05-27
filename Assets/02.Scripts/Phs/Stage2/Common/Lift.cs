using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Lift : InteracableBase
{
    public float moveSpeed = 5.0f;
    public float waitTime = 3.0f;
    float currTime = 0.0f;

    bool canUse = false;
    bool move = false;

    PersonController controller;
    Rigidbody rigid;

    private void Start()
    {
        controller = FindAnyObjectByType<PersonController>();
        rigid = GetComponent<Rigidbody>();
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
        while(currTime < waitTime)
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
            //transform.Translate(Time.fixedDeltaTime * Vector3.down * 3f);
            rigid.MovePosition(rigid.position + (Time.fixedDeltaTime * moveSpeed * Vector3.down));
            controller.transform.Translate(Time.fixedDeltaTime * moveSpeed * Vector3.down);
        }
    }
}
