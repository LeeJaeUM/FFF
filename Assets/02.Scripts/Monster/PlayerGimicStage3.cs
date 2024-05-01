using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine.AI;

public class PlayerGimicStage3 : MonoBehaviour
{
    RaycastHit hit;

    [SerializeField]
    private Transform raycastOrigin;
    [SerializeField]
    private Color rayColor = Color.red;

    [SerializeField]
    private GameObject interactionUI; // 상호작용이 가능한지 나타내는 UI 오브젝트
    [SerializeField]
    private TextMeshProUGUI textField; // 게임의 진행을 위한 Text
    [SerializeField]
    private TextMeshProUGUI choiceText1Field; // 선택지 1
    [SerializeField]
    private TextMeshProUGUI choiceText2Field; // 선택지 2
    [SerializeField]
    private TextMeshProUGUI choiceText3Field; // 선택지 3
    [SerializeField]
    private TextMeshProUGUI getItemTextField; // 획득 아이템 텍스트

    // 발전기 조작 변수
    [SerializeField]
    private GameObject generatorButtonUI; // 발전기 조작 UI
    private bool switch1 = true;
    private bool switch2 = true;
    private bool switch3 = true;
    private bool switch4 = true;
    private bool switch5 = true;
    private bool switch6 = true;
    private bool switch7 = true;
    private bool switch8 = true;
    private bool switch9 = true;
    private bool switch10 = true;
    [SerializeField]
    private Animator switch1Animator;
    private Animator switch2Animator;
    private Animator switch3Animator;
    private Animator switch4Animator;
    private Animator switch5Animator;
    private Animator switch6Animator;
    private Animator switch7Animator;
    private Animator switch8Animator;
    private Animator switch9Animator;
    private Animator switch10Animator;

    [SerializeField]
    private GameObject player; // 플레이어 캐릭터
    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private Animator restroomDoorAnimator; // 문이 열리는 애니메이션
    [SerializeField]
    private Animator storageDoorAnimator; // 문이 열리는 애니메이션
    [SerializeField]
    private Animator bookShelfAnimator; // 책장이 움직이는 애니메이션
    [SerializeField]
    private Animator hiddenRoomAnimator; // 비밀문이 열리는 애니메이션
    [SerializeField]
    private Animator clearDoorAnimator; // 클리어 문이 열리는 애니메이션
    [SerializeField]
    private Animator startLeftDoor; // 시작 시 처음 열리는 문 애니메이션
    [SerializeField]
    private Animator startRightDoor; // 시작 시 처음 열리는 문 애니메이션
    [SerializeField]
    private Animator restaurantDoorAnimator; // 식당 문 애니메이션
    [SerializeField]
    private Animator generatorDoorAnimator; // 발전기 문 애니메이션

    [SerializeField]
    private GameObject trap; // Trap 오브젝트
    [SerializeField]
    private GameObject trapUI; // TrapUI 오브젝트
    [SerializeField]
    private GameObject pictureFrame; // PictureFrame 오브젝트
    
    [SerializeField]
    private GameObject tvScreen;
    [SerializeField]
    private Material newScreenMaterial; // TV 스크린 오브젝트의 머티리얼

    [SerializeField]
    private GameObject[] diaryUI; // Diary UI 오브젝트
    [SerializeField]
    private GameObject[] researchLog; // Page UI 오브젝트
    [SerializeField]
    private GameObject remoteControl; // 리모컨 오브젝트
    [SerializeField]
    private GameObject blade; // Blade 오브젝트
    [SerializeField]
    private GameObject metal; // 쇠로 된 식기 오브젝트
    [SerializeField]
    private GameObject pilers; // 연장 오브젝트
    [SerializeField]
    private GameObject restaurantRoomDoorKey; // RestaurantRoomDoorKey 오브젝트
    [SerializeField]
    private GameObject restroomCaution; // RestaurantCaution의 자식으로 있는 Text(TMP)
    private int currentPageIndex = 0; // 페이지 수

    private float maxDistance = 2f; // Raycast 최대 범위

    private bool isInteraction = false; // 플레이어의 상호작용 여부
    private bool textDisplayed = false; // 상호작용 중의 텍스트 중복 방지 여부
    private bool textDisplayed2 = false; // 
    private bool interating = false; // 플레이어가 현재 상호작용 중인지 아닌지 여부
    private bool choicing = false; // 선택지가 떴을 때 여부
    private bool restroomOpen = true; // RestRoomDoor의 열림 여부
    private bool isDiary3Open = false; // 다이어리의 3페이지의 활성 여부
    private bool hiddenRoomDoor = true; // 비밀문의 열림 상태
    private bool restaurantDoor = true; // 식당문의 열림 상태
    private bool storageDoorOpen = false; // 창고문의 열림 상태

    private bool isEvent = false; // 이벤트 발생 동안

    private bool startDoorOpen = false; // 시작 문이 열림의 유무
    private bool haveRemoteControl = false; // 인벤토리에 RemoteControl이 있을 경우
    private bool isInteractionTouchPad = false; // TouchPad와 상호작용을 했을 경우
    private bool haveAxe = false; // 인벤토리에 Axe오브젝트가 있을 경우
    private bool haveRestaurantDoorKey = false; // 인벤토리에 RestaurantDoorKey가 있을 경우
    private bool haveDoctorHand = false; // 인벤토리에 DoctorHand가 있을 경우
    private bool haveBlade = false; // 인벤토리에 Blade가 있을 경우
    private bool haveMetal = false; // 인벤토리에 Metal이 있을 경우
    private bool havePilers = false; // 인벤토리에 Pilers가 있을 경우
    private bool isMoveBookShelf = false; // BookShelf가 이동했을 경우
    private bool isOperable = false; // 발전기 조작 가능 여부

