using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster3 : MonoBehaviour
{
    RaycastHit hit;
    [SerializeField]
    private NavMeshAgent monsterAgent; // 몬스터 네비게이션
    [SerializeField]
    private GameObject laboratoryTrap; // Laboratory에 있는 함정

    private float MaxDistance = 15f; // 몬스터의 감지 거리

    private bool isChasing = false; // 몬스터의 추격 여부
    private float chaseDuration = 10f; // 몬스터의 추격 시간

    private float chaseDelay = 2f; // 몬스터의 추격 딜레이

    private void Update()
    {
        // 몬스터가 플레이어를 추격함
        if (Physics.Raycast(transform.position, transform.forward, out hit, MaxDistance))
        {
            if(hit.collider.CompareTag("MONSTER"))
            {
                isChasing = true;

                StartCoroutine(StopMonster());
            }

            // Laboratory에서 시체와 상호작용을 할 경우 몬스터가 플레이어를 추격
            if(hit.collider.CompareTag("LABORATORYTRAP"))
            {
                Debug.Log("상호작용가능");
                if(Input.GetKeyDown("E"))
                {
                    laboratoryTrap.SetActive(true);

                    StartCoroutine(ESCTrap());
                }
            }
        }

        // isChasing이 true일 때 몬스터는 플레이어를 계속 추격함.
        if(isChasing)
        {
            monsterAgent.SetDestination(transform.position);
        }
    }

    // 몬스터관련 태그를 플레이어가 봤을 경우 몬스터는 10초간 플레이어를 지속적으로 추격함.
    private System.Collections.IEnumerator StopMonster()
    {
        yield return new WaitForSeconds(chaseDuration);
        isChasing = false;
    }

    private System.Collections.IEnumerator ESCTrap()
    {
        yield return new WaitForSeconds(chaseDelay);
        laboratoryTrap.SetActive(false);

        isChasing = true;
    }
}
