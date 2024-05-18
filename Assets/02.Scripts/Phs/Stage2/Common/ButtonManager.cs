using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public Stage2_Button[] buttons;
    public GameObject[] Battery;

    public Material successMaterial;
    public Material failMaterial;

    private void Start()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].ButtonID = i;
            buttons[i].onTrigger += OnTrigger;
        }
    }

    private void OnTrigger(int id)
    {
        Battery[id].GetComponent<MeshRenderer>().material = successMaterial; 
    }
}