    private bool unLockStorageDoor = false; // StorageDoor의 해금 상태

    [SerializeField]
    private AudioSource padBeep; // 패드가 틀렸을 때의 경고음
    [SerializeField]
    private AudioSource padUnlockSound; // 패드가 성공했을 때의 언락 사운드
    [SerializeField]
    private AudioSource bookShelfSound; // 책장 틈에서 나오는 바람 소리 
    [SerializeField]
    private AudioSource bikeEngineSound; // 바이크 엔진 소리

    private void Start()
    {
        
    }

    private void Update()
    {
        Renderer renderer= tvScreen.GetComponent<Renderer>();

        if(!startDoorOpen)
            StartCoroutine(OpenStartDoor());

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {
            Debug.Log(hit.collider.gameObject.name);
            
            // 레이가 Player에 충돌할 경우
            if(hit.collider.CompareTag("Player"))
            {
                return;
            }

            // Restroom------------------------------------------------------------------------------------
            // PictureFrame 상호작용
            if (hit.collider.CompareTag("PICTUREFRAME"))
            {
                // 플레이어가 다른 오브젝트 또는 해당 오브젝트와 상호작용 중이지 않을 때 PictureFrame과 상호작용 가능 여부
                if(!isInteraction)
                {
                    interactionUI.SetActive(true);
                }

                // 상호작용 시작
                if(Input.GetKeyDown(KeyCode.E) && !textDisplayed)
                {
                    isInteraction = true;
                    interating = true;
                    interactionUI.SetActive(false);
                    textField.text = "꽤나 큰 액자다...";
                    textDisplayed = true;
                }

                // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
                else if(Input.GetKeyDown(KeyCode.E) && textDisplayed)
                {
                    textField.text = "들춰낼까?";
                    choiceText1Field.text = "1. 들춰낸다";
                    choiceText2Field.text = "2. 내버려둔다";
                    choicing = true;
                }

                // 선택지가 출력되었을 때 가능한 버튼
                if(choicing)
                {
                    // 1번을 누를 때
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        textField.text = " "; // 상호작용 종료
                        choiceText1Field.text = " ";
                        choiceText2Field.text = " ";
                        trap.SetActive(true); // 함정 생성
                        pictureFrame.SetActive(false); // 함정 생성
                    }

                    // 2번을 누를 때
                    else if(Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        textField.text = " "; // 상호작용 종료
                        choiceText1Field.text = " ";
                        choiceText2Field.text = " ";
                        player.SetActive(true); // 상호작용 종료
                        isInteraction = false;
                        textDisplayed = false;
                        interating = false;
                        choicing = false;
                    }
                }
            }

            // Diary 상호작용
            if(hit.collider.CompareTag("DIARY"))
            {
                // 플레이어가 다른 오브젝트 또는 해당 오브젝트와 상호작용을 하지 않을 때
                if(!isInteraction)
                {
                    interactionUI.SetActive(true);
                }

                // 상호작용 시작
                if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                {
                    isInteraction = true;
                    interating = true;
                    interactionUI.SetActive(false);
                    textField.text = "이 곳 담당의 일기인거 같다.";
                    textDisplayed = true;
                }

                // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
                else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !isEvent)
                {
                    textField.text = "볼까?";
                    choiceText1Field.text = "1. 본다";
                    choiceText2Field.text = "2. 내버려둔다";
                    choicing = true;
                }

                // 선택지가 출력되었을 때 가능한 버튼
                if (choicing)
                {
                    // 1번을 누를 때
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        textField.text = " ";
                        choiceText1Field.text = " ";
                        choiceText2Field.text = " ";
                        choicing = false;
                        textDisplayed = false;
                        isEvent = true;
                        diaryUI[currentPageIndex].SetActive(true); // 일기장 UI 출력
                    }

                    // 2번을 누를 때
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        textField.text = " ";
                        choiceText1Field.text = " ";
                        choiceText2Field.text = " ";
                        player.SetActive(true); // 상호작용 종료
                        isInteraction = false;
                        textDisplayed = false;
                        interating = false;
                        choicing = false;
                    }
                }

                // 이벤트 시작
                if(isEvent && !choicing && !textDisplayed && isInteraction && interating)
                {
                    // E키를 누를 때마다 페이지가 넘어감
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        diaryUI[currentPageIndex].SetActive(false);

                        currentPageIndex = (currentPageIndex + 1) % diaryUI.Length;

                        diaryUI[currentPageIndex].SetActive(true);

                        // 마지막 페이지에서 E키를 누르면 상호작용 종료
                        if (currentPageIndex == 4)
                        {
                            diaryUI[currentPageIndex].SetActive(false);

                            currentPageIndex = 0;

                            isDiary3Open = true;
                            isEvent = false;
                            isInteraction = false;
                            interating = false;
                            player.SetActive(true);
                        }
                    }
                }
            }

            // RetroTelevision 상호작용
            if(hit.collider.CompareTag("RETROTELEVISION"))
            {
                // 플레이어가 다른 오브젝트 또는 해당 오브젝트와 상호작용을 하지 않을 때
                if (!isInteraction)
                {
                    interactionUI.SetActive(true);
                }         

                // 일반상호작용(인벤토리에 RemoteControl이 없을 경우)
                if(!haveRemoteControl)
                {
                    // 일반상호작용 시작
                    if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                    {
                        isInteraction = true;
                        interating = true;
                        interactionUI.SetActive(false);
                        textField.text = "단순 조작으로는 켜지지 않는다...";
                        textDisplayed = true;
                    }

                    // 일반상호작용 종료
                    else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !isEvent)
                    {
                        textField.text = " ";
                        player.SetActive(true);
                        isInteraction = false;
                        textDisplayed = false;
                        interating = false;
                    }
                }

                // 특수상호작용(인벤토리에 RemoteControl이 있을 경우)
                if(haveRemoteControl)
                {
                    // 특수상호작용 시작
                    if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                    {
                        isInteraction = true;
                        interating = true;
                        interactionUI.SetActive(false);
                        textField.text = "TV를 켤 수 있을거 같다...";
                        textDisplayed = true;
                    }

                    // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
                    else if (Input.GetKeyDown(KeyCode.E) && textDisplayed)
                    {
                        textField.text = "TV를 켜볼까?";
                        choiceText1Field.text = "1. 켠다";
                        choiceText2Field.text = "2. 내버려둔다";
                        choicing = true;
                    }

                    if(choicing)
                    {
                        // 1번을 누를 때
                        if (Input.GetKeyDown(KeyCode.Alpha1))
                        {
                            textField.text = " ";
                            choiceText1Field.text = " ";
                            choiceText2Field.text = " ";
                            choicing = false;
                            textDisplayed = false;
                            renderer.material = newScreenMaterial; // TV에 몬스터의 얼굴이 뜸.
                        }

                        // 2번을 누를 때
                        else if (Input.GetKeyDown(KeyCode.Alpha2))
                        {
                            textField.text = " "; // 상호작용 종료
                            choiceText1Field.text = " ";
                            choiceText2Field.text = " ";
                            player.SetActive(true); // 상호작용 종료
                            isInteraction = false;
                            textDisplayed = false;
                            interating = false;
                            choicing = false;
                        }
                    }
                }
            }

            // Lamp상호작용
            if(hit.collider.CompareTag("LAMP"))
            {
                if (!isInteraction)
                {
                    interactionUI.SetActive(true);
                }

                // 일반상호작용 시작
                if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                {
                    isInteraction = true;
                    interating = true;
                    interactionUI.SetActive(false);
                    textField.text = "불이 켜지지 않는다...";
                    textDisplayed = true;
                }

                // 일반상호작용 종료
                else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !isEvent)
                {
                    textField.text = " ";
                    player.SetActive(true);
                    isInteraction = false;
                    textDisplayed = false;
                    interating = false;
                }
            }

            // RestroomDoor상호작용
            if(hit.collider.CompareTag("RESTROOMDOOR"))
            {
                if (!isInteraction)
                {
                    interactionUI.SetActive(true);
                }

                // 상호작용 시작, 문이 열리고 닫힘.
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if(restroomOpen)
                    {
                        restroomDoorAnimator.SetBool("IsOpen", true);
                    }
                    else
                    {
                        restroomDoorAnimator.SetBool("IsOpen", false);
                    }

                    restroomOpen = !restroomOpen;
                }
            }

            // ClipBoard상호작용
            if(hit.collider.CompareTag("CLIPBOARD"))
            {
                if (!isInteraction)
                {
                    interactionUI.SetActive(true);
                }

                // 상호작용 시작
                if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                {
                    isInteraction = true;
                    interating = true;
                    interactionUI.SetActive(false);
                    textField.text = "실험체들의 연구 보고서인거 같다.";
                    textDisplayed = true;
                }

                // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
                else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !isEvent)
                {
                    textField.text = "볼까?";
                    choiceText1Field.text = "1. 본다";
                    choiceText2Field.text = "2. 내버려둔다";
                    choicing = true;
                }

                // 선택지가 출력되었을 때 가능한 버튼
                if (choicing)
                {
                    // 1번을 누를 때
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        textField.text = " ";
                        choiceText1Field.text = " ";
                        choiceText2Field.text = " ";
                        choicing = false;
                        textDisplayed = false;
                        isEvent = true;
                        researchLog[currentPageIndex].SetActive(true); // 연구보고서 UI 출력
                    }

                    // 2번을 누를 때
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        textField.text = " ";
                        choiceText1Field.text = " ";
                        choiceText2Field.text = " ";
                        player.SetActive(true); // 상호작용 종료
                        isInteraction = false;
                        textDisplayed = false;
                        interating = false;
                        choicing = false;
                    }
                }

                // 이벤트 시작
                if (isEvent && !choicing && !textDisplayed && isInteraction && interating)
                {
                    // E키를 누를 때마다 페이지가 넘어감
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        researchLog[currentPageIndex].SetActive(false);

                        currentPageIndex = (currentPageIndex + 1) % researchLog.Length;

                        researchLog[currentPageIndex].SetActive(true);

                        // 마지막 페이지에서 E키를 누르면 상호작용 종료
                        if (currentPageIndex == 4)
                        {
                            researchLog[currentPageIndex].SetActive(false);

                            currentPageIndex = 0;

                            isEvent = false;
                            isInteraction = false;
                            interating = false;
                            player.SetActive(true);
                        }
                    }
                }
            }

            // RemoteControl 상호작용
            if(hit.collider.CompareTag("REMOTECONTROL"))
            {
                if(!isInteraction)
                {
                    interactionUI.SetActive(true);
                }

                // 상호작용 시작
                if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                {
                    isInteraction = true;
                    interating = true;
                    interactionUI.SetActive(false);
                    textField.text = "TV를 켤 수 있는 리모컨이다.";
                    textDisplayed = true;
                }

                // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
                else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !isEvent)
                {
                    textField.text = "가지고 있을까?";
                    choiceText1Field.text = "1. 줍는다";
                    choiceText2Field.text = "2. 내버려둔다";
                    choicing = true;
                }

                if(choicing)
                {
                    // 1번을 누를 때
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        textField.text = " ";
                        choiceText1Field.text = " ";
                        choiceText2Field.text = " ";
                        getItemTextField.text = "리모컨 획득";
                        Destroy(remoteControl); // 리모컨 비활성화
                        haveRemoteControl = true; // RetroTelevision 조건 활성화

                        StartCoroutine(GetItem());
                    }

                    // 2번을 누를 때
                    else if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        textField.text = " "; // 상호작용 종료
                        choiceText1Field.text = " ";
                        choiceText2Field.text = " ";
                        player.SetActive(true); // 상호작용 종료
                        isInteraction = false;
                        textDisplayed = false;
                        interating = false;
                        choicing = false;
                    }
                }
            }

            // DeadDoctor 상호작용
            if(hit.collider.CompareTag("DEADDOCTOR"))
            {
                if (!isInteraction)
                {
                    interactionUI.SetActive(true);
                }

                // 일반상호작용
                if(!isInteractionTouchPad)
                {
                    // 일반상호작용 시작
                    if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                    {
                        isInteraction = true;
                        interating = true;
                        interactionUI.SetActive(false);
                        textField.text = "이 곳에서 실험을 했던 과학자인거 같다...";
                        textDisplayed = true;
                    }

                    // 일반상호작용 종료
                    else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !isEvent)
                    {
                        textField.text = " ";
                        player.SetActive(true);
                        isInteraction = false;
                        textDisplayed = false;
                        interating = false;
                    }
                }

                // 특수상호작용
                if(isInteractionTouchPad)
                {
                    // 특수상호작용 1-1
                    if(!haveAxe)
                    {
                        // 특수상호작용 1-1 시작
                        if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                        {
                            isInteraction = true;
                            interating = true;
                            interactionUI.SetActive(false);
                            textField.text = "손을 자를 수 있는 것이 필요하다..";
                            textDisplayed = true;
                        }

                        // 특수상호작용 1-1 종료
                        else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !isEvent)
                        {
                            textField.text = " ";
                            player.SetActive(true);
                            isInteraction = false;
                            textDisplayed = false;
                            interating = false;
                        }
                    }

                    // 특수상호작용 1-2
                    if(haveAxe)
                    {
                        // 특수상호작용 1-2 시작
                        if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                        {
                            isInteraction = true;
                            interating = true;
                            interactionUI.SetActive(false);
                            textField.text = "이 시체의 손을 가지고 가보자.";
                            textDisplayed = true;
                        }

                        // 특수상호작용 1-2 선택지
                        else if (Input.GetKeyDown(KeyCode.E) && textDisplayed)
                        {
                            textField.text = "시체의 손을 자를까?";
                            choiceText1Field.text = "1. 자른다";
                            choiceText2Field.text = "2. 내버려둔다";
                            choicing = true;
                        }

                        if(choicing)
                        {
                            // 1번을 누를 때
                            if (Input.GetKeyDown(KeyCode.Alpha1))
                            {
                                textField.text = " "; // 상호작용 종료
                                choiceText1Field.text = " ";
                                choiceText2Field.text = " ";
                                getItemTextField.text = "절단된 의사의 손 획득";
                                haveDoctorHand = true;

                                StartCoroutine(GetItem());
                            }
                        }
                    }
                }
            }

            // RestaurantRoomDoorKey 상호작용
            if (hit.collider.CompareTag("RESTAURANTDOORKEY"))
            {
                if(!isInteraction)
                {
                    interactionUI.SetActive(true);
                }

                // 상호작용 시작
                if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                {
                    isInteraction = true;
                    interating = true;
                    textField.text = "다른 방을 열 수 있는 열쇠인거 같다..";
                    interactionUI.SetActive(false);
                    haveRestaurantDoorKey = true;
                    textDisplayed = true;
                }

                // 상호작용 종료
                else if (Input.GetKeyDown(KeyCode.E) && textDisplayed)
                {
                    textField.text = " "; // 상호작용 종료
                    choiceText1Field.text = " ";
                    choiceText2Field.text = " ";
                    getItemTextField.text = "식당 열쇠 획득";
                    Destroy(restaurantRoomDoorKey);
                    restroomCaution.SetActive(true);

                    StartCoroutine(GetItem());
                }
            }

            // Bag 상호작용
            if(hit.collider.CompareTag("BAG"))
            {
                if(!isInteraction)
                {
                    interactionUI.SetActive(true);
                }

                // 상호작용 시작
                if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                {
                    isInteraction = true;
                    interating = true;
                    textField.text = "언제든 떠날 준비였던거 같다.";
                    interactionUI.SetActive(false);
                    textDisplayed = true;
                }

                // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
                else if (Input.GetKeyDown(KeyCode.E) && textDisplayed)
                {
                    textField.text = "내용물을 확인해볼까?";
                    choiceText1Field.text = "1. 앞주머니";
                    choiceText2Field.text = "2. 중간";
                    choiceText3Field.text = "3. 메인주머니";
                    choicing = true;
                }

                if (choicing)
                {
                    // 인벤토리에 Axe오브젝트가 없을 경우
                    if (!haveAxe)
                    {
                        if (Input.GetKeyDown(KeyCode.Alpha1))
                        {
                            textField.text = " ";
                            choiceText1Field.text = " ";
                            choiceText2Field.text = " ";
                            choiceText3Field.text = " ";
                            getItemTextField.text = "도끼 획득";
                            haveAxe = true;

                            StartCoroutine(GetItem()); // 상호작용 종료
                        }
                    }

                    // 인벤토리에 Axe오브젝트가 있을 경우
                    else if(haveAxe)
                    {
                        if(Input.GetKeyDown(KeyCode.Alpha1))
                        {
                            textField.text = " ";
                            choiceText1Field.text = " ";
                            choiceText2Field.text = " ";
                            choiceText3Field.text = " ";
                            player.SetActive(true); // 상호작용 강제 종료
                            isInteraction = false;
                            textDisplayed = false;
                            interating = false;
                            choicing = false;
                        }
                    }

                    if(Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        textField.text = "아무것도 없다";
                        choiceText1Field.text = " ";
                        choiceText2Field.text = " ";
                        choiceText3Field.text = " ";

                        StartCoroutine(GetItem()); // 상호작용 종료
                    }

                    if(Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        textField.text = " ";
                        choiceText1Field.text = " ";
                        choiceText2Field.text = " ";
                        choiceText3Field.text = " ";
                        trapUI.SetActive(true); // 게임 오버
                    }
                }
            }

            // TouchPad 상호작용
            if(hit.collider.CompareTag("TOUCHPAD"))
            {
                if(!isInteraction)
                {
                    interactionUI.SetActive(true);
                }

                //일반상호작용(인벤토리에 절단된 의사의 손이 없을 경우)
                if(!haveDoctorHand)
                {
                    // 상호작용 시작
                    if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                    {
                        isInteraction = true;
                        interating = true;
                        textField.text = "손을 갖다 대는 걸로 작동하는 패드인거 같다..";
                        interactionUI.SetActive(false);
                        textDisplayed = true;
                        isInteractionTouchPad = true;
                    }

                    // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
                    else if (Input.GetKeyDown(KeyCode.E) && textDisplayed)
                    {
                        padBeep.Play();
                        textField.text = "적당한 것을 찾아보자";

                        StartCoroutine(GetItem());
                    }
                }

                //특수상호작용(인벤토리에 절단된 의사의 손이 있을경우)
                if(haveDoctorHand)
                {
                    // 상호작용 시작
                    if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                    {
                        isInteraction = true;
                        interating = true;
                        textField.text = "(절단된 의사의 손을 갖다 댄다..)";
                        interactionUI.SetActive(false);
                        textDisplayed = true;
                        storageDoorOpen = true;
                    }

                    // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
                    else if (Input.GetKeyDown(KeyCode.E) && textDisplayed)
                    {
                        padUnlockSound.Play();
                        textField.text = " ";
                        unLockStorageDoor = true; // 창고문이 열렸다는 효과음
                        storageDoorAnimator.SetBool("IsUnlock", true); // 창고문이 열림

                        player.SetActive(true); // 상호작용 강제 종료
                        isInteraction = false;
                        textDisplayed = false;
                        interating = false;
                        choicing = false;
                    }
                }
            }

            // BookShelf 상호작용
            if(hit.collider.CompareTag("BOOKSHELF"))
            {
                if (!isInteraction)
                {
                    interactionUI.SetActive(true);
                }

                // 일반상호작용
                if(!isDiary3Open)
                {
                    // 상호작용 시작
                    if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                    {
                        isInteraction = true;
                        interating = true;
                        textField.text = "커다란 책장이다..";
                        interactionUI.SetActive(false);
                        textDisplayed = true;
                        isInteractionTouchPad = true;
                    }

                    // 일반상호작용 종료
                    else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !isEvent)
                    {
                        textField.text = " ";
                        player.SetActive(true);
                        isInteraction = false;
                        textDisplayed = false;
                        interating = false;
                    }
                }

                // 특수상호작용
                if (isDiary3Open)
                {
                    // 상호작용 시작
                    if (Input.GetKeyDown(KeyCode.E) && !textDisplayed)
                    {
                        isInteraction = true;
                        interating = true;
                        interactionUI.SetActive(false);
                        bookShelfSound.Play();
                        textField.text = "뒤에 바람이 들어온다...";
                        textDisplayed = true;
                    }

                    // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
                    else if (Input.GetKeyDown(KeyCode.E) && textDisplayed)
                    {
                        textField.text = "뒤에 무언가가 있다...";
                        choiceText1Field.text = "1. 끌어당긴다";
                        choiceText2Field.text = "2. 내버려둔다";
                        choicing = true;
                    }

                    if(choicing)
                    {
                        // 1번을 누를 때
                        if (Input.GetKeyDown(KeyCode.Alpha1))
                        {
                            textField.text = " ";
                            choiceText1Field.text = " ";
                            choiceText2Field.text = " ";
                            bookShelfAnimator.SetBool("IsTransform", true); // 책장이 옮겨짐
                            isMoveBookShelf = true;

                            StartCoroutine(GetItem()); // 상호작용 종료
                        }

                        // 2번을 누를 때
                        else if (Input.GetKeyDown(KeyCode.Alpha2))
                        {
                            textField.text = " "; // 상호작용 종료
                            choiceText1Field.text = " ";
                            choiceText2Field.text = " ";
                            player.SetActive(true); // 상호작용 종료
                            isInteraction = false;
                            textDisplayed = false;
                            interating = false;
                            choicing = false;
                        }
                    }
                }
            }

            // HiddenRoomDoor 상호작용
            if(hit.collider.CompareTag("HIDDENROOMDOOR"))
            {
                if (!isInteraction)
                {
                    interactionUI.SetActive(true);
                }

                // BookShelf가 이동하지 않은 상태에서 HiddenRoomDoor와 상호작용이 되는 것을 방지
                if(isMoveBookShelf)
                {
                    // 상호작용 시작, 문이 열리고 닫힘.
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (hiddenRoomDoor)
                        {
                            hiddenRoomAnimator.SetBool("IsOpen", true);
                        }
                        else
                        {
                            hiddenRoomAnimator.SetBool("IsOpen", false);
                        }

                        hiddenRoomDoor = !hiddenRoomDoor;
                    }
                }
            }

            // ClearDoor 상호작용
            if(hit.collider.CompareTag("CLEARDOOR"))
            {
                if (!isInteraction)
                {
                    interactionUI.SetActive(true);
                }

                // 상호작용 시작
                if (Input.GetKeyDown(KeyCode.E) && !textDisplayed)
                {
                    isInteraction = true;
                    interating = true;
                    interactionUI.SetActive(false);
                    textField.text = "이 문이 일기에 적혀있던 탈출구 인거 같다";
                    textDisplayed = true;
                }

                // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
                else if (Input.GetKeyDown(KeyCode.E) && textDisplayed)
                {
                    textField.text = "열까?";
                    choiceText1Field.text = "1. 연다";
                    choiceText2Field.text = "2. 내버려둔다";
                    choicing = true;
                }

                // 선택지가 출력되었을 때 가능한 버튼
                if (choicing)
                {
                    // 1번을 누를 때(게임 오버)
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        textField.text = " "; // 상호작용 종료
                        choiceText1Field.text = " ";
                        choiceText2Field.text = " ";

                        clearDoorAnimator.SetBool("IsOpen", true);
                    }

                    // 2번을 누를 때
                    else if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        textField.text = " "; // 상호작용 종료
                        choiceText1Field.text = " ";
                        choiceText2Field.text = " ";
                        player.SetActive(true); // 상호작용 종료
                        isInteraction = false;
                        textDisplayed = false;
                        interating = false;
                        choicing = false;
                    }
                }
            }
            // --------------------------------------------------------------------------------

            // Lobby---------------------------------------------------------------------------------
            // MecanicEye 상호작용
            if(hit.collider.CompareTag("MECANICEYE"))
            {
                if (!isInteraction)
                {
                    interactionUI.SetActive(true);
                }

                // 일반상호작용 시작
                if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !textDisplayed2 && !isEvent)
                {
                    isInteraction = true;
                    interating = true;
                    interactionUI.SetActive(false);
                    textField.text = "홍채인식으로 작동하는거 같다.";
                    textDisplayed = true;
                }

                // 일반상호작용 중
                else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !textDisplayed2&& !isEvent)
                {
                    textField.text = "지금은 작동하지 않는 거 같다.";
                    textDisplayed2 = true;
                }

                // 일반상호작용 종료
                else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && textDisplayed2 && !isEvent)
                {
                    textField.text = " ";
                    player.SetActive(true);
                    isInteraction = false;
                    textDisplayed = false;
                    textDisplayed2 = false;
                    interating = false;
                }
            }

            // LobbyKeyPad 상호작용
            if(hit.collider.CompareTag("LOBBYKEYPAD"))
            {
                if (!isInteraction)
                {
                    interactionUI.SetActive(true);
                }

                // 일반상호작용 시작
                if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !textDisplayed2 && !isEvent)
                {
                    isInteraction = true;
                    interating = true;
                    interactionUI.SetActive(false);
                    textField.text = "비밀번호를 입력해야 하는거 같다.";
                    textDisplayed = true;
                }

                // 일반상호작용 중
                else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !textDisplayed2 && !isEvent)
                {
                    textField.text = "지금은 작동하지 않는 거 같다..";
                    textDisplayed2 = true;
                }

                // 일반상호작용 종료
                else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && textDisplayed2 && !isEvent)
                {
                    textField.text = " ";
                    player.SetActive(true);
                    isInteraction = false;
                    textDisplayed = false;
                    textDisplayed2 = false;
                    interating = false;
                }
            }
            //---------------------------------------------------------------------------------

            // Restaurant ---------------------------------------------------------------------------------
            // RestaurantDoor 상호작용
            if(hit.collider.CompareTag("RESTAURANTDOOR"))
            {
                if (!isInteraction)
                {
                    interactionUI.SetActive(true);
                }

                // 일반상호작용(식당 문 열쇠가 없을 경우)
                if(!haveRestaurantDoorKey)
                {
                    // 상호작용 시작
                    if (Input.GetKeyDown(KeyCode.E) && !textDisplayed)
                    {
                        isInteraction = true;
                        interating = true;
                        interactionUI.SetActive(false);
                        textField.text = "문이 열리지 않는다.";
                        textDisplayed = true;
                    }

                    // 상호작용 종료
                    else if (Input.GetKeyDown(KeyCode.E) && textDisplayed)
                    {
                        textField.text = " ";
                        player.SetActive(true); // 상호작용 강제 종료
                        isInteraction = false;
                        textDisplayed = false;
                        interating = false;
                        choicing = false;
                    }
                }

                // 특수상호작용(식당 문 열쇠가 있을 경우)
                else if(haveRestaurantDoorKey)
                {
                    // 상호작용 시작, 문이 열리고 닫힘.
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (restaurantDoor)
                        {
                            restaurantDoorAnimator.SetBool("IsOpen", true);
                        }
                        else
                        {
                            restaurantDoorAnimator.SetBool("IsOpen", false);
                        }

                        restaurantDoor = !restaurantDoor;
                    }
                }
            }

            //Metal 상호작용
            if(hit.collider.CompareTag("METAL"))
            {
                if(!isInteraction)
                {
                    interactionUI.SetActive(true);
                }

                // 상호작용 시작
                if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                {
                    isInteraction = true;
                    interating = true;
                    interactionUI.SetActive(false);
                    textField.text = "쇠로 된 식기다.";
                    textDisplayed = true;
                }

                // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
                else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !isEvent)
                {
                    textField.text = "쓸 데가 있을까?";
                    choiceText1Field.text = "1. 줍는다";
                    choiceText2Field.text = "2. 내버려둔다";
                    choicing = true;
                }

                if (choicing)
                {
                    // 1번을 누를 때
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        textField.text = " ";
                        choiceText1Field.text = " ";
                        choiceText2Field.text = " ";
                        getItemTextField.text = "쇠로 된 식기 획득";
                        Destroy(metal); // 식기 비활성화
                        haveMetal = true;

                        StartCoroutine(GetItem());
                    }

                    // 2번을 누를 때
                    else if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        textField.text = " "; // 상호작용 종료
                        choiceText1Field.text = " ";
                        choiceText2Field.text = " ";
                        player.SetActive(true); // 상호작용 종료
                        isInteraction = false;
                        textDisplayed = false;
                        interating = false;
                        choicing = false;
                    }
                }
            }
            //-----------------------------------------------------------------------------

            //Storage-----------------------------------------------------------------------------------------------
            // Generator 상호작용
            if(hit.collider.CompareTag("GENERATOR"))
            {
                if(!isInteraction)
                {
                    interactionUI.SetActive(true);
                }

                // 일반상호작용(인벤토리에 Pilers가 없을 경우)
                if(!havePilers)
                {
                    // 상호작용 시작
                    if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                    {
                        isInteraction = true;
                        interating = true;
                        interactionUI.SetActive(false);
                        textField.text = "문을 열 만한 도구가 필요하다.";
                        textDisplayed = true;
                    }

                    // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
                    else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !isEvent)
                    {
                        textField.text = " ";
                        player.SetActive(true); // 상호작용 강제 종료
                        isInteraction = false;
                        textDisplayed = false;
                        interating = false;
                        choicing = false;
                    }
                }

                // 특수상호작용1(인벤토리에 Pilers가 있을 경우
                if(havePilers && !isOperable)
                {
                    // 상호작용 시작
                    if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                    {
                        isInteraction = true;
                        interating = true;
                        interactionUI.SetActive(false);
                        textField.text = "연장으로 문을 열 수 있을 거 같다.";
                        textDisplayed = true;
                    }

                    // 상호작용 종료(발전기 문이 열림)
                    else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !isEvent)
                    {
                        textField.text = " ";
                        generatorDoorAnimator.SetBool("IsOpen", true);
                        isOperable = true;
                        player.SetActive(true); // 상호작용 강제 종료
                        isInteraction = false;
                        textDisplayed = false;
                        interating = false;
                        choicing = false;

                        isOperable = true;
                    }
                }

                // 특수상호작용1-1(발전기 문이 열렸을 경우)
                if(isOperable)
                {
                    // 상호작용 시작
                    if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                    {
                        isInteraction = true;
                        interating = true;
                        interactionUI.SetActive(false);
                        textField.text = "발전기를 조작할 수 있을 거 같다.";
                        textDisplayed = true;
                    }

                    // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
                    else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !isEvent)
                    {
                        textField.text = "조작하시겠습니까?";
                        choiceText1Field.text = "1. 조작한다";
                        choiceText2Field.text = "2. 내버려둔다";
                        choicing = true;
                    }

                    if (choicing)
                    {
                        // 1번을 누를 때
                        if (Input.GetKeyDown(KeyCode.Alpha1))
                        {
                            textField.text = " ";
                            choiceText1Field.text = " ";
                            choiceText2Field.text = " ";
                            isEvent = true;
                            textDisplayed = false;
                            choicing = false;
                        }

                        // 2번을 누를 때
                        else if (Input.GetKeyDown(KeyCode.Alpha2))
                        {
                            textField.text = " "; // 상호작용 종료
                            choiceText1Field.text = " ";
                            choiceText2Field.text = " ";
                            player.SetActive(true); // 상호작용 종료
                            isInteraction = false;
                            textDisplayed = false;
                            interating = false;
                            choicing = false;
                        }
                    }

                    // 이벤트 시작
                    if (isEvent && !choicing && !textDisplayed && isInteraction && interating)
                    {
                        generatorButtonUI.SetActive(true);

                        // 1번 스위치 조작
                        if (Input.GetKeyDown(KeyCode.Keypad1))
                        {
                            if (switch1)
                            {
                                switch1Animator.SetBool("IsMove", true);
                            }
                            else
                            {
                                switch1Animator.SetBool("IsMove", false);
                            }

                            switch1 = !switch1;
                        }

                        // 2번 스위치 조작
                        if (Input.GetKeyDown(KeyCode.Keypad2))
                        {
                            if (switch2)
                            {
                                switch2Animator.SetBool("IsMove", true);
                            }
                            else
                            {
                                switch2Animator.SetBool("IsMove", false);
                            }

                            switch2 = !switch2;
                        }
                    }
                }
            }

            // Bike 상호작용
            if(hit.collider.CompareTag("BIKE"))
            {
                if(!isInteraction)
                {
                    interactionUI.SetActive(true);
                }

                // 상호작용 시작
                if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                {
                    isInteraction = true;
                    interating = true;
                    interactionUI.SetActive(false);
                    bikeEngineSound.Play();
                    textField.text = "잘 작동한다..";
                    textDisplayed = true;
                }

                // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
                else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !isEvent)
                {
                    textField.text = " ";
                    player.SetActive(true); // 상호작용 강제 종료
                    isInteraction = false;
                    textDisplayed = false;
                    interating = false;
                    choicing = false;
                }
            }

            // StorageDoor 상호작용
            if(hit.collider.CompareTag("STORAGEDOOR"))
            {
                if(!storageDoorOpen)
                {
                    if (!isInteraction)
                    {
                        interactionUI.SetActive(true);
                    }

                    // 일반상호작용 시작
                    if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !textDisplayed2 && !isEvent)
                    {
                        isInteraction = true;
                        interating = true;
                        interactionUI.SetActive(false);
                        textField.text = "일반적으로 열리는 문이 아니다..";
                        textDisplayed = true;
                    }

                    // 일반상호작용 중
                    else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !textDisplayed2 && !isEvent)
                    {
                        textField.text = "어딘가 열 수 있는 장치가 있을것이다.";
                        textDisplayed2 = true;
                    }

                    // 일반상호작용 종료
                    else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && textDisplayed2 && !isEvent)
                    {
                        textField.text = " ";
                        player.SetActive(true);
                        isInteraction = false;
                        textDisplayed = false;
                        textDisplayed2 = false;
                        interating = false;
                    }
                }
            }

            // Blade 상호작용
            if(hit.collider.CompareTag("BLADE"))
            {
                if(!isInteraction)
                {
                    interactionUI.SetActive(true);
                }

                // 상호작용 시작
                if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                {
                    isInteraction = true;
                    interating = true;
                    interactionUI.SetActive(false);
                    textField.text = "꽤나 예리한 칼이다.";
                    textDisplayed = true;
                }

                // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
                else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !isEvent)
                {
                    textField.text = "가지고 있을까?";
                    choiceText1Field.text = "1. 줍는다";
                    choiceText2Field.text = "2. 내버려둔다";
                    choicing = true;
                }

                if(choicing)
                {
                    // 1번을 누를 때
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        textField.text = " ";
                        choiceText1Field.text = " ";
                        choiceText2Field.text = " ";
                        getItemTextField.text = "예리한 식칼 획득";
                        Destroy(blade); // 리모컨 비활성화
                        haveBlade = true; // RetroTelevision 조건 활성화

                        StartCoroutine(GetItem());
                    }

                    // 2번을 누를 때
                    else if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        textField.text = " "; // 상호작용 종료
                        choiceText1Field.text = " ";
                        choiceText2Field.text = " ";
                        player.SetActive(true); // 상호작용 종료
                        isInteraction = false;
                        textDisplayed = false;
                        interating = false;
                        choicing = false;
                    }
                }
            }
            //-------------------------------------------------------------------------------------

            // Bathroom -------------------------------------------------------------------------------------
            // Pilers 상호작용
            if(hit.collider.CompareTag("PILERS"))
            {
                if (!isInteraction)
                {
                    interactionUI.SetActive(true);
                }

                // 상호작용 시작
                if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                {
                    isInteraction = true;
                    interating = true;
                    interactionUI.SetActive(false);
                    textField.text = "무언가 뜯어버릴 때 유용할 거 같다.";
                    textDisplayed = true;
                }

                // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
                else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !isEvent)
                {
                    textField.text = "가지고 있을까?";
                    choiceText1Field.text = "1. 줍는다";
                    choiceText2Field.text = "2. 내버려둔다";
                    choicing = true;
                }

                if (choicing)
                {
                    // 1번을 누를 때
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        textField.text = " ";
                        choiceText1Field.text = " ";
                        choiceText2Field.text = " ";
                        getItemTextField.text = "연장 획득";
                        Destroy(pilers); // 리모컨 비활성화
                        havePilers = true; // RetroTelevision 조건 활성화

                        StartCoroutine(GetItem());
                    }

                    // 2번을 누를 때
                    else if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        textField.text = " "; // 상호작용 종료
                        choiceText1Field.text = " ";
                        choiceText2Field.text = " ";
                        player.SetActive(true); // 상호작용 종료
                        isInteraction = false;
                        textDisplayed = false;
                        interating = false;
                        choicing = false;
                    }
                }
            }
        }

        // 플레이어가 상호작용가능한 오브젝트와 멀어질 경우
        else
        {
            interactionUI.SetActive(false);
            isInteraction = false;
        }

        // 플레이어가 상호작용 중일 때 StopControl로 이동
        if(interating)
        {
            StopControl();
        }

        else
        {
            Debug.DrawRay(raycastOrigin.position, raycastOrigin.forward * maxDistance, rayColor);
        }
    }

    // 플레이어는 캐릭터 조종 불가 및 카메라 회전 불가
    void StopControl()
    {
        if(player!= null)
        {
            player.SetActive(false);
        }
    }

    IEnumerator GetItem()
    {
        yield return new WaitForSeconds(0.5f);

        textField.text = " ";
        getItemTextField.text = " ";
        player.SetActive(true); // 상호작용 강제 종료
        isInteraction = false;
        textDisplayed = false;
        interating = false;
        choicing = false;
    }

    IEnumerator OpenStartDoor()
    {
        yield return new WaitForSeconds(7);

        startLeftDoor.SetTrigger("IsOpen");
        startRightDoor.SetTrigger("IsOpen");
        startDoorOpen = true;
    }
}
