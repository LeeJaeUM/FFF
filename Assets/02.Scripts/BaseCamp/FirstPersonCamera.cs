using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform playerTransform;
    public float sensitivity = 2f;

    float rotationX = 0f;

    private void Start()
    {
        playerTransform = transform;
    }

    void Update()
    {
        // 마우스 입력 감지
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // 카메라 회전 처리
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // 카메라가 너무 위로 올라가거나 아래로 내려가지 않도록 제한

        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f); // X 축 회전
        playerTransform.Rotate(Vector3.up * mouseX); // 플레이어 오브젝트의 Y 축 회전
    }
}
