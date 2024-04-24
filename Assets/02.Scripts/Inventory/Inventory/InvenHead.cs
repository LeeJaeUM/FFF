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

    private Transform parent;

    private void Start()
    {
        parent = transform.parent;

        Button button = GetComponentInChildren<Button>();
        button.onClick.AddListener(() =>
        {
            CanvasGroup canvas = parent.GetComponent<CanvasGroup>();
            canvas.blocksRaycasts = false;
            canvas.interactable = false;
            canvas.alpha = 0;
        });
    }

    public void OnDrag(PointerEventData eventData)
    {
        parent.position = parent.position + (Vector3)eventData.delta;
    }
}
