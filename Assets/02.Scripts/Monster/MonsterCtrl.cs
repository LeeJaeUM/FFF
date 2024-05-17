using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MonsterCtrl : MonoBehaviour
{
    NavMeshAgent agent;
    NavMeshSurface surface;
    Animator anim;
    Vector3 vec;
    AudioSource audio;

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
    private float chaseDuration = 20f; // 추적을 유지할 시간

    private float totalTime = 120f; // 낮과 밤이 바뀌는 시간
    private float nowTime = 0;
    private float chaseTimer = 0; // 추적을 유지하는 타이머
    private float pursuitRange = 5f; // 플레이어를 인식하는 범위

    private bool isMonsterMove = false; // 몬스터의 이동 상황
    private int current = 0; // 몬스터의 이동 순서 변수
    private bool isChasing = false; // 추적 중인지 여부확인

    private int maxHp = 5; // 몬스터의 기존 Hp 

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audio=GetComponent<AudioSource>();

        // 초기 목적지 설정
        SetDestinationByIndex(current);
    }

    private void Update()
    {
        nowTime += Time.deltaTime;

        // 시간의 흐름에 따라 낮밤 조절
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

                audio.Play(); // 사운드 재생
                agent.SetDestination(player.position);
            }

            // 추적 중이면서 지정된 시간 이내에 있는 경우 계속 추적
            if(isChasing && chaseTimer<chaseDuration)
            {
                agent.SetDestination(player.position);
                chaseTimer += Time.deltaTime;
            }

            // 몬스터의 Hp가 0이 되었을 때 일시 정지
            if (maxHp <= 0)
            {
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

    // 몬스터의 맵 랜덤 이동
    void SetDestinationByIndex(int index)
    {
        if(area != null && area.Length>index)
        {
            agent.SetDestination(area[index].position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 총알에 맞았을 경우 체력 감소
        if(other.gameObject.CompareTag("BULLET"))
        {
            maxHp -= 1;
        }

        // 플레이어와 닿았을 경우 게임 오버 씬으로 이동
        if (other.gameObject.CompareTag("PLAYER"))
            SceneManager.LoadScene("GameOverScene1");
    }

    IEnumerator ReDestination()
    {
        yield return new WaitForSeconds(20f);

        maxHp = 5; // 체력 초기화
    }
}
