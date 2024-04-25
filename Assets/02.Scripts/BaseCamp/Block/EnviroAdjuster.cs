using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviroAdjuster : MonoBehaviour
{

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

    //이 floor의 connecting을 수집 순서대로 left, right, forward, back이 들어옴
    [SerializeField] Connecting[] connectings = null;

    public float lengthMultiX = 1.5f;
    public float lengthMultiZ = 1.5f;


    //현재 연결된 floor가 있는 지 판단한다
    public bool isConForward = false;
    public bool isConBack = false;
    public bool isConLeft = false;
    public bool isConRight = false;


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
        //연결될때 날라온 액션
        if(count == 0)
        {
            // connecting에서 사용중인 enum타입의 값을 int로 받아서 어느 위치인지 확인
            // 1 = forward, 2 = back, 3 = left, 4 = right
            switch (usedDir)
            {
                case 1: isConForward = true;
                    break;
                case 2: isConBack = true;
                    break;
                case 3: isConLeft = true;
                    break;
                case 4: isConRight = true;
                    break;
            }
        }
        // 연결 해제 될 때 날아온 액션
        else
        {
            // 1 = forward, 2 = back, 3 = left, 4 = right
            switch (usedDir)
            {
                case 1:
                    isConForward = false;
                    break;
                case 2:
                    isConBack = false;
                    break;
                case 3:
                    isConLeft = false;
                    break;
                case 4:
                    isConRight = false;
                    break;
            }
        }

    }


    //public enum UsedDir
    //{
    //    None = 0,
    //    Forward,
    //    Back,
    //    Left,
    //    Right,
    //    Top,
    //    Bottom
    //}

}
