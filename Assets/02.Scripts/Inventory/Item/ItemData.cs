using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������� �����͸� ��Ƶδ� ��ũ���ͺ� ������Ʈ
[CreateAssetMenu(fileName = "ItemData", menuName = "Data/ItemData", order = 0)]
public class ItemData : ScriptableObject
{
    /// <summary>
    /// �������� ������ȣ
    /// </summary>
    public int itemID;

    /// <summary>
    /// �������� ������
    /// </summary>
    public Sprite itemIcon;

    /// <summary>
    /// �������� �̸�
    /// </summary>
    public string itemName;

    [TextArea]
    /// <summary>
    /// �������� ����
    /// </summary>
    public string itemDescription;

    /// <summary>
    /// �������� ����ũ��
    /// </summary>
    [Range(1, 3)]
    public int SizeX = 1;

    /// <summary>
    /// �������� ����ũ��
    /// </summary>
    [Range(1, 5)]
    public int SizeY = 1;

    public Vector2Int Size
    {
        get => new Vector2Int(SizeX, SizeY);
        set
        {
            if(Size != value)
            {
                Size = value;
                Debug.Log(Size);
            }
        }
    }

    /// <summary>
    /// �������� �ִ� ����
    /// </summary>
    public int maxItemCount;

    /// <summary>
    /// ������ ����
    /// </summary>
    public float itemPrice;

    /// <summary>
    /// �������� ����
    /// </summary>
    public float itemWeight;
}
