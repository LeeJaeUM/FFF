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

    [SerializeField] private int wall_HoCount = 0;
    [SerializeField] private int wall_VeCount = 0;
    [SerializeField] private int floorCount = 0;

    /// <summary>
    /// Adjuster에 사용할 카운트용 프로퍼티 
    /// </summary>
    public int FloorCount
    {
        get => floorCount;
        private set
        {
            floorCount = value;
            onChangeCount?.Invoke((int)usedDir ,floorCount);
        }
    }

    /// <summary>
    /// Adjuster에 보낼 액션 : floor가 연결되어있는지 판단한다.
    /// </summary>
    public Action<int, int> onChangeCount;

   

    private void Awake()
    {
        connectorLayer = LayerMask.GetMask("BuildObj");
        DirSet();
        CountReset();
    }

    private void OnDisable()
    {
        UpdateConnecting(true, true);

    }

    /// <summary>
    /// UsedDir을 생성될때 자동으로 설정하는 함수
    /// </summary>
    void DirSet()   
    {
        Transform parentTransform = transform.parent;
        Vector3 localPosition = parentTransform.InverseTransformPoint(transform.position);

        if (localPosition.x > 0)
            usedDir = UsedDir.Right;
        else if (localPosition.x < 0)
            usedDir = UsedDir.Left;
        else if (localPosition.z > 0)
            usedDir = UsedDir.Forward;
        else if (localPosition.z < 0)
            usedDir = UsedDir.Back;
        else if (localPosition.y > 0)
            usedDir = UsedDir.Top;
        else if (localPosition.y < 0)
            usedDir = UsedDir.Bottom;
        else
            usedDir = UsedDir.None;
    }

    /// <summary>
    /// 카운팅을 초기화하는 함수
    /// </summary>
    void CountReset()
    {
        switch (objType)
        {
            case ObjType.Floor:
                if (usedDir == UsedDir.Left || usedDir == UsedDir.Right)
                {
                    CountSetting(0, 2, 1);
                }
                else
                {
                    CountSetting(2, 0, 1);
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

    /// <summary>
    /// 카운팅을 초기화할 때 사용하는 함수
    /// </summary>
    /// <param name="ho">horizontal 벽 타일</param>
    /// <param name="ve">vertical 벽 타일</param>
    /// <param name="floor">바닥 타일</param>
    void CountSetting(int ho, int ve, int floor)
    {
        wall_HoCount = ho;
        wall_VeCount = ve;
        floorCount = floor;
    }

    /// <summary>
    /// 겹치는 Connecting이 있는 지 확인 후 어떤 타일인지 확인하여 카운트를 변경시키는 함수
    /// </summary>
    /// <param name="rootCall">중복방지용 변수  최초에 실행될 Connecting이면 true 아니면 flase</param>
    /// <param name="isDestroy">타일을 삭제할 떄 실행한건지 판단하는 변수 true면 삭제 시 실행 false면 생성 시 실행</param>
    public void UpdateConnecting(bool rootCall = false, bool isDestroy = false)
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

                if (!isDestroy)
                {
                    switch (otherConnecting.objType)
                    {
                        case ObjType.Wall_Ho: wall_HoCount--; break;
                        case ObjType.Wall_Ve: wall_VeCount--; break;
                        case ObjType.Floor: FloorCount--; break;            // Adjuster에 델리게이트를 실행함-------------------------------
                    }
                    //중복되어 무한 루프 방지
                    if (rootCall)
                        otherConnecting.UpdateConnecting();
                }
                else
                {
                    //Debug.LogWarning("삭제될떄의 작업");
                    //중복되어 무한 루프 방지
                    if (rootCall)
                    {
                        otherConnecting.CountReset();
                        otherConnecting.UpdateConnecting(false, true);
                    }
                }
            }
        }
        canBuild = true;

        if (wall_HoCount < 1)
        {
            isConnectedToWall_Ho = true;
            wall_HoCount = 0;
        }
        else
            isConnectedToWall_Ho = false;

        if (wall_VeCount < 1)
        {
            isConnectedToWall_Ve = true;
            wall_VeCount = 0;
        }
        else
            isConnectedToWall_Ve = false;

        if (floorCount < 1)            // Adjuster에 델리게이트를 실행함-------------------------------
        {
            isConnectedToFloor = true;
            FloorCount = 0;
        }
        else 
            isConnectedToFloor = false;

        if (isConnectedToFloor && isConnectedToWall_Ho && isConnectedToWall_Ve)
            canBuild = false;

    }

#if UNITY_EDITOR

    /// <summary>
    /// 설치가능한지 시각적으로 표시
    /// </summary>
    private void OnDrawGizmos()
    {
        Color gizColor = Color.white;
        if (isConnectedToFloor && isConnectedToWall_Ho && isConnectedToWall_Ve)
        {
            gizColor = Color.black;

        }
        else if (isConnectedToFloor && isConnectedToWall_Ho)
        {
            gizColor = Color.green;
        }
        else if (isConnectedToWall_Ho && isConnectedToWall_Ve)
        {
            gizColor = Color.blue;
        }
        else if (isConnectedToWall_Ve && isConnectedToFloor)
        {
            gizColor = Color.red;
        }
        Gizmos.color = gizColor;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x / 3f);
    }

#endif

}
