using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : RecycleObject
{
    private ItemData data = null;

    private ItemData Data
    {
        get => data;
        set
        {
            if(data != value)
            {
                data = value;
                itemCode = data.itemCode;
                model = Instantiate(data.itemPrefab, transform);
            }
        }
    }

    private GameObject model;
    private ItemCode itemCode;
    private int itemCount;

    [SerializeField]
    InventoryUI inven;

    private void Start()
    {
        inven = GameManager.Instance.inven;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPickUp();
        }
    }

    public void SetData(ItemCode _code, int _count)
    {
        ResetData();
        Data = inven.FindCodeData(_code);
        itemCount = _count;
    }

    public void ResetData()
    {
        Data = null;
        itemCount = 0;
        if(model != null)
        {
            Destroy(model);
        }
    }

    private void OnPickUp()
    {
        inven.GetItemToSlot(itemCode, itemCount); 
        LifeOver();
    }
}
