using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BookUnit : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // 컴포넌트
    TextMeshProUGUI titleText;
    Image panal;

    /// <summary>
    /// 유닛 아이디
    /// </summary>
    public int id;

    /// <summary>
    /// 기본색깔
    /// </summary>
    public Color baseColor;

    /// <summary>
    /// 마우스 들어갈때 색깔
    /// </summary>
    public Color enterColor;

    /// <summary>
    /// 선택될 때 색깔
    /// </summary>
    public Color selectColor;

    /// <summary>
    /// true면 선택됨, false면 선택이 안됨
    /// </summary>
    bool isSelect = false;

    /// <summary>
    /// 선택 델리게이트
    /// </summary>
    public Action<int> onSelect;

    /// <summary>
    /// 취소 델리게이트
    /// </summary>
    public Action<int> onCancel;

    private void Awake()
    {
        titleText = GetComponentInChildren<TextMeshProUGUI>();
        panal = GetComponent<Image>();
    }

    public void Initialize(int _id)
    {
        id = _id;

        int x = id % 10;
        int y = id / 10;

        //Debug.Log($"{x}_{y}");

        titleText.text = $"{y + 1}\n-\n{x + 1}";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left && !isSelect)
        {
            panal.color = selectColor;
            isSelect = true;
            onSelect?.Invoke(id);
        }
        else if(eventData.button == PointerEventData.InputButton.Right && isSelect)
        {
            panal.color = enterColor;
            isSelect = false;
            onCancel?.Invoke(id);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!isSelect)
        {
            panal.color = enterColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelect)
        {
            panal.color = baseColor;
        }
    }
}
