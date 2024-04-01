using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    NavMeshAgent agent;
    Animator anim;
    Vector3 vec;

    [SerializeField]
    private Transform skyLight; // directional light
    [SerializeField]
    private Transform[] area; // 몬스터의 이동 루트 변수
    [SerializeField]
    private Transform player; // 플레이어 위치
    [SerializeField]
    private GameObject darkWall; // 밤이 될 때마다 사라질 벽(파란색 벽)
    [SerializeField]
    private Color afternoonAmbientColor; // 낮으로 변환될 때 SkyBox 색
    [SerializeField]
    private Color nightAmbientColor; // 밤으로 변환될 때 SkyBox 색
    private float chaseDuration = 10f; // 추적을 유지할 시간

    private float totalTime = 60f; // 낮과 밤이 바뀌는 시간
    private float nowTime = 0;
    private float chaseTimer = 0; // 추적을 유지하는 타이머
    private float pursuitRange = 3f; // 플레이어를 인식하는 범위

    private bool isMonsterMove = false; // 몬스터의 이동 상황
    private int current = 0; // 몬스터의 이동 순서 변수
    private bool isChasing = false; // 추적 중인지 여부확인
    private bool isDarkWall = true; // 벽의 생성 여부

    private int maxHp = 5;
    private int stopHp = 0;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // 초기 목적지 설정
        SetDestinationByIndex(current);
    }

    private void Update()
    {
        nowTime += Time.deltaTime;

        if (nowTime >= totalTime)
        {
            isMonsterMove = !isMonsterMove;
            if(isMonsterMove)
            {
                darkWall.SetActive(false);
                RenderSettings.ambientSkyColor = nightAmbientColor;
            }
            else if(!isMonsterMove)
            {
                darkWall.SetActive(true);
                RenderSettings.ambientSkyColor = afternoonAmbientColor;
            }

            nowTime = 0;

            SetNextDestination();
        }

        if (isMonsterMove == true && area != null && area.Length>0)
        {
            // 플레이어와의 거리 계산
            float distanceToPlayer=Vector3.Distance(transform.position, player.position);

            // 추적 시작 조건
            if(player != null && distanceToPlayer <= pursuitRange)
            {
                isChasing = true;
                chaseTimer = 0;

                agent.SetDestination(player.position);
                anim.SetBool("IsWalk", true);
            }

            // 추적 중이면서 지정된 시간 이내에 있는 경우 계속 추적
            if(isChasing && chaseTimer<chaseDuration)
            {
                agent.SetDestination(player.position);
                chaseTimer += Time.deltaTime;
            }

            if (maxHp <= 0)
            {
                Debug.Log("활동을 정지합니다");
                agent.ResetPath();

                StartCoroutine(ReDestination());
            }

            else
            {
                float remainingDistance = agent.remainingDistance;

                if (remainingDistance <= agent.stoppingDistance)
                {
                    // 현재 목적지에 도착했으면 다음 목적지 설정
                    SetNextDestination();
                    isChasing = false;
                }

                anim.SetBool("IsWalk", true);
            }
        }
        else
        {
            agent.ResetPath();
            anim.SetBool("IsWalk", false);
        }
    }

    void SetNextDestination()
    {
        if(area!=null && area.Length>0)
        {
            current = (current + 1) % area.Length;

            // 다음 목적지로 이동
            SetDestinationByIndex(current);
        }
    }

    void SetDestinationByIndex(int index)
    {
        if(area != null && area.Length>index)
        {
            agent.SetDestination(area[index].position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("BULLET"))
        {
            Debug.Log("닿았습니다");
            maxHp -= 1;
        }
    }

    IEnumerator ReDestination()
    {
        yield return new WaitForSeconds(20f);

        maxHp = 5;
    }
}
