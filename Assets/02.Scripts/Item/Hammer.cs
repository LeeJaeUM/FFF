using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    // 합쳐지면, 갖고있다고 알리기

    bool hasHammer = false;

    /// <summary>
    /// Hammer가 합쳐지면, 실행될 델리게이트
    /// </summary>
    public Action onHasHammer;

    /// <summary>
    /// Hammer를 합칠 때 호출 될 함수
    /// </summary>
    public void mergeHammer()
    {
        hasHammer = true;
        onHasHammer?.Invoke();
    }

}
