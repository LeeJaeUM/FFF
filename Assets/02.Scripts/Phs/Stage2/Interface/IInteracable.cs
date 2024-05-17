using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템을 동작시 작동하는 인터페이스
/// </summary>
public interface IInteracable
{
    /// <summary>
    /// 사용 가능한지를 확인하는 프로퍼티
    /// </summary>
    bool CanUse
    {
        get;
    }

    /// <summary>
    /// 사용시 동작하는 함수
    /// </summary>
    void Use(); 
}
