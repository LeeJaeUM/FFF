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
    [SerializeField] private float connectorOverlapRadius = 1;
    [SerializeField] private LayerMask connectorLayer;

    public bool canBuild = true;
    public bool isConnectedToFloor = false;
    public bool isConnectedToWall = false; 
    [SerializeField] private bool canConnectToFloor = true;
    [SerializeField] private bool canConnectToWall = true;

    private void Awake()
    {
        connectorLayer = LayerMask.GetMask("BuildObj");
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = isConnectedToFloor ? (isConnectedToFloor ? Color.red : Color.green) : (!isConnectedToFloor ? Color.green : Color.yellow);
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x / 3f);
    }
    public void UpdateConnecting(bool rootCall = false)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, connectorOverlapRadius * 0.5f, connectorLayer);

        //연결되어있지 않다면 연결 가능하다고 표시
        canConnectToFloor = !isConnectedToFloor;
        canConnectToWall = !isConnectedToWall;

        foreach (Collider collider in colliders)
        {   
            //현재 오브젝트와 같다면 반복을 건너뛴다.
            if (collider.GetInstanceID() == GetComponent<Collider>().GetInstanceID())
            {
                continue;
            }
            //같은 레이어라면 = Connectingㅣ라면
            if (gameObject.layer == collider.gameObject.layer)
            {
                Connecting otherConnecting = collider.GetComponent<Connecting>();
                switch (otherConnecting.objType)
                {
                    case ObjType.Wall_Ho: isConnectedToWall = true;   break;
                    case ObjType.Wall_Ve: isConnectedToWall = true; break;
                    case ObjType.Floor: isConnectedToFloor = true;
                        break;
                }
                //중복되어 무한 루프 방지
                if(rootCall)    
                    otherConnecting.UpdateConnecting();
            }
        }
        canBuild = true;

        if(isConnectedToFloor && isConnectedToWall)
            canBuild = false;

    }
}
