using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingedDoor : MonoBehaviour, IInteractable
{
    private Transform door; // 문 오브젝트
    private float openAngle = 90f; // 문이 열릴 때의 각도
    private float closedAngle = 0f; // 문이 닫힐 때의 각도
    private float swingSpeed = 4.0f; // 문이 회전하는 속도
    private bool isOpen1 = false; // 문이 열려 있는지 여부

    void Start()
    {
        // 문을 닫힌 상태로 초기화
        door.localRotation = Quaternion.Euler(0, closedAngle, 0);
    }

    void Update()
    {
        
    }
    public virtual void Interact()
    {
        Debug.Log("1");
        ToggleDoor();
    }

    public void ToggleDoor()
    {
        if (isOpen1)
        {
            Debug.Log("open");
            StartCoroutine(SwingDoor(closedAngle));
        }
        else
        {
            Debug.Log("close");
            StartCoroutine(SwingDoor(openAngle));
        }
        isOpen1 = !isOpen1;
    }

    IEnumerator SwingDoor(float targetAngle)
    {
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        while (Quaternion.Angle(door.localRotation, targetRotation) > 0.01f)
        {
            door.localRotation = Quaternion.Slerp(door.localRotation, targetRotation, swingSpeed * Time.deltaTime);
            yield return null;
        }
        door.localRotation = targetRotation; // 정확한 각도로 설정
    }
}