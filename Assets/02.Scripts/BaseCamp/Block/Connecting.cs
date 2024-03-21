using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connecting : MonoBehaviour
{
    /// <summary>
    /// 현재 사용중인 벽 또는 토대의 방향
    /// 예시) 토대의 앞 부분이여서 기본적으로 forward를 가진 상태라면 
    /// </summary>
    //[Flags]
    //public enum UsedDir
    //{
    //    None        = 0,
    //    Forward     = 1 << 0,
    //    Back        = 1 << 1,
    //    Left        = 1 << 2,
    //    Right       = 1 << 3,
    //    Top         = 1 << 4,
    //    Bottom      = 1 << 5
    //}  
    public enum UsedDir
    {
        None        = 0,
        Forward,
        Back   ,
        Left,
        Right,
        Top,
        Bottom
    }

    public enum ObjType
    {
        None,
        Wall_Ho,
        Wall_Ve,
        Floor
    }

    public UsedDir usedDir = UsedDir.None;
    public ObjType objType = ObjType.None;  
    public bool canBuild = true;
    public bool isBuild = false;
    [SerializeField] private float connectorOverlapRadius = 1;
    [SerializeField] private LayerMask connectorLayer;

    public bool isConnectedToFloor = false;
    public bool isConnectedToWall = false;
    private void OnDrawGizmos()
    {
        Gizmos.color = isConnectedToFloor ? (isConnectedToFloor ? Color.red : Color.green) : (!isConnectedToFloor ? Color.green : Color.yellow);
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x / 3f);
    }
    public void UpdateConnecting()
    {

    }
}
