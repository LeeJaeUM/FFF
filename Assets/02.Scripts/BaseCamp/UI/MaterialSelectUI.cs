using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialSelectUI : MonoBehaviour
{
    [SerializeField] Button[] selectButtons;

    public Action<int> onSelectMaterial;

    private void Awake()
    {   
        Transform child = transform.GetChild(0);
        Transform g_child = child.transform.GetChild(0);
        selectButtons = g_child.GetComponentsInChildren<Button>(true);

        for (int i = 0; i < 4; i++)
        {
            //    selectButtons[i] = transform.GetChild(i).GetComponent<Button>();     

            // 각 버튼의 클릭 이벤트에 대한 리스너를 추가
            int buttonIndex = i;
            selectButtons[i].onClick.AddListener(() => OnButtonClicked(buttonIndex));
        }
    }

    // 버튼이 클릭되었을 때 호출되는 함수
    public void OnButtonClicked(int index)
    {
        // 액션 실행
        if (onSelectMaterial != null)
        {
            onSelectMaterial(index);
        }
    }
}
