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
}
