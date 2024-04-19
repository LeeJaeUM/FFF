using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField]
    GameObject brokenBottlePrefab;
    [SerializeField]
    GameObject keyItem;

    [SerializeField]
    float forceMagnitude = 5.0f;
    [SerializeField]
    float threshold = 5.0f;
    [SerializeField]
    float dropChance = 1.0f;

    Rigidbody rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update() // 테스트
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            if (rigid != null)
            {
                rigid.AddForce(transform.up * forceMagnitude, ForceMode.Impulse);
            }

            //Explode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float force = collision.impulse.magnitude;
        Debug.Log($"Force : {force}");

        if(force >=  threshold)
        {
            // 랜덤하게 아이템을 드롭할지 결정
            if (Random.value < dropChance)
            {
                DropItem();
            }
            Explode();
        }
        
    }

    void Explode()
    {
        if(rigid != null)
        {
            GameObject brokenBottle = Instantiate(brokenBottlePrefab, this.transform.position, Quaternion.identity);
            brokenBottle.GetComponent<BrokenBottle>().RandomVelocities();
            Destroy(gameObject);
        }
        //GameObject brokenBottle = Instantiate(brokenBottlePrefab, this.transform.position, Quaternion.identity);
        //brokenBottle.GetComponent<BrokenBottle>().RandomVelocities();
        //Destroy(gameObject);
    }

    /// <summary>
    /// 키 아이템을 드롭할 때 실행 될 함수 (확률에서 걸러지면 실행되지 않는다)
    /// </summary>
    void DropItem()
    {
        // "Key" 아이템을 생성하여 화면에 표시
        Instantiate(keyItem, transform.position, Quaternion.identity);
    }
}
