using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 Door가 공통으로 가지고있어야 할 기능들
/// </summary>
public class DoorBase : MonoBehaviour
{
    [SerializeField]
    

    protected virtual void Open()
    {
    }
}
