using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerGimic : MonoBehaviour
{
    [SerializeField]
    private GameObject soundMonsterPrefab; // 몬스터 프리팹을 저장하는 변수명 수정
    [SerializeField]
    private Transform[] monsterSpawnZone; // 몬스터의 스폰 구역

    private GameObject soundMonsterInstance; // 몬스터 인스턴스를 저장하는 변수 추가

    private bool isTrapped = false;

    private void Update()
    {
        // isTrapped 조건 추가, soundMonsterInstance가 null이 아닌지도 확인
        if (isTrapped && soundMonsterInstance != null)
        {
            NavMeshAgent monsterAgent = soundMonsterInstance.GetComponent<NavMeshAgent>();
            if (monsterAgent != null)
            {
                monsterAgent.SetDestination(gameObject.transform.position);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TRAP"))
        {
            if (!isTrapped)
            {
                // 랜덤한 스폰 지점 선택
                Transform spawnPoint = monsterSpawnZone[Random.Range(0, monsterSpawnZone.Length)];

                // 몬스터 생성 및 인스턴스 변수에 할당
                soundMonsterInstance = Instantiate(soundMonsterPrefab, spawnPoint.position, Quaternion.identity);
                isTrapped = true;
            }
        }

        if(other.gameObject.CompareTag("HOME"))
        {
            DestroyMonster();
        }
    }

    private void DestroyMonster()
    {
        if(soundMonsterInstance != null)
        {
            Destroy(soundMonsterInstance);
            isTrapped = false;
        }
    }
}
