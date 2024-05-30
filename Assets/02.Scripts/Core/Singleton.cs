using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : Component
{
    bool isInitialized = false;
    private static bool isShutdown = false;
    private static T instance = null;

    public static T Instance
    {
        get
        {
            if (isShutdown)
            {
                Debug.LogWarning("싱글톤이 죽었습니다.");
                return null;
            }

            if (instance == null)
            {
                T singletion = FindAnyObjectByType<T>();
                if (singletion == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "Singleton";
                    singletion = obj.AddComponent<T>();
                }
                instance = singletion;
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    protected virtual void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected virtual void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

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

    protected virtual void OnPreInitialize()
    {
        isInitialized = true;
    }

    protected virtual void OnInitialize()
    {
    }

    private void OnApplicationQuit()
    {
        isShutdown = true;
    }
}
