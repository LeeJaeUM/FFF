using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZoe : MonoBehaviour
{
    public ItemCode itemCode;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(!GameManager.Instance.inven.UseItemCheck(itemCode))
            {
                Debug.Log($"{collision.gameObject.name}이 죽었다.");
            }
        }
    }
}
