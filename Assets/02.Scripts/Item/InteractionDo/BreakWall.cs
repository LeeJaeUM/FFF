using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWall : MonoBehaviour
{
    //public void Interact()
    //{
    //    // 아이템 사용 시 동작 구현
    //    // Matches와 Dynamite이 인벤토리에 있는지 확인
    //    if (Inventory.HasItem("Matches") && Inventory.HasItem("Dynamite"))
    //    {
    //        // 특수 상호작용: 벽을 부수는 동작 구현
    //        DestroyWall();
    //    }
    //    else
    //    {
    //        // 일반 상호작용: 메시지 표시
    //        Debug.Log("더 강한 도구가 필요합니다.");
    //    }
    //}

    private void DestroyWall()
    {
        // 벽 파괴 동작 구현
        Debug.Log("벽이 파괴되었습니다!");
    }
}
