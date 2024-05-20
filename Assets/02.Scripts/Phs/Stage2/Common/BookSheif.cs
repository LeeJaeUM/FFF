using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookSheif : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;

    CanvasGroup canvas;

    Button complete;

    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        Transform child = transform.GetChild(0);
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

    }

    private void Complete()
    {

    }

    private void Stop()
    {
        Close();
    }
}
