using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanFood : MonoBehaviour
{
    public void OnUsed()
    {
        // 플레이어가 CanFood를 사용했을 때 호출 될 함수
        StartCoroutine(CanFoodUse());

        // 플레이어는 기본적으로 체력이 4로 되어있음.
        // 하루가 지날 때마다 체력이 1씩 줄어들고 0이 되면 게임오버.
        // 맵에는 총 3개의 CanFood가 있고 1개의 CanFood를 먹을 때마다 체력이 3이 증가,
        // 탈출하지 않고 총 10일을 버틸 수 있음.
    }

    /// <summary>
    /// CanFood를 사용 시 취식시간
    /// </summary>
    /// <returns></returns>
    IEnumerator CanFoodUse()
    {
        yield return new WaitForSeconds(3.0f);  // 사용 시 3초동안 대기
        Destroy(gameObject);                    // 아이템 삭제
    }
}
