using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerGimic : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent monsterAgent;

    private bool isTraped = false;

    private void Update()
    {
        if(isTraped && monsterAgent != null)
        {
            monsterAgent.SetDestination(gameObject.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TRAP"))
        {
            isTraped = true;
        }
    }
}
