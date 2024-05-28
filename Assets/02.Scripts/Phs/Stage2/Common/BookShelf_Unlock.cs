using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BookShelf_Unlock : MonoBehaviour
{
    /// <summary>
    /// 자식 유닛
    /// </summary>
    BookUnit[] bookUnits;

    [SerializeField]
    List<int> selectIdList;

    TextMeshProUGUI infoText;

    public Action onSecretRoom;

    public Action onWarning;

    // 컴포넌트
    CanvasGroup canvas;

    Button complete;

    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
        selectIdList = new List<int>();
    }

    private void Start()
    {
        Transform child = transform.GetChild(0);

        bookUnits = new BookUnit[child.childCount];
        for(int i = 0; i < bookUnits.Length; i++)
        {
            bookUnits[i] = child.GetChild(i).GetComponent<BookUnit>();
            bookUnits[i].Initialize(i);
            bookUnits[i].onSelect += OnSelect;
            bookUnits[i].onCancel += OnCancel;
        }

        child = transform.GetChild(2);
        infoText = child.GetComponentInChildren<TextMeshProUGUI>();
        infoText.text = "";

        child = transform.GetChild(3);
        complete = child.GetComponent<Button>();
        complete.onClick.AddListener(OnComplete);

        child = transform.GetChild(4);
        Button cancel = child.GetComponent<Button>();
        cancel.onClick.AddListener(() =>
        {
            selectIdList.Clear();
            OnRefresh();
            Close();
        });

        GameManager.Instance.inven.onHintClose += () =>
        {
            selectIdList.Clear();
            OnRefresh();
            Close();
        };

        Close();
    }

    private void OnComplete()
    {
        // (2, 7, 10, 29)
        if(selectIdList.Count == 4)
        {
            Debug.LogWarning(selectIdList.Count);

            for (int i = 0; i < selectIdList.Count; i++)
            {
                int id = selectIdList[i];

                if (id != 2 && id != 7 && id != 10 && id != 29)
                {
                    Debug.LogWarning($"{id}_경고");
                    Stage1Manager.Instance.BottomTMPText = "경고";
                    onWarning?.Invoke();
                    Close();
                    return;
                }
            }
            onSecretRoom?.Invoke();
            Close();
            return;
        }

        Stage1Manager.Instance.BottomTMPText = "경고";
        Close();
    }

    private void OnRefresh()
    {
        infoText.text = string.Empty;

        for(int i = 0; i < selectIdList.Count; i++)
        {
            int x = selectIdList[i] % 10;
            int y = selectIdList[i] / 10;

            infoText.text += $"{y + 1} - {x + 1}";

            if (selectIdList.Count - 1 != i)
            {
                infoText.text += ", ";
            }
        }
    }

    private void OnSelect(int index)
    {
        selectIdList.Add(index);
        selectIdList.Sort();
        OnRefresh();
    }

    private void OnCancel(int index)
    {
        selectIdList.Remove(index);
        selectIdList.Sort();
        OnRefresh();
    }

    #region UI On/Off
    public void Open()
    {
        canvas.alpha = 1.0f;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
    }

    public void Close()
    {
        canvas.blocksRaycasts = false;
        canvas.interactable = false;
        canvas.alpha = 0;
    }
    #endregion
}
