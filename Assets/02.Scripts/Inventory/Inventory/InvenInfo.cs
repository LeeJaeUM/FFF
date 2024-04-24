using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvenInfo : MonoBehaviour
{
    TextMeshProUGUI weightText;

    private void Awake()
    {
        weightText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Button button = transform.GetChild(1).GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            ProduceManager produceManager = GameManager.Instance.inven.produceManager;
            if (produceManager != null)
            {
                produceManager.OnOff();
            }
        });
    }

    private void Start()
    {
        GameManager.Instance.inven.onWeightChange += Refresh;
    }

    private void Refresh(float weight)
    {
        string weightString = weight.ToString("F1");
        weightText.text = $"무게 : {weightString}kg";
    }
}
