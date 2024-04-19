using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvenHead : MonoBehaviour, IDragHandler
{
    public Action<Vector2> Drag;
    public Action DragEnd;

    private void Start()
    {
        Button button = GetComponentInChildren<Button>();
        button.onClick.AddListener(() =>
        {
            GameManager.Instance.inven.Close();
        });
    }

    public void OnDrag(PointerEventData eventData)
    {
        Drag?.Invoke(eventData.delta);
    }
}
