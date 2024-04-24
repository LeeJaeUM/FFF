using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGimicStage3 : MonoBehaviour
{
    RaycastHit hit;

    private int MaxDistance = 3;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(Physics.Raycast(transform.position,transform.forward,out hit,MaxDistance))
        {
            if(hit.collider.CompareTag("PICTUREFRAME"))
            {
                Debug.Log("상호작용준비 완료");
            }
        }
    }
}
