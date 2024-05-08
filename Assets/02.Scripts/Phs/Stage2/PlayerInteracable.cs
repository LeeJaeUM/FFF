using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteracable : MonoBehaviour
{
    List<IInteracable> interacables;

    Vector3 ScreenCenter;

    Camera cam;

    Ray ray;

    MouseInputAction inputAction;

    private void Awake()
    {
        inputAction = new MouseInputAction();
        interacables = new List<IInteracable>();
        cam = Camera.main;
        ScreenCenter = new Vector3(cam.pixelWidth * 0.5f, cam.pixelHeight * 0.5f);
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

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision.gameObject.name);
    //    IInteracable inter = collision.gameObject.GetComponent<IInteracable>();
    //    if (inter != null)
    //    {
    //        interacables.Add(inter);
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log(other.name);
    //    IInteracable inter = other.GetComponent<IInteracable>();
    //    if(inter != null)
    //    {
    //        interacables.Add(inter);
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    IInteracable inter = collision.gameObject.GetComponent<IInteracable>();
    //    if (inter != null)
    //    {
    //        interacables.Remove(inter);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    IInteracable inter = other.GetComponent<IInteracable>();
    //    if (inter != null)
    //    {
    //        interacables.Remove(inter);
    //    }
    //}

    private void Update()
    {
        ObjectHitCheck();
    }

    private IInteracable ObjectHitCheck()
    {
        //ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        ray = cam.ScreenPointToRay(ScreenCenter);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            IInteracable inter = hit.collider.gameObject.GetComponent<IInteracable>();
            if (inter != null && inter.CanUse)
            {
                Debug.Log($"{inter}의 동작이 가능합니다.");
                return inter;
            }
        }

        return null;
    }

    private void ObjectInteracable(InputAction.CallbackContext context)
    {
        IInteracable inter = ObjectHitCheck();

        if (inter != null)
        {
            Debug.Log($"{inter}");
            inter.Use();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(ray.origin, ray.direction * 1000);
    }
#endif
}
