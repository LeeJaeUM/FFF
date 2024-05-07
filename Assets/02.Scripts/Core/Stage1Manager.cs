using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stage1Manager : MonoBehaviour
{
    // 싱글톤 인스턴스를 저장할 정적 변수
    private static Stage1Manager instance;

    // StageManager 인스턴스에 대한 전역 액세스 포인트를 제공하는 프로퍼티
    public static Stage1Manager Instance
    {
        get
        {
            // 인스턴스가 없는 경우 새로운 인스턴스 생성
            if (instance == null)
            {
                instance = FindObjectOfType<Stage1Manager>();

                // Scene에 StageManager가 없는 경우 에러 메시지 출력
                if (instance == null)
                {
                    Debug.LogError("Scene에 Stage1Manager가 존재하지 않습니다.");
                }
            }
            return instance;
        }
    }

    [SerializeField] float rayDistance = 10f; // 레이의 최대 거리

    public TextMeshProUGUI bottomTMP;


    private void Awake()
    {
        // 다른 Scene으로 넘어가더라도 StageManager 인스턴스가 파괴되지 않도록 함
        //DontDestroyOnLoad(gameObject);

        // 인스턴스가 이미 있고 현재 인스턴스와 다른 경우, 현재 인스턴스 파괴
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        Transform child = transform.GetChild(0);
        child = child.GetChild(0);
        bottomTMP = child.GetComponent<TextMeshProUGUI>();

    }

    [SerializeField] private KeyPad keyPad;
    public KeyPad KeyPad
    {
        get
        {
            if(keyPad == null)
            {
                Debug.Log("ds");
                keyPad = FindObjectOfType<KeyPad>(true);
            }
            return keyPad;
        }
    }

    [SerializeField] private TipsUI tipsUI;
    public TipsUI TipsUI
    {
        get
        {
            if (tipsUI == null)
            {
                tipsUI = FindAnyObjectByType<TipsUI>();
            }
            return tipsUI;
        }
    }

}
