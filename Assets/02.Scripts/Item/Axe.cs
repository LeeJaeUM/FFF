using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Axe의 보유 여부를 관리하는 클래스
/// </summary>
public class Axe : MonoBehaviour
{
    /// <summary>
    /// Axe를 가지고있는지 확인하는 변수
    /// </summary>
    private bool hasAxe = false;

    /// <summary>
    /// hasAxe의 프로퍼티
    /// </summary>
    public bool HasAxe
    {
        get => hasAxe;
        set
        {
            if (hasAxe != value)
            {
                hasAxe = value;
                onHasAxeChanged?.Invoke(hasAxe);
            }
        }
    }
    public Action<bool> onHasAxeChanged;
}
