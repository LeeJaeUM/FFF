using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGimic : MonoBehaviour
{
    public Transform[] moveGround;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("MOVEBLOCK"))
        {
            if (moveGround.Length > 0)
            {
                moveGround[0].Translate(Vector2.right * 3);
                moveGround[1].Translate(Vector2.right * 3);
            }
        }
    }
}
