using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Monster2Ctrl : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent monsterAgent; // Stage2 몬스터의 NavMeshAgent 변수
    [SerializeField]
    private AudioSource brokenWoodSound; // 플레이어가 나무를 밟았을 경우 생기는 소리
    [SerializeField]
    private Animator monsterAnim;
    [SerializeField]
    private AudioSource rage; // 몬스터 움직일 때 사운드

    private int playerLife = 3; // 플레이어의 생명
    private bool isWoodExit = false;
    private bool isChasing = false;

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

        // 플레이어가 몬스터와 닿았을 경우 게임 오버
        if(other.gameObject.CompareTag("MONSTER"))
        {
            SceneManager.LoadScene("GameOverScene2");
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
            monsterAgent.SetDestination(gameObject.transform.position);
            monsterAnim.SetBool("isChasing", isChasing);
            rage.Play(); // 사운드 재생
        }
    }
}
