using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeypadButton : MonoBehaviour
{
    private int number;

    TextMeshProUGUI text;

    public Action<int> onInputNumber;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Initialize(int _number)
    {
        this.number = _number;
        text.text = _number.ToString();

        Button button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            onInputNumber?.Invoke(number);
        });
    }
}
