using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    public TextMeshProUGUI nameText, DescriptionText, priceText, weightText;
    public Image Icon;

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
        }
    }

    public void Open(ItemData data)
    {
        if(!IsPause && data != null)
        {
            // 컴포넌트 채우기
            Icon.sprite = data.itemIcon;
            nameText.text = data.name;
            DescriptionText.text = data.itemDescription;
            priceText.text = data.itemPrice.ToString();
            weightText.text = data.itemWeight.ToString();
        }
    }

    public void Close()
    {

    }
}
