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
    private GameObject soundMonster2Prefab; // 몬스터2 프리팹을 저장하는 변수명 수정
    [SerializeField]
    private GameObject soundMonster3Prefab; // 몬스터 3 프리팹을 저장흐는 변수명 수정
    [SerializeField]
    private Transform[] monsterSpawnZone; // 몬스터의 스폰 구역

    private GameObject soundMonsterInstance; // 몬스터 인스턴스를 저장하는 변수 추가
    private GameObject soundMonster2Instance; // 몬스터2 인스턴스를 저장하는 변수 추가
    private GameObject soundMonster3Instance; // 몬스터3 인스턴스를 저장하는 변수 추가

    private bool isTrapped = false;
    private bool isTrapped2 = false;
    private bool isTrapped3 = false;

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

        if(isTrapped2 && soundMonster2Instance != null)
        {
            NavMeshAgent monsterAgent2 = soundMonster2Instance.GetComponent<NavMeshAgent>();
            if(monsterAgent2 != null)
            {
                monsterAgent2.SetDestination(gameObject.transform.position);
            }
        }

        if(isTrapped3 && soundMonster3Instance != null)
        {
            NavMeshAgent monsterAgent3 = soundMonster3Instance.GetComponent<NavMeshAgent>();
            if(monsterAgent3 != null)
            {
                monsterAgent3.SetDestination(gameObject.transform.position);
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

        if(other.gameObject.CompareTag("TRAP2"))
        {
            if(!isTrapped2)
            {
                // 랜덤한 스폰 지점 선택
                Transform spawnPoint = monsterSpawnZone[Random.Range(0, monsterSpawnZone.Length)];

                // 몬스터 생성 및 인스턴스 변수에 할당
                soundMonster2Instance = Instantiate(soundMonster2Prefab, spawnPoint.position, Quaternion.identity);
                isTrapped2 = true;
            }
        }

        if(other.gameObject.CompareTag("TRAP3"))
        {
            if(!isTrapped3)
            {
                // 랜덤한 스폰 지점 선택
                Transform spawnPoint = monsterSpawnZone[Random.Range(0, monsterSpawnZone.Length)];

                // 몬스터 생성 및 인스턴스 변수에 할당
                soundMonster3Instance = Instantiate(soundMonster3Prefab, spawnPoint.position, Quaternion.identity);
                isTrapped3 = true;
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
