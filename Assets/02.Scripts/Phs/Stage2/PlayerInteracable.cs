using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteracable : MonoBehaviour
{
    List<IInteracable> interacables;

    Camera cam;

    MouseInputAction inputAction;

    private void Awake()
    {
        inputAction = new MouseInputAction();
        interacables = new List<IInteracable>();
        cam = Camera.main;
    }

    private void OnEnable()
    {
        inputAction.Interacable.Enable();
        inputAction.Interacable.InteracableAction.performed += ObjectInteracable;
    }

    private void OnDisable()
    {
        inputAction.Interacable.InteracableAction.performed -= ObjectInteracable;   
        inputAction.Interacable.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteracable inter = other.GetComponent<IInteracable>();
        if(inter != null)
        {
            Debug.Log(other.name);
            interacables.Add(inter);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteracable inter = other.GetComponent<IInteracable>();
        if (inter != null)
        {
            Debug.Log(other.name);
            interacables.Remove(inter);
        }
    }

    private void Update()
    {
        ObjectHitCheck();
    }

    private IInteracable ObjectHitCheck()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            foreach (var inter in interacables)
            {
                if (inter == hit.collider.gameObject.GetComponent<IInteracable>() && inter.CanUse)
                {
                    //Debug.Log($"{inter}의 동작이 가능합니다.");
                    return inter;
                }
            }
        }

        return null;
    }

    private void ObjectInteracable(InputAction.CallbackContext context)
    {
        //Debug.Log("동작");
        IInteracable inter = ObjectHitCheck();
        //Debug.Log(inter != null);
        //Debug.Log(inter.CanUse);
        if (inter != null && inter.CanUse)
        {
            Debug.Log($"{inter}");
            inter.Use();
        }
    }

}
