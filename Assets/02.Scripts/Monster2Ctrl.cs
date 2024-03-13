using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster2Ctrl : MonoBehaviour
{
    NavMeshAgent agent;

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private Collider player;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
}
