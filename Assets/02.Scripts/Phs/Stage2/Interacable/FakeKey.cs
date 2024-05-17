using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeKey : MonoBehaviour
{
    public PickUpItem _fakeKey;

    CanvasGroup canvas;

    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
        Close();
    }

    private void Start()
    {
        Button yes = transform.GetChild(2).GetComponent<Button>();
        Button no = transform.GetChild(3).GetComponent<Button>();

        yes.onClick.AddListener(() =>
        {
            _fakeKey.GetItem();
            Close();
            Stage1Manager.Instance.BottomTMPText = "경보";
        });
        no.onClick.AddListener(Close);
    }

    public void Open()
    {
        canvas.alpha = 1.0f;
        canvas.blocksRaycasts = true;
        canvas.interactable = true;
    }

    public void Close()
    {
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
        canvas.alpha = 0.0f;
    }
}
