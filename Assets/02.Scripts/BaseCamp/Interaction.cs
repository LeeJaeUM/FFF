using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InteractAction : MonoBehaviour
{
    public float interactDistance = 5.0f; // 상호작용 가능한 최대 거리


    void Update()
    {
        // brain.

        RaycastHit hit; // Ray에 부딪힌 물체 정보를 저장할 변수

        // Cinemachine Virtual Camera를 통해 Ray 발사
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));

        // Ray를 Scene 창에 그림
        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);

        // Ray에 부딪힌 물체 정보를 저장
        if (Physics.Raycast(ray, out hit, interactDistance))
        {

        }
    }
}