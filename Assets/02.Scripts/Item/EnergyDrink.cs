using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrink : MonoBehaviour
{
    /// <summary>
    /// 에너지드링크 효과 지속 시간
    /// </summary>
    public float duration = 3.0f;


    private ActionController controller;

    public Action OnGetDrink;
}