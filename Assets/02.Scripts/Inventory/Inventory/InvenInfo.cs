using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InvenInfo : MonoBehaviour
{
    TextMeshProUGUI weightText;
    TextMeshProUGUI priceText;

    private void Awake()
    {
        weightText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        priceText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameManager.Instance.inven.onWeightChange += Refresh;
    }

    private void Refresh(float weight)
    {
        weightText.text = $"무게 : {weight}kg";
    }
}
