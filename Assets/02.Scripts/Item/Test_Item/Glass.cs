using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Glass : MonoBehaviour
{
    [SerializeField]
    private GameObject KeyItem;

    [SerializeField]
    private float dropChance = 1.0f;

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 유리병을 부수는 오브젝트인지 확인
        if (collision.gameObject.CompareTag("Player")) // 태그 확인
        {
            // 랜덤하게 아이템을 드롭할지 결정
            if (Random.value < dropChance)
            {
                DropItem();
            }

            // 유리병 파괴
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// 키 아이템을 드롭할 때 실행 될 함수 (확률에서 걸러지면 실행되지 않는다)
    /// </summary>
    void DropItem()
    {
        // "Key" 아이템을 생성하여 화면에 표시
        Instantiate(KeyItem, transform.position, Quaternion.identity);
    }
}
