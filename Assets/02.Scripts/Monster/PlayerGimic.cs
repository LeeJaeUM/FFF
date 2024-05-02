using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerGimic : MonoBehaviour
{
    [SerializeField] float rayDistance = 10f; // 레이의 최대 거리

    private void Update()
    {
        // 충돌 정보를 저장할 변수
        RaycastHit hitInfo;

        // 레이를 발사하고 충돌 검출
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, rayDistance))
        {
            // 충돌한 물체에 대한 처리
            //Debug.Log("Hit object: " + hitInfo.collider.gameObject.name);

            // 레이의 출발점에서 충돌 지점까지 라인 그리기
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);

            IInteractable interactable = hitInfo.collider.gameObject.GetComponent<IInteractable>();
            if(interactable != null )
            {
                interactable.Interact();
            }
            else
            {
                //Debug.Log(" 상호작용 가능한 오브젝트가 아니다! ");
            }
        }
        else
        {
            // 레이가 아무것도 충돌하지 않은 경우
            // 레이의 최대 거리만큼 라인 그리기
            Debug.DrawLine(transform.position, transform.position + transform.forward * rayDistance, Color.yellow);
        }
    }
}
