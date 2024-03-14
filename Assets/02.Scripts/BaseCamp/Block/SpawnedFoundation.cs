using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedFoundation : MonoBehaviour
{
    public Vector3 currentPosition;
    [Flags]
    public enum UsedDirection
    {
        None        = 0,
        Forward     = 1 << 0,
        Back        = 1 << 1,
        Left        = 1 << 2,
        Right       = 1 << 3,
        All         = int.MaxValue
    }
    public UsedDirection usedDirection = UsedDirection.None;

    void Start()
    {
        // 자신의 위치를 저장
        currentPosition = transform.position;

        // 각 방향에 대해 레이캐스트를 쏘고 플래그 업데이트
        UpdateDirectionFlags(Vector3.forward);
        UpdateDirectionFlags(Vector3.back);
        UpdateDirectionFlags(Vector3.left);
        UpdateDirectionFlags(Vector3.right);
    }

    public bool Check_Forward()
    {
        bool result = true;
        if (usedDirection.HasFlag(UsedDirection.Forward))
        {
            result = false;
            Debug.Log("생성불가 앞");
        }
        else 
            usedDirection |= UsedDirection.Forward;

        return result;
    }
    public bool Check_Back()
    {
        bool result = true;
        if (usedDirection.HasFlag(UsedDirection.Back))
        {
            result = false;
            Debug.Log("생성불가 뒤");
        }
        else
            usedDirection |= UsedDirection.Back;

        return result;
    }
    public bool Check_Left()
    {
        bool result = true;
        if (usedDirection.HasFlag(UsedDirection.Left))
        {
            result = false;
            Debug.Log("생성불가 좌");
        }
        else
            usedDirection |= UsedDirection.Left;

        return result;
    }
    public bool Check_Right()
    {
        bool result = true;
        if (usedDirection.HasFlag(UsedDirection.Right))
        {
            result = false;
            Debug.Log("생성불가 우");
        }
        else
            usedDirection |= UsedDirection.Right;

        return result;
    }

    void UpdateDirectionFlags(Vector3 direction)
    {
        // Raycast를 쏴서 해당 방향에 SpawnedFoundation이 있는지 확인
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 2.0f))
        {
            SpawnedFoundation adjacentFoundation = hit.collider.GetComponent<SpawnedFoundation>();
            if (adjacentFoundation != null && adjacentFoundation != this)
            {
                // 방향에 따라 플래그 업데이트
                if (direction == Vector3.forward)
                    usedDirection |= UsedDirection.Forward;
                else if (direction == Vector3.back)
                    usedDirection |= UsedDirection.Back;
                else if (direction == Vector3.left)
                    usedDirection |= UsedDirection.Left;
                else if (direction == Vector3.right)
                    usedDirection |= UsedDirection.Right;
            }
        }
    }

}
