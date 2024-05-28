using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster2Ctrl : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent monsterAgent; // Stage2 몬스터의 NavMeshAgent 변수
    [SerializeField]
    private AudioSource brokenWoodSound; // 플레이어가 나무를 밟았을 경우 생기는 소리
    [SerializeField]
    private Animator monsterAnim;

    private int playerLife = 3; // 플레이어의 생명
    private bool isWoodExit = false;
    private bool isChasing = false;

    public Transform target;

    private void Awake()
    {
        monsterAgent = GetComponent<NavMeshAgent>();
        monsterAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        ButtonManager manager = FindAnyObjectByType<ButtonManager>();
        manager.onWarning += HpDown;

        ChooseUI choose = FindAnyObjectByType<ChooseUI>();
        choose.onWarning += HpDown;

        BookShelf_Unlock bookShelf = FindAnyObjectByType<BookShelf_Unlock>();
        bookShelf.onWarning += HpDown;

        Test_Inventory test = FindAnyObjectByType<Test_Inventory>();
        if(test != null)
        {
            test.onWarning += HpDown;
        }
    }

    private void HpDown()
    {
        playerLife--;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 BrokenTable을 밟았을 경우
        if(other.gameObject.CompareTag("WOODTRAP"))
        {
            if(!isWoodExit)
            {
                //brokenWoodSound.Play();
                playerLife--; // 플레이어의 생명이 1깎임
                Debug.Log(playerLife);
                isWoodExit = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어가 BrokenTable에서 중복 충돌을 방지하기 위한 코드
        if(other.gameObject.CompareTag("WOODTRAP"))
        {
            if(isWoodExit)
            {
                isWoodExit = false;
            }
        }
    }

    private void Update()
    {
        // playerLife가 0이 됬을 경우 몬스터가 플레이어를 추적한다.
        if(playerLife<=0)
        {
            isChasing = true;
            monsterAgent.SetDestination(target.position);
            monsterAnim.SetBool("isChasing", isChasing);
        }
    }
}
