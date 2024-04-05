using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialSelectUI : MonoBehaviour
{
    [SerializeField] Button[] selectButtons;

    public Action<int> onSelecMAterial;

    private void Awake()
    {   //BlockPanel떄문에 -1 함
        selectButtons = new Button[transform.childCount-1];
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            selectButtons[i] = transform.GetChild(i).GetComponent<Button>();     
            
            // 각 버튼의 클릭 이벤트에 대한 리스너를 추가
            int buttonIndex = i;
            selectButtons[i].onClick.AddListener(() => OnButtonClicked(buttonIndex));
        }
    }

    // 버튼이 클릭되었을 때 호출되는 함수
    void OnButtonClicked(int index)
    {
        // 액션 실행
        if (onSelecMAterial != null)
        {
            onSelecMAterial(index);
        }
    }
}
