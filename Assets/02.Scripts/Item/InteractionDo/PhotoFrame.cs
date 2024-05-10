using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoFrame : MonoBehaviour, IInteractable
{
    public GameObject secondKey;
    GameObject inventory;

    private void Awake()
    {
        
    }

    public void Interact()
    {
        //if (Inventory.HasItem("CrowBar"))
        //{
        //    // CrowBar가 있을 때 상호작용
        //    OpenFrame();
        //}
        //else
        //{
        //    // CrowBar가 없을 때 메시지 표시
        //    Debug.Log("손으로는 열 수 없다. 땔 수 있는 장비가 필요하다.");
        //}
    }

    private void OpenFrame()
    {
        // 사진액자 열기
        Debug.Log("액자가 열렸습니다.");
        // PhotoFrame 오브젝트 비활성화
        gameObject.SetActive(false);
        // SecondKey 오브젝트 활성화
        secondKey.SetActive(true);
    }
}
