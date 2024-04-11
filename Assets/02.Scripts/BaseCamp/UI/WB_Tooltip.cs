using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WB_Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GameObject tooltipObj;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        tooltipObj = child.gameObject;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipObj.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipObj.SetActive(false);
    }
}
