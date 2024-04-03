using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialSelectUI : MonoBehaviour
{
    [SerializeField] Button[] selectButtons;

    private void Awake()
    {
        selectButtons = GetComponentsInChildren<Button>();
    }
}
