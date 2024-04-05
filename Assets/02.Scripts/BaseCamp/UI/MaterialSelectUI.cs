using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialSelectUI : MonoBehaviour
{
    [SerializeField] Button[] selectButtons;
    [SerializeField] Transform[] wallButtonsTr; 

    private void Awake()
    {   //BlockPanel떄문에 -1 함
        selectButtons = new Button[transform.childCount-1];
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Debug.Log("tt");
            selectButtons[i] = transform.GetChild(i).GetComponent<Button>();    
        }

        

    }
}
