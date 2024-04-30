using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPad : MonoBehaviour
{
    [SerializeField] Button[] keys = null;

    private void Awake()
    {
        keys = GetComponentsInChildren<Button>();


    }
}
