using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : Component
{
    /// <summary>
    /// 이 싱글톤이 초기화되었는지 확인하기 위한 변수
    /// </summary>
    bool isInitialized = false;

    /// <summary>
    /// 종료처리에 들어갔는지 확인하기 위한 변수
    /// </summary>
    private static bool isShutdown = false;

    /// <summary>
    /// 이 싱글톤의 객체(인스턴스)
    /// </summary>
    private static T instance = null;

    /// <summary>
    /// 이 싱글톤
    /// </summary>
    public static T Instance
    {
        get
        {
            if (isShutdown)  // 종료처리에 들어갔으면
            {
                Debug.LogWarning("싱글톤이 죽었습니다.");    // 경고출력하고 
                return null;                                 // null 리턴
            }

            if (instance == null)        // 객체가 없으면
            {
                T singletion = FindAnyObjectByType<T>();            // 다른게임 오브젝트에 해당 싱글톤이 있으면
                if (singletion == null)                              // 다른 게임 오브젝트에도 이 싱글톤이 없으면
                {
                    GameObject obj = new GameObject();              // 빈 게임 오브젝트 만들고
                    obj.name = "Singleton";                         // 이름 지정한 다음
                    singletion = obj.AddComponent<T>();             // 싱클톤 컴포넌트 만들어서 추가
                }
                instance = singletion;                              // 다른 게임오브젝트에 있는 싱글톤이나 새로만든 싱글톤을 저장
                DontDestroyOnLoad(instance.gameObject);             // 씬이 사라질 때 게임오브젝트가 삭제되지 않도록 설정
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)        // 씬이 이미 배치된 다른 싱글톤이 없다.
        {
            instance = this as T;        // 첫번째를 저장
            DontDestroyOnLoad(instance.gameObject); // 씬이 사라질 때 게임오브젝트가 삭제되지 않도록 설정
        }
        else                // 
        {
            if (instance != this)
            {
                Destroy(this.gameObject);   // 나 자신을 삭제
            }
        }
    }

    protected virtual void OnEnable()
    {
        // SceneManager.sceneLoaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected virtual void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// 씬이 로드되었을 때 호출될 함수
    /// </summary>
    /// <param name="scene">씬정보</param>
    /// <param name="mode">로딩모드</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!isInitialized)
        {
            OnPreInitialize();
        }
        if (mode != LoadSceneMode.Additive)
        {
            OnInitialize();
        }
    }

    /// <summary>
    /// 싱클톤이 만들어질 때 단 한번만 호출되는 함수
    /// </summary>
    protected virtual void OnPreInitialize()
    {
        isInitialized = true;
    }

    /// <summary>
    /// 싱글톤이 만들어지고 씬이 Single로 로드될 때마다 호출될 함수(additive는 안됨)
    /// </summary>
    protected virtual void OnInitialize()
    {

    }

    private void OnApplicationQuit()
    {
        isShutdown = true;
    }
}