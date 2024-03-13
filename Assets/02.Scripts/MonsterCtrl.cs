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
    private Transform[] area; // ������ �̵� ��Ʈ ����
    [SerializeField]
    private Transform player; // �÷��̾� ��ġ
    private float chaseDuration = 10f; // ������ ������ �ð�

    private float totalTime = 30; // ���� ���� �ٲ�� �ð�
    private float nowTime = 0;
    private float chaseTimer = 0; // ������ �����ϴ� Ÿ�̸�
    private float pursuitRange = 3f; // �÷��̾ �ν��ϴ� �ð�

    private bool isMonsterMove = false; // ������ �̵� ��Ȳ
    private int current = 0; // ������ �̵� ���� ����
    private bool isChasing = false; // ���� ������ ����Ȯ��

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // �ʱ� ������ ����
        SetDestinationByIndex(current);
    }

    private void Update()
    {
        nowTime += Time.deltaTime;

        // �ð��� �帧
        if (nowTime >= totalTime)
        {
            RotateSkyLight(); 
            isMonsterMove = !isMonsterMove;
            nowTime = 0; // directional light�� ȸ���ϸ� nowTime �ʱ�ȭ

            SetNextDestination();
        }

        if (isMonsterMove == true && area != null && area.Length>0)
        {
            // �÷��̾���� �Ÿ� ���
            float distanceToPlayer=Vector3.Distance(transform.position, player.position);

            // ���� ���� ����
            if(player != null && distanceToPlayer <= pursuitRange)
            {
                isChasing = true;
                chaseTimer = 0;

                agent.SetDestination(player.position);
                anim.SetBool("IsWalk", true);
            }

            // ���� ���̸鼭 ������ �ð� �̳��� �ִ� ��� ��� ����
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
                    // ���� �������� ���������� ���� ������ ����
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

    // Dirctional Light ȸ��
    void RotateSkyLight()
    {
        if (skyLight != null)
        {
            skyLight.transform.Rotate(180f, 0, 0);
        }
    }

    // ������ �̵� ��Ʈ ����
    void SetNextDestination()
    {
        if(area!=null && area.Length>0)
        {
            current = (current + 1) % area.Length;

            // ���� �������� �̵�
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
        // �Ѿ˿� ����� �� ���
        if(other.gameObject.CompareTag("BULLET"))
        {
            anim.SetTrigger("IsDeath");

            agent.enabled = false;

            Invoke("DestroyMonster", 3f);
        }
    }

    void DestroyMonster()
    {
        Destroy(gameObject);
    }
}
