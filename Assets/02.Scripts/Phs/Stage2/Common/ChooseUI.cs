using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ChooseUI;

public class ChooseUI : MonoBehaviour
{
    public enum ChooseType
    {
        None,
        FakeKey,
        Book,
        BookButton,
    }

    ChooseType type = ChooseType.None;

    public ChooseType Type
    {
        get => type;
        set
        {
            if(type != value)
            {
                type = value;
                switch (type)
                {
                    case ChooseType.None:
                        Close();
                        break;
                    case ChooseType.FakeKey:
                        message.text = "왠지 이상한 기본이 든다. 열쇠를 가져가겠습니까?";
                        break;
                    case ChooseType.Book:
                        message.text = "책을 끼울 수 있을 것 같다. 책을 끼우겠습니까?";
                        break;
                    case ChooseType.BookButton:
                        message.text = "버튼을 누르겠습니까?";
                        break;
                }
            }
        }
    }

    public PickUpItem _fakeKey;

    public Transform bookTransform;

    public Action onWarning;

    public Action onBookButtonActive;

    CanvasGroup canvas;

    Stage1Manager manager;

    TextMeshProUGUI message;

    private void Awake()
    {
        message = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        canvas = GetComponent<CanvasGroup>();
        manager = Stage1Manager.Instance;
        Close();
    }

    private void Start()
    {
        Button yes = transform.GetChild(2).GetComponent<Button>();
        Button no = transform.GetChild(3).GetComponent<Button>();

        yes.onClick.AddListener(onYes);
        no.onClick.AddListener(onNo);
    }

    private void onYes()
    {
        switch (Type)
        {
            case ChooseType.FakeKey:
                _fakeKey.GetItem();
                Close();
                manager.BottomTMPText = "경보";
                Type = ChooseType.None;
                onWarning?.Invoke();
                break;
            case ChooseType.Book:
                Close();
                Batch();
                onBookButtonActive?.Invoke();
                Type = ChooseType.None;
                break;
            case ChooseType.BookButton:
                Type = ChooseType.None;
                BookButtonInteracable book = FindAnyObjectByType<BookButtonInteracable>();
                if (!book.isCanUse)
                {
                    manager.BottomTMPText = "경보";
                    onWarning?.Invoke();
                }
                else
                {
                    BookShelf_Unlock unlock = FindAnyObjectByType<BookShelf_Unlock>();
                    if (unlock != null)
                    {
                        unlock.Open();
                    }
                }
                break;
        }
    }

    private void onNo()
    {
        switch (Type)
        {
            case ChooseType.FakeKey:
                Close();
                manager.BottomTMPText = "왠지 잘한 것 같다.";
                Type = ChooseType.None;
                break;
            case ChooseType.Book:
                Close();
                Type = ChooseType.None;
                break;
            case ChooseType.BookButton:
                Close();
                Type = ChooseType.None;
                break;
        }
    }

    public void Open(ChooseType type)
    {
        Type = type;
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

    public void Batch()
    {
        Transform child = bookTransform.GetChild(0);
        child = child.GetChild(0);
        child.gameObject.SetActive(true);

        child = bookTransform.GetChild(1);
        child = child.GetChild(0);
        child.gameObject.SetActive(true);
    }
}

