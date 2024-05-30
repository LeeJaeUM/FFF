using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestInputField : MonoBehaviour
{
    TextMeshProUGUI sd;
    public TMP_InputField inputField;

    public string ans = "1235";

    private void Awake()
    {
        // Input Field에 이벤트 리스너 추가
         inputField.onEndEdit.AddListener(OnInputEndEdit);
    }


    // 입력이 완료되었을 때 호출되는 메서드
    public void OnInputEndEdit(string input)
    {
        Debug.Log("입력값: " + input);
        if(input == ans)
        {
            Debug.Log("정답");
        }
        else
        {
            Debug.Log("틀렸어");
        }
        // 여기서 입력값에 대한 추가적인 처리를 수행할 수 있습니다.
    }

}