using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Respawner : TestBase
{
    public Transform respawn;

    public Transform player;


    private void Start()
    {
        respawn = transform;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Debug.Log("test");
        player.position = respawn.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("뭐라도 닿음");
        if (other.CompareTag("Player"))
        {
            Debug.Log("testEnter");
            other.gameObject.transform.position = respawn.position;
        }
    }

    
}
