using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Android;

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
    private float chaseDuration = 10f; // 추적을 유지할 시간

    private float totalTime = 30; // 낮과 밤이 바뀌는 시간
    private float nowTime = 0;
    private float chaseTimer = 0; // 추적을 유지하는 타이머
    private float pursuitRange = 3f; // 플레이어를 인식하는 시간

    private bool isMonsterMove = false; // 몬스터의 이동 상황
    private int current = 0; // 몬스터의 이동 순서 변수
    private bool isChasing = false; // 추적 중인지 여부확인

    private int monsterLife = 30;

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

        // 시간의 흐름
        if (nowTime >= totalTime)
        {
            RotateSkyLight(); 
            isMonsterMove = !isMonsterMove;
            nowTime = 0; // directional light가 회전하면 nowTime 초기화

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

    // Dirctional Light 회전
    void RotateSkyLight()
    {
        if (skyLight != null)
        {
            skyLight.transform.Rotate(180f, 0, 0);
        }
    }

    // 몬스터의 이동 루트 설정
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

    void OnTriggerEnter(Collider other)
    {
        // 총알에 닿았을 때 사망
        if(other.gameObject.CompareTag("BULLET"))
        {
            monsterLife -= 1;
            
            if(monsterLife <= 0)
            {
                anim.SetTrigger("IsDeath");

                agent.enabled = false;

                Invoke("DestroyMonster", 3f);
            }
        }
    }

    void DestroyMonster()
    {
        Destroy(gameObject);
    }
}
