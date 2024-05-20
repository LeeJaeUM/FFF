using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BookSheif : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;

    CanvasGroup canvas;

    Button complete;

    bool isBook6 = false;
    bool isBook29 = false;

    bool IsBook
    {
        get => isBook6 && isBook6;
        set
        {
            complete.interactable = true;
        }
    }

    GameObject bookGrab = null;

    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        Transform child = transform.GetChild(1);

        child = child.GetChild(0);
        Button book_6 = child.GetComponent<Button>();
        book_6.interactable = true;
        book_6.onClick.AddListener(() =>
        {
            Arrangement(ItemCode.Book_6);
            book_6.interactable = false;
        });

        child = transform.GetChild(1);
        Button book_29 = child.GetComponent<Button>();
        book_29.interactable = true;
        book_29.onClick.AddListener(() =>
        {
            Arrangement(ItemCode.Book_29);
            book_29.interactable = false;
        });

        child = transform.GetChild(2);
        complete = child.GetComponent<Button>();
        complete.interactable = false;
        complete.onClick.AddListener(Complete);

        child = transform.GetChild(3);
        Button stop = child.GetComponent<Button>();
        stop.onClick.AddListener(Stop);
    }

    private void Update()
    {
        if (bookGrab != null)
        {
        }
    }

    public void Open()
    {
        vcam.Priority = 100;

        canvas.alpha = 1.0f;
        canvas.blocksRaycasts = true;
        canvas.interactable = true;
    }

    private void Close()
    {
        vcam.Priority = 1;

        canvas.interactable = false;
        canvas.blocksRaycasts = false;
        canvas.alpha = 0.0f;
    }

    private void Arrangement(ItemCode code)
    {
        if(code == ItemCode.Book_6)
        {
            isBook6 = true;
        }
        else
        {
            isBook29 = true;
        }

        ItemData data = GameManager.Instance.inven.FindCodeData(code);
        bookGrab = Instantiate(data.itemPrefab);
    }

    private void Complete()
    {

    }

    private void Stop()
    {
        Close();
    }
}
