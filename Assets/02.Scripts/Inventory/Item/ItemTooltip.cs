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
    /// �Ͻ� ���� ��带 Ȯ���ϰ� �����ϴ� ������Ƽ
    /// </summary>
    public bool IsPause
    {
        get => isPause;
        set
        {
            isPause = value;
            if (isPause)
            {
                Close();    // �Ͻ� ������ �Ǹ� �����ִ� �� ����â�� �ݴ´�.
            }
        }
    }

    public void Open(ItemData data)
    {
        if(!IsPause && data != null)
        {
            // ������Ʈ ä���
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
