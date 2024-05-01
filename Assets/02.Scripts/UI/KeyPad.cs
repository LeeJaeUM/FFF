using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyPad : MonoBehaviour
{
    //입력 버튼들
    //비밀번호는 5812
    Button[] pads = null;

    //입력된 넘버들 
    TextMeshProUGUI[] selectedNums = null;

    int numCount = 0;

    //정답을 확인 할 int 배열
    [SerializeField] List<int> clickNums = new List<int>();

    [Header("정답숫자")]
    [Tooltip("#은 10, *는 12")]
    [SerializeField] List<int> rightAnswer = new List<int> { 5, 8, 1, 2 };

    Image lensDotImg;

    // 정답 입력시 호출할 액션
    public Action onAnswerCheck;

    private void Awake()
    {
        pads = GetComponentsInChildren<Button>();

        Transform child = transform.GetChild(1);
        child = child.transform.GetChild(0);

        //확인용 이미지 찾은 후 Red로 변경
        lensDotImg = child.GetComponent<Image>(); 
        lensDotImg.color = Color.red;

        child = transform.GetChild(2);
        selectedNums = child.GetComponentsInChildren<TextMeshProUGUI>();

        int[] numbers = new int[pads.Length]; // 각 버튼에 대한 숫자 배열 선언

        // 숫자 배열 초기화
        for (int i = 0; i < pads.Length; i++)
        {
            numbers[i] = i + 1;
        }

        // 각 버튼에 클릭 리스너 등록
        for (int i = 0; i < pads.Length; i++)
        {
            int numberToSend = numbers[i]; // 해당 버튼에 대한 숫자 가져오기

            pads[i].onClick.AddListener(() => ClickNumPad(numberToSend));
        }


        //시작 시 입력된 숫자 초기화
        RefreshSelectedNums();
    }

    /// <summary>
    /// 버튼을 누르면 입력되는 함수
    /// </summary>
    /// <param name="clickNum">입력될 숫자</param>
    void ClickNumPad(int clickNum)
    {
        //selectNum가 10 = *, 11 = 0, 12 = #, 13이 확인버튼
        Debug.Log(clickNum);

        if(numCount < 4)
        {
            //입력된 숫자 글자에 넣기
            switch (clickNum)
            {
                case 10:
                    selectedNums[numCount].text = "*";
                    clickNums.Add(clickNum);
                    numCount++;
                    break;
                case 11:
                    clickNum = 0;
                    selectedNums[numCount].text = clickNum.ToString();
                    clickNums.Add(clickNum);
                    numCount++;
                    break;
                case 12:
                    selectedNums[numCount].text = "#";
                    clickNums.Add(clickNum);
                    numCount++;
                    break;
                case 13:
                    break;
                default:
                    selectedNums[numCount].text = clickNum.ToString();
                    clickNums.Add(clickNum);
                    numCount++;
                    break;
            }
        }
        else
        {
            if(clickNum == 13)
            {
                //정답이면 true 틀리면 false
                if (CheckAnswer())
                {
                    Debug.Log("정답!! 이 시점에서 액션을 쓰면 확인가능");
                    lensDotImg.color = Color.green;
                    onAnswerCheck?.Invoke();    
                }
                else
                {
                    //4자리를 넘으면 초기화
                    numCount = 0;
                    clickNums.Clear();
                    RefreshSelectedNums();
                }
            }
            else
            {
                Debug.Log("확인 버튼을 눌러야한다");
            }
        }

    }

    /// <summary>
    /// 입력된 숫자들 초기화 함수
    /// </summary>
    void RefreshSelectedNums()
    {
        foreach (var tmp in selectedNums)
        {
            tmp.text = "-";
        }
    }

    /// <summary>
    /// 정답을 체크하는 함수
    /// </summary>
    /// <returns>정답이면 true 리턴 = 5812</returns>
    private bool CheckAnswer()
    {
        bool result = false;    // 그 외의 경우는 정답이 아님

        // 현재 리스트에 들어있는 숫자의 개수가 4개가 아니면 정답이 될 수 없음
        if (clickNums.Count != 4)
        {
            return result;
        }

        // 리스트에 들어있는 숫자가 차례로 5, 8, 1, 2라면 정답으로 처리
        if (clickNums[0] == rightAnswer[0] && clickNums[1] == rightAnswer[1] && 
            clickNums[2] == rightAnswer[2] && clickNums[3] == rightAnswer[3])
        {
            result = true;
        }

        return result;
    }
}
