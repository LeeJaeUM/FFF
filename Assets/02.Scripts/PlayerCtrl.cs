using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    Rigidbody rb;

    private float moveSpeed = 3f; // 플레이어의 속도

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 vec = new Vector3(Time.deltaTime * moveSpeed * h, 0, Time.deltaTime * moveSpeed * v);

        transform.Translate(vec);
    }
}
