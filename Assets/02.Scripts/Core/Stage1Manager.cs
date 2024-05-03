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

    private void Update()
    {
        // 충돌 정보를 저장할 변수
        RaycastHit hitInfo;

        // 레이를 발사하고 충돌 검출
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, rayDistance))
        {
            // 충돌한 물체에 대한 처리
            //Debug.Log("Hit object: " + hitInfo.collider.gameObject.name);

            // 레이의 출발점에서 충돌 지점까지 라인 그리기
            Debug.DrawLine(Camera.main.transform.position, hitInfo.point, Color.red);

            IInteractable interactable = hitInfo.collider.gameObject.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
            else
            {
                //Debug.Log(" 상호작용 가능한 오브젝트가 아니다! ");
            }
        }
        else
        {
            // 레이가 아무것도 충돌하지 않은 경우
            // 레이의 최대 거리만큼 라인 그리기
            Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * rayDistance, Color.yellow);
        }
    }
}
