using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    public Vector3 currentPosition;

    void Start()
    {
        // 자신의 위치를 저장
        currentPosition = transform.position;
    }
}
