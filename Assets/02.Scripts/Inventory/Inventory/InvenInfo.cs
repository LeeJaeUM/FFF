using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvenInfo : MonoBehaviour
{
    InventoryUI inven;
    TextMeshProUGUI weightText;

    private void Awake()
    {
        inven = transform.parent.parent.GetComponent<InventoryUI>();    
        weightText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Button button = transform.GetChild(1).GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            ProduceManager produceManager = inven.produceManager;
            if (produceManager != null)
            {
                produceManager.OnOff();
            }
        });
    }

    private void Start()
    {
        inven.onWeightChange += Refresh;
    }

    private void Refresh(float weight)
    {
        string weightString = weight.ToString("F1");
        weightText.text = $"무게 : {weightString}kg";
    }
}
