using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster3 : MonoBehaviour
{
    RaycastHit hit;
    [SerializeField]
    private NavMeshAgent monsterAgent;

    private float MaxDistance = 15f;

    private bool isChasing = false;
    private float chaseDuration = 10f;

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, MaxDistance))
        {
            if(hit.collider.CompareTag("MONSTER"))
            {
                isChasing = true;
                monsterAgent.SetDestination(transform.position);

                StartCoroutine(StopMonster());
            }
        }

        if(isChasing)
        {
            monsterAgent.SetDestination(transform.position);
        }
    }

    private System.Collections.IEnumerator StopMonster()
    {
        yield return new WaitForSeconds(chaseDuration);
        isChasing = false;
    }
}
