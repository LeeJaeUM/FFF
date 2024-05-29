using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : RecycleObject
{
    public GameObject originalPrefab;
    public int poolSize = 64;
    T[] pool;
    Queue<T> readyQueue;

    private void Awake()
    {
        Initialized();
    }

    public void Initialized()
    {
        if (pool == null)
        {
            pool = new T[poolSize];
            readyQueue = new Queue<T>(poolSize);

            GenerateObjects(0, poolSize, pool);
        }
        else
        {
            foreach (T obj in pool)
            {
                if (obj != null) // 수정: 객체가 null인지 확인
                {
                    obj.gameObject.SetActive(false);
                }
            }
        }
    }

    public T GetObject(Vector3? position = null, Vector3? eulerAngle = null)
    {
        while (readyQueue.Count > 0)  // 수정: 레디큐가 비어있지 않은 동안 반복
        {
            T comp = readyQueue.Dequeue();
            if (comp != null && comp.gameObject != null) // 수정: 객체와 게임 오브젝트가 null인지 확인
            {
                comp.gameObject.SetActive(true);
                comp.transform.position = position.GetValueOrDefault();
                comp.transform.eulerAngles = eulerAngle.GetValueOrDefault();
                OnGetObject(comp);
                return comp;
            }
        }

        ExpandPool();
        return GetObject(position, eulerAngle);
    }

    protected virtual void OnGetObject(T component)
    {
    }

    void ExpandPool()
    {
        Debug.LogWarning($"{gameObject.name} 풀 사이즈 증가. {poolSize} -> {poolSize * 2}");
        int newSize = poolSize * 2;
        T[] newPool = new T[newSize];
        for (int i = 0; i < poolSize; i++)
        {
            newPool[i] = pool[i];
        }

        GenerateObjects(poolSize, newSize, newPool);
        pool = newPool;
        poolSize = newSize;
    }

    void GenerateObjects(int start, int end, T[] results)
    {
        for (int i = start; i < end; i++)
        {
            GameObject obj = Instantiate(originalPrefab, transform);
            obj.name = $"{originalPrefab.name}_{i}";

            T comp = obj.GetComponent<T>();
            if (comp != null)
            {
                comp.onDisable += () => readyQueue.Enqueue(comp);
                results[i] = comp;
                obj.SetActive(false);
            }
        }
    }
}
