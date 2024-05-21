using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintUI : MonoBehaviour
{
    GameObject[] UIObject;

    private void Awake()
    {
        UIObject = new GameObject[transform.childCount];

        for(int i = 0;  i < UIObject.Length; i++)
        {
            UIObject[i] = transform.GetChild(i).gameObject;
        }
    }

    private void Start()
    {
        GameManager.Instance.inven.onInteracable += OnReadHint;
        GameManager.Instance.inven.onHintClose += OnHintClose;
    }

    private void OnReadHint(ItemContain contain)
    {
        ItemCode code = contain.item.itemCode;

        switch(code)
        {
            case ItemCode.GoldHint_1:
                UIObject[1].SetActive(true);
                break;
            case ItemCode.GoldHint_2:
                UIObject[2].SetActive(true);
                break;
            case ItemCode.Room3Hint:
                UIObject[3].SetActive(true);
                break;
            case ItemCode.Room4Caution:
                UIObject[4].SetActive(true);
                break;
            default:
                break;
        }
    }

    private void OnHintClose()
    {
        foreach(GameObject obj in UIObject)
        {
            obj.SetActive(false);
        }
    }
}
