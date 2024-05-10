using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Keypad : MonoBehaviour
{
    KeypadButton[] buttons;
    Button okButton;
    Button cancelButton;

    TextMeshProUGUI[] Texts;
    CanvasGroup canvas;

    int[] numbers = new int[4];

    private int number = 0;

    public int Number
    {
        get => number; 
        set
        {
           if(number != value)
            {
                number = value;
                Debug.Log(number);
            }
        }
    }

    public Action<bool> onNumberCheck;

    private void Awake()
    {
        Transform child = transform.GetChild(1);
        buttons = new KeypadButton[10];
        TextMeshProUGUI text = null;

        buttons[0] = child.GetChild(10).GetComponent<KeypadButton>();
        buttons[0].Initialize(0);

        for (int i = 1; i < buttons.Length; i++)
        {
            buttons[i] = child.GetChild(i - 1).GetComponent<KeypadButton>();
            buttons[i].Initialize(i);
        }

        okButton = child.GetChild(9).GetComponent<Button>();
        text = okButton.GetComponentInChildren<TextMeshProUGUI>();
        text.text = "*";

        cancelButton = child.GetChild(11).GetComponent<Button>();
        text = cancelButton.GetComponentInChildren<TextMeshProUGUI>();
        text.text = "#";

        child = transform.GetChild(2);
        Texts = child.GetComponentsInChildren<TextMeshProUGUI>();

        canvas = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        OnShowKeypad();

        foreach (KeypadButton button in buttons)
        {
            button.onInputNumber += OnInputNumber;
        }

        okButton.onClick.AddListener(NumberCheck);
        cancelButton.onClick.AddListener(() =>
        {
            for(int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = 0;
            }
            Refresh();
        });
    }

    private void OnInputNumber(int index)
    {
        numbers[3] = numbers[2];
        numbers[2] = numbers[1];
        numbers[1] = numbers[0];
        numbers[0] = index;
        Refresh();
    }

    private void Refresh()
    {
        Number = 0;

        for (int i = 0; i < numbers.Length; i++)
        {
            Texts[i].text = numbers[i].ToString();

            Number += numbers[i] * (int)Mathf.Pow(10, i);
        }
    }

    private void NumberCheck()
    {
        bool result = false;

        if(Number == 5942)
        {
            result = true;
            OnShowKeypad();
        }

        Debug.Log(result);
        onNumberCheck?.Invoke(result);
    }

    public void OnShowKeypad()
    {
        if(canvas.alpha <0.1f)
        {
            canvas.alpha = 1.0f;
            canvas.interactable = true;
            canvas.blocksRaycasts = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            canvas.alpha = 0f;
            canvas.interactable = false;
            canvas.blocksRaycasts = false; 
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
