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
    public bool isConnectedToWall_Ho = false;
    public bool isConnectedToWall_Ve = false;

    [SerializeField] private int Wall_HoCount = 0;
    [SerializeField] private int Wall_VeCount = 0;
    [SerializeField] private int FloorCount = 0;


    private void Awake()
    {
        connectorLayer = LayerMask.GetMask("BuildObj");
        switch (objType)
        {
            case ObjType.Floor:
                if(usedDir == UsedDir.Left || usedDir == UsedDir.Right)
                {
                    CountSetting(0, 2, 1);
                }
                else
                {
                    CountSetting(2,0,1);
                }
                break;
            case ObjType.Wall_Ho:
                if (usedDir == UsedDir.Top || usedDir == UsedDir.Bottom)
                {
                    CountSetting(1, 0, 2);
                }
                else
                {
                    CountSetting(1, 2, 0);
                }
                break;
            case ObjType.Wall_Ve:
                if (usedDir == UsedDir.Top || usedDir == UsedDir.Bottom)
                {
                    CountSetting(0, 1, 2);
                }
                else
                {
                    CountSetting(2, 1, 0);
                }
                break;
        }
    }

    void CountSetting(int ho, int ve, int floor)
    {
        Wall_HoCount = ho;
        Wall_VeCount = ve;
        FloorCount = floor;
    }

    private void OnDrawGizmos()
    {
        Color gizColor = Color.green;
        if(isConnectedToFloor )
        {
            if (isConnectedToWall_Ho || isConnectedToWall_Ve)
            {
                gizColor = Color.red;
            }

        }
        Gizmos.color = gizColor;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x / 3f);
    }
    public void UpdateConnecting(bool rootCall = false)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, connectorOverlapRadius * 0.5f, connectorLayer);


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
                    case ObjType.Wall_Ho: Wall_HoCount--;   break;
                    case ObjType.Wall_Ve: Wall_VeCount--;   break;
                    case ObjType.Floor:     FloorCount--;   break;
                }
                //중복되어 무한 루프 방지
                if(rootCall)    
                    otherConnecting.UpdateConnecting();
            }
        }
        canBuild = true;

        if (Wall_HoCount < 1)
            isConnectedToWall_Ho = true;
        if (Wall_VeCount < 1)
            isConnectedToWall_Ve = true;
        if (FloorCount < 1) 
            isConnectedToFloor = true;

        if (isConnectedToFloor && isConnectedToWall_Ho && isConnectedToWall_Ve)
            canBuild = false;

    }
}
