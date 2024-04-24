using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstKeyDoor : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        //if(Inventory.HasItem("Key") && Inventory.HasItem("Hammer"))
        //{
        //    // 열쇠와 망치 둘 다 있을 땐 문을 열기
        //    OpenDoor();
        //}
        //else if (Inventory.HasItem("Hammer") && !Inventory.HasItem("key"))
        //{
        //    // 망치만 있을 땐 부수기
        //    BreakDoor();
        //}
        //else if (Inventory.HasItem("Key") && !Inventory.HasItem("Hammer"))
        //{
        //    // 열쇠만 있을 때 문을 열기
        //    OpenDoor();
        //}
        //else
        //{
        //    // 둘 다 없을 땐 아무 작업도 수행하지 않음
        //    Debug.Log("열쇠가 없어서 문을 열거나 부술 수 없습니다.");
        //}
    }

    /// <summary>
    /// 문을 열 때 호출 될 함수
    /// </summary>
    private void OpenDoor()
    {
        // 문을 열기
        Debug.Log("문이 열렸습니다.");
        // 문 오브젝트의 상태 변경 등 구현
    }

    private void BreakDoor()
    {
        // 망치로 문을 부수기
        Debug.Log("문을 부수는 소리가 들렸습니다.");
        // 문 오브젝트의 상태 변경 등 구현
    }
}
