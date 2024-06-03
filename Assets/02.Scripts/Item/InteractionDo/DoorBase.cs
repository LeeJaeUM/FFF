using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorBase : MonoBehaviour, IInteractable
{
    HingedDoor hingedDoor;
    enum DoorType
    {
        None = 0,
        Masterkey,
        Firstkey,
        Metal,
        Clear,
        OilSyringe,
        SecondKey,
        ThirdKey
    }

    [SerializeField] private DoorType doorType = DoorType.None;
    AudioManager.Sfx sfx_Open;
    AudioManager.Sfx sfx_Close;
    AudioManager.Sfx sfx_CannotOpen = AudioManager.Sfx.HammerHittingMetalDoor;

    private string announsText = string.Empty;

    [SerializeField] private bool isFirst = true;
    [SerializeField] private bool isOpen = false;

    public int itemcode = 0;
    private InventoryUI inventoryUI;

    readonly int Interact_Hash = Animator.StringToHash("Interact");
    private Animator animator;

    public Transform door; // 문 오브젝트
    private float openAngle = 90f; // 문이 열릴 때의 각도
    private float closedAngle = 0f; // 문이 닫힐 때의 각도
    private float swingSpeed = 4.0f; // 문이 회전하는 속도
    private bool isOpen1 = false; // 문이 열려 있는지 여부

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
        animator = GetComponent<Animator>();
        if (itemcode == 34)
        {
            door.localRotation = Quaternion.Euler(0, closedAngle, 0);
        } 
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

    protected virtual void Start()
    {

        //start에서 위의 doorType에 따라서 itemCode Itemcode enum타입의 맞게 코드 설정해주기
        switch (doorType)
        {
            case DoorType.None:
                Debug.Log("아무것도 선택되지 않은 문");
                itemcode = 99;
                sfx_Open = AudioManager.Sfx.DoorOpening;
                sfx_Close = AudioManager.Sfx.DoorClosing;
                break;
            case DoorType.Masterkey:
                itemcode = (int)ItemCode.MasterKey;
                announsText = ("특수한 열쇠가 필요하다.");
                sfx_Open = AudioManager.Sfx.MetalDoorOpening;
                sfx_Close = AudioManager.Sfx.MetalDoorOpening;
                break;
            case DoorType.Firstkey:
                itemcode = (int)ItemCode.FirstDoorKey;
                announsText = ("열쇠가 필요하다.");
                sfx_Open = AudioManager.Sfx.DoorOpening;
                sfx_Close = AudioManager.Sfx.DoorClosing;
                break;
            case DoorType.Metal:
                itemcode = (int)ItemCode.OilBucket;
                announsText = ("폭발물이 필요하다.");
                sfx_Open = AudioManager.Sfx.MetalDoorOpening;
                sfx_Close = AudioManager.Sfx.MetalDoorOpening;
                break;
            case DoorType.Clear:
                itemcode = -1;
                announsText = ("옆의 패드의 비밀번호를 입력해야한다.");
                sfx_Open = AudioManager.Sfx.MetalDoorOpening;
                sfx_Close = AudioManager.Sfx.MetalDoorOpening;
                break;
            case DoorType.OilSyringe:
                itemcode = (int)ItemCode.OilSyringe;
                announsText = ("문고리가 녹슬어서 문을 열 수 없다. 기름같은 것이 필요할 거 같다.");
                sfx_Open = AudioManager.Sfx.MetalDoorOpening;
                sfx_Close = AudioManager.Sfx.MetalDoorOpening;
                break;
            case DoorType.SecondKey:
                itemcode = (int)ItemCode.SecondKey;
                announsText = ("열쇠가 필요하다.");
                sfx_Open = AudioManager.Sfx.DoorOpening;
                sfx_Close = AudioManager.Sfx.DoorClosing;
                break;
            case DoorType.ThirdKey:
                itemcode = (int)ItemCode.ThirdKey;
                announsText = ("열쇠가 필요하다.");
                sfx_Open = AudioManager.Sfx.DoorOpening;
                sfx_Close = AudioManager.Sfx.DoorClosing;
                break;
            default: break;
        }
    }

    public virtual void Interact()
    {
        //기본 문 조건
        if (itemcode == 99)
        {
            DoorOpen();
        }
        // interact했을때 인벤토리에 코드에 맞는 아이템이 있는지 확인해서
        else if(inventoryUI.UseItemCheck((ItemCode)itemcode))
        {
            //조건에 충족되면 아이템 추가 또는 여타 상호작용
            DoorOpen();
        }
        else    // false면
        {
            // 불충분시 안내 텍스트
            Stage1Manager.Instance.BottomTMPText = announsText;
            AudioManager.instance.PlaySfx(sfx_CannotOpen);
        }


        Debug.Log("문과 interact함");
    }

    protected virtual void DoorOpen()
    {
        if(!isOpen)
        {
            if(isFirst)
            {
                //문 성공적으로 열림
                Stage1Manager.Instance.BottomTMPText = ("문이 열렸다");
                isFirst = false;
            }
            ToggleDoor();
            AudioManager.instance.PlaySfx(sfx_Open);
            animator.SetTrigger(Interact_Hash);
            
        }
        else
        {
            ToggleDoor();
            AudioManager.instance.PlaySfx(sfx_Close);
            animator.SetTrigger(Interact_Hash);
        }

    }
}
