using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    private TextMeshProUGUI nameText, DescriptionText, priceText, weightText;
    private Image Icon;

    CanvasGroup canvasGroup;

    /// <summary>
    /// 일시 징지 모드(true면 일시 정지, false면 사용 중)
    /// </summary>
    private bool isPause;

    /// <summary>
    /// 일시 정지 모드를 확인하고 설정하는 프로퍼티
    /// </summary>
    public bool IsPause
    {
        get => isPause;
        set
        {
            isPause = value;
            if (isPause)
            {
                Close();    // 일시 정지가 되면 열려있던 상세 정보창도 닫는다.
            }
            //Debug.Log(isPause);
        }
    }

    /// <summary>
    /// 알파값이 변하는 속도
    /// </summary>
    public float alphaChangeSpeed = 10.0f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;

        Transform child = transform.GetChild(1);
        Icon = child.GetComponent<Image>();
        child = transform.GetChild(2);
        nameText = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(3);
        priceText = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(4);
        weightText = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(5);
        DescriptionText = child.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        MovePosition(Mouse.current.position.ReadValue());
    }

    public void Open(ItemData data)
    {
        if(!IsPause && data != null)
        {
            // 컴포넌트 채우기
            Icon.sprite = data.itemIcon;
            nameText.text = data.name;
            DescriptionText.text =$" {data.itemDescription}";
            priceText.text = $"가격 : {data.itemPrice}";
            weightText.text = $"무게 : {data.itemWeight}";

            canvasGroup.alpha = 0.0001f;

            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }
    }

    public void Close()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    /// <summary>
    /// 상세 정보창을 움직이는 함수
    /// </summary>
    /// <param name="screenPos">스크린 좌표</param>
    public void MovePosition(Vector2 screenPos)
    {
        if(canvasGroup.alpha > 0.0f)
        {
            RectTransform rect = (RectTransform)transform;
            int over = (int)(screenPos.x + rect.sizeDelta.x) - Screen.width;    // 얼마나 넘쳤는지 확인
            screenPos.x -= Mathf.Max(0, over);  // over를 양수로만 사용(음수일때는 별도 처리 필요없음
            rect.position = screenPos;
        }
    }


    /// <summary>
    /// 알파를 0 -> 1로 만드는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeIn()
    {
        while(canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        canvasGroup.alpha = 1.0f;
    }

    IEnumerator FadeOut()
    {
        while (canvasGroup.alpha > 0.0f)
        {
            canvasGroup.alpha -= Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        canvasGroup.alpha = 0.0f;
    }
}
