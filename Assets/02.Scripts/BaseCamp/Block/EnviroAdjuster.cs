using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviroAdjuster : MonoBehaviour
{
    const float halfWidth = 1.5f;

    /// <summary>
    /// 스포너에 리턴할 위치값
    /// </summary>
    private Vector3 centerVec = Vector3.zero;
    public Vector3 CenterVec
    {
        get => centerVec;
        private set
        {
            centerVec = value;
        }
    }


    //현재 연결된 floor가 있는 지 판단한다
    public bool isConnect = false;

    //이 floor의 connecting을 수집 순서대로 left, right, forward, back이 들어옴
    [SerializeField] Connecting[] connectings = null;

    /// <summary>
    /// connecting에서 사용중인 enum타입의 값을 int로 받아서 어느 위치인지 확인
    /// </summary>
    public enum UsedDir
    {
        None = 0,
        Forward,
        Back,
        Left,
        Right,
        Top,
        Bottom
    }


    private void Start()
    {
        Transform parent = transform.parent;
        connectings = parent.GetComponentsInChildren<Connecting>();

        for (int i = 0; i < connectings.Length; i++)
        {
            connectings[i].onChangeCount += OnChangeCount;
        }

        //자신의 위치를 설정
        CenterVec = transform.position;
    }

    /// <summary>
    /// 연결된 floor체크 함수. 
    /// </summary>
    /// <param name="usedDir">현재 액션을 날린 바닥의 위치</param>
    /// <param name="count">0이면 연결됨 1이면 연결되어있지 않음이다</param>
    private void OnChangeCount(int usedDir,int count)
    {
        if(count == 0)
        {

        }
        else
        {

        }

        //1 = forward, 2 = back, 3 = left, 4 = right
        switch (usedDir)
        {
            case 1: 
                centerVec = Vector3.zero;
                break;
            case 2:
                centerVec = Vector3.zero;
                break;
            case 3:
                centerVec = Vector3.zero;
                break;
            case 4:
                centerVec = Vector3.zero;
                break;
        }
    }
}
