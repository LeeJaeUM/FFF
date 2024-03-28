using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPErson : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도
    public float mouseSensitivity = 100f; // 마우스 감도

    private Rigidbody rb;
    private float xRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 숨기기
    }

    void Update()
    {
        // 플레이어 이동
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        rb.MovePosition(rb.position + move * moveSpeed * Time.deltaTime);

        // 플레이어 회전
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 수직 회전 제한
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}