using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster4 : MonoBehaviour
{
    [SerializeField]
    private GameObject monster; // 몬스터 오브젝트
    [SerializeField]
    private GameObject startWall; // 부스에서 나오면 생기는 벽
    [SerializeField]
    private GameObject goalWall; // 부스에 도착하면 생기는 벽

    RaycastHit hit;

    private float distance = 10f; // 몬스터 감지 거리

    private void Start()
    {
        startWall.SetActive(false);
    }

    private void Update()
    {
        // 뒤를 돌아봤을 경우에 게임오버
        if(Physics.Raycast(transform.position,transform.forward,out hit,distance))
        {
            if(hit.collider.CompareTag("MONSTER"))
            {
                Debug.Log("게임 오버");
            }
        }

        // 몬스터가 플레이어의 뒤에 지속적으로 있음
        Vector3 vec = new Vector3(0, 1, -4);

        monster.transform.position = gameObject.transform.position + vec;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("HOME"))
        {
            monster.SetActive(false);
            goalWall.SetActive(true);
            startWall.SetActive(false);
        }

        if(other.gameObject.CompareTag("ROOM"))
        {
            monster.SetActive(true);
            startWall.SetActive(true);
        }

        if(other.gameObject.CompareTag("GOAL"))
        {
            goalWall.SetActive(false);
        }
    }
}
