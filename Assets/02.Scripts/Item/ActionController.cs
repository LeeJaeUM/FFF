using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    /// <summary>
    /// 아이템 습득이 가능한 최대 거리
    /// </summary>
    private float range;

    /// <summary>
    /// 아이템 습득 가능할 시 True
    /// </summary>
    private bool pickupActivated = false;

    /// <summary>
    /// 충돌체 정보 저장
    /// </summary>
    private RaycastHit hitInfo;

    [SerializeField]
    /// <summary>
    /// 특정 레이어를 가진 오브젝트에 대해서만 습득할 수 있어야 한다.
    /// </summary>
    private LayerMask layerMask;

    [SerializeField]
    /// <summary>
    /// 행동을 보여 줄 텍스트
    /// </summary>
    private TextMeshProUGUI actionText;

    void Update()
    {
        CheckItem();
        TryAction();
    }

    /// <summary>
    /// E키 입력이 들어왔는지를 검사한다.(E키가 들어온다면)
    /// </summary>
    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))    // E키가 입력되면
        {
            CheckItem();    // 아이템을 주울 수 있는 상태인지 검사
            CanPickUp();    // 실제 줍는 처리 실행
        }
    }

    /// <summary>
    /// 항상 아이템이 사정거리 안에 있는지를 체크한다.(E키를 누르는게 아니더라도)
    /// </summary>
    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))  // 사정거리 내에 layerMask 레이어를 가진 오브젝트가 있다면
        {
            if (hitInfo.transform.CompareTag("Item"))   // 그 오브젝트의 태그가 Item이면
            {
                ItemInfoAppear();   // 아이템을 주우라는 텍스트를 띄워줌
            }
        }
        else
        {
            // 사정거리 내에 layerMask 레이어를 가진 오브젝트가 없다.
            ItemInfoDisappear();    // 아이템을 주우라는 텍스트 비활성화
        }
    }

    /// <summary>
    /// 아이템을 주울 수 있는 상태, 아이템을 주우라는 텍스트를 띄워줌
    /// </summary>
    private void ItemInfoAppear()
    {
        pickupActivated = true;     // 아이템을 주울 수 있는 상태이다.
        actionText.gameObject.SetActive(true);  // 텍스트 활성화
        if(hitInfo.transform != null && hitInfo.transform.GetComponent<ItemPickUp>() != null && hitInfo.transform.GetComponent<ItemPickUp>().itemData != null)
        {
            actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().itemData.itemName + " Pick up " + "<color=yellow>" + "[E]" + "</color>";
        }
    }

    /// <summary>
    /// 아이템을 주울 수 없는 상태, 아이템을 주우라는 텍스트를 비활성화 한다.
    /// </summary>
    private void ItemInfoDisappear()
    {
        pickupActivated = false;    // 아이템을 주울 수 없는 상태이다.
        actionText.gameObject.SetActive(false); // 텍스트 비활성화
    }

    /// <summary>
    /// 실제 아이템을 줍는 처리를 한다
    /// </summary>
    private void CanPickUp()
    {
        if (pickupActivated)    // pickupActivated가 true일 때(아이템을 주울 수 있는 상태일 때)
        {
            if (hitInfo.transform != null)  // 충돌 오브젝트가 존재할 때만 처리(혹시 모를 NullReferenceException 방지)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().itemData.itemName + " 획득 했습니다.");  
                Destroy(hitInfo.transform.gameObject);  // 월드에 배치된 해당 아이템 파괴.(주워서 인벤토리에 넣었기 때문에)
                ItemInfoDisappear();                    // 아이템을 주울 수 없는 상태로 만들고, 줍는 도움말 텍스트 비활성화
            }
        }
    }
}
