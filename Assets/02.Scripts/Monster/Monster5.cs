using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster5 : MonoBehaviour
{
    private float shake = 20f;

    private Quaternion rotation;

    private void Start()
    {
        rotation = transform.rotation;
    }

    private void Update()
    {
        Quaternion currentRotation = transform.rotation;

        float rotationChangeMagnitude=Quaternion.Angle(currentRotation, rotation);

        if(rotationChangeMagnitude > shake)
        {
            Debug.Log("게임 오버");

            rotation = currentRotation;
        }
    }
}
