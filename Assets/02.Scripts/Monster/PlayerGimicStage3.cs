using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine.AI;
using Unity.Properties;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerGimicStage3 : MonoBehaviour
{
    RaycastHit hit;

    [SerializeField]
    private Transform gameOverZone; // 플레이어가 이동할 게임오버 존
    [SerializeField]
    private Transform lastGameZone; // 라스트 키를 배치할 공간

    [SerializeField]
    private Transform raycastOrigin;
    [SerializeField]
    private Color rayColor = Color.red;
    [SerializeField]
    private GameObject player; // 플레이어 캐릭터
    [SerializeField]
    private Transform playerPosition; // 플레이어 위치
    [SerializeField]
    private LayerMask playerLayer; // 플레이어 레이어
    #region UI 변수
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

    [SerializeField]
    private TMP_InputField passwordInputField; // InputField 변수 선언
    private string correctLobbyPassword = "1028"; //  로비 키패드의 올바른 비밀번호
    private string correctStorage2Password = "9862"; // 창고2 키패드의 올바른 비밀번호

    private string targetTag = "GORE"; // 바꿀 대상의 태그
    [SerializeField]
    private GameObject laboratoryTrap; // 대체할 프리팹
    [SerializeField]
    private TextMeshPro caution2Text; // caution2text의 UI시스템
    #endregion

    #region Generator 변수
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
    [SerializeField]
    private Animator switch2Animator;
    [SerializeField]
    private Animator switch3Animator;
    [SerializeField]
    private Animator switch4Animator;
    [SerializeField]
    private Animator switch5Animator;
    [SerializeField]
    private Animator switch6Animator;
    [SerializeField]
    private Animator switch7Animator;
    [SerializeField]
    private Animator switch8Animator;
    [SerializeField]
    private Animator switch9Animator;
    [SerializeField]
    private Animator switch10Animator;
    [SerializeField]
    private Animator generatorLeverAnimator;
    #endregion

    #region Animator 변수 
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
    private Animator laboratoryLeftDoorAnimator; // 연구실 왼쪽 문
    [SerializeField]
    private Animator laboratoryRightDoorAnimator; // 연구실 오른쪽 문
    [SerializeField]
    private Animator bathroomDoorAnimator; // 화장실 문 애니메이션
    [SerializeField]
    private Animator storage2UpDoorAnimaotr; // 창고 2 위쪽문 애니메이션
    [SerializeField]
    private Animator storage2DownDoorAnimator; // 창고 2 아래쪽문 애니메이션
    [SerializeField]
    private Animator LeverAnimator; // Lever 애니메이션
    [SerializeField]
    private Animator treatmentPlantUpDoorAnimator;  // 처리실 위쪽 문 애니메이션
    [SerializeField]
    private Animator treatmentPlantDownDoorAnimator; // 처리실 아래쪽 문 애니메이션
    #endregion

    #region GameObject 변수
    [SerializeField]
    private GameObject trap; // Trap 오브젝트
    [SerializeField]
    private GameObject trapUI; // TrapUI 오브젝트
    [SerializeField]
    private GameObject pictureFrame; // PictureFrame 오브젝트
    [SerializeField]
    private GameObject tvScreen;
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
    private GameObject generator; // 발전기 오브젝트
    [SerializeField]
    private GameObject restaurantRoomDoorKey; // RestaurantRoomDoorKey 오브젝트
    [SerializeField]
    private GameObject restroomCaution; // RestaurantCaution의 자식으로 있는 Text(TMP)
    [SerializeField]
    private GameObject storageTrap; // Generator의 조작을 실패했을 경우 나타나는 오브젝트
    [SerializeField]
    private GameObject keyPadUI; // KeyPad 오브젝트를 조작할 때 뜨는 UI
    [SerializeField]
    private GameObject lobbyTrap; // 로비에 있는 Trap오브젝트
    [SerializeField]
    private GameObject bathroomCaution; // 화장실 주의 표시 오브젝트
    [SerializeField]
    private GameObject storage2Trap; // 두번째 창고의 비밀번호를 맞았을 경우 생기는 트랩
    [SerializeField]
    private GameObject caution2; // 창고2의 경고문
    [SerializeField]
    private GameObject meatWall; // 고기벽 오브젝트
    [SerializeField]
    private GameObject microwave_gas; // 전자레인지와 가스 오브젝트
    [SerializeField]
    private GameObject lastKeyPrefab; // 라스트 게임 키 프리팹
    #endregion
    
    [SerializeField]
    private Material newScreenMaterial; // TV 스크린 오브젝트의 머티리얼
    private int currentPageIndex = 0; // 페이지 수

    private float maxDistance = 2f; // Raycast 최대 범위

    #region Bool 변수
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
    private bool bathroomDoorOpen = true; // 화장실문 열림 상태

    private bool isEvent = false; // 이벤트 발생 동안

    private bool startDoorOpen = false; // 시작 문이 열림의 유무
    private bool haveRemoteControl = false; // 인벤토리에 RemoteControl이 있을 경우
    private bool isInteractionTouchPad = false; // TouchPad와 상호작용을 했을 경우
    private bool isInteractionMecanicEye = false; // MecanicEye와 상호작용을 했을 경우
    private bool haveAxe = false; // 인벤토리에 Axe오브젝트가 있을 경우
    private bool haveRestaurantDoorKey = false; // 인벤토리에 RestaurantDoorKey가 있을 경우
    private bool haveDoctorHand = false; // 인벤토리에 DoctorHand가 있을 경우
    private bool haveBlade = false; // 인벤토리에 Blade가 있을 경우
    private bool haveMetal = false; // 인벤토리에 Metal이 있을 경우
    private bool havePilers = false; // 인벤토리에 Pilers가 있을 경우
    private bool haveDoctorEye = false; // 인벤토리에 DoctorEye가 있을 경우
    private bool haveboom = false; // 인벤토리에 Boom이 있을 경우
    private bool haveMicrowave_Gas = false; // 인벤토리의 Microwave_Gas 유무
    private bool isMoveBookShelf = false; // BookShelf가 이동했을 경우
    private bool isOperable = false; // 발전기 조작 가능 여부

    private bool lobbyPower = false; // MecanicEye 조작 가능 여부

    private bool unLockStorageDoor = false; // StorageDoor의 해금 상태
    private bool lobbyKeypadUnlock = false; // LobbyKeyPad의 해금 상태
    private bool mecanicEyeUnlock = false; // MecanicEye의 해금 상태
    private bool storage2Unlock = false; // Storage2Keypad의 해금 상태

    private bool treatmentPlantDoorUnLock = false; // 처리실 문의 해금상태

    private bool isInteractionMeatWall = false; // MeatWall과 상호작용 여부

    private bool isLastKeySetting = false; // LastGameKey 세팅 여부
    #endregion

    #region AudioSource 변수
    [SerializeField]
    private AudioSource trapSound; // 트랩 사운드
    [SerializeField]
    private AudioSource[] bookSound; // 책 넘기는 사운드
    [SerializeField]
    private AudioSource roomDoorSound; // 문이 열렸을 때의 효과음
    [SerializeField]
    private AudioSource itemGetSound; // 아이템을 먹었을 때의 사운드
    [SerializeField]
    private AudioSource goreSound; // 시체를 건들 때의 사운드
    [SerializeField]
    private AudioSource padBeep; // 패드가 틀렸을 때의 경고음
    [SerializeField]
    private AudioSource padClearSound; // 패드가 성공했을 때의 사운드
    [SerializeField]
    private AudioSource windSound; // 책장 틈에서 나오는 바람 소리 
    [SerializeField]
    private AudioSource bookShelfSound; // 책장 옮길 때 사운드
    [SerializeField]
    private AudioSource clearDoorSound; // 클리어 문 사운드
    [SerializeField]
    private AudioSource generatorDoorLock; // 발전기 문 안열리는 사운드
    [SerializeField]
    private AudioSource bikeEngineSound; // 바이크 엔진 소리
    [SerializeField]
    private AudioSource generatorButtonSound; // 발전기 돌리는 소리
    [SerializeField]
    private AudioSource meatWallSound; // 고기벽 만지는 소리
    [SerializeField]
    private AudioSource boomSound; // 폭탄터지는 소리
    [SerializeField]
    private AudioSource trashCanSound; // 쓰레기통 사운드
    [SerializeField]
    private AudioSource backgroundSound; // 기본 배경음
    [SerializeField]
    private AudioSource lastGameSound; // 마지막 기믹 사운드
    #endregion

    [SerializeField]
    private NavMeshAgent monster; // 몬스터 네브매쉬
    [SerializeField]
    private NavMeshAgent trashcanMonster; // 트래시 캔 몬스터 네브매시

    private float totalTime = 30f; // 총 시간
    private float currentTime; // 현재 시간
    [SerializeField]
    private TextMeshProUGUI timerText; // UI에 시간을 표시할 Text 객체
    [SerializeField]
    private GameObject lastTimer; // 타임어택 UI;

    private void Start()
    {
       
    }

    private void Update()
    {
        if (!startDoorOpen)
            StartCoroutine(OpenStartDoor());

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {
            Debug.Log(hit.collider.gameObject.name);

            // 레이가 Player에 충돌할 경우
            if (hit.collider.CompareTag("Player"))
            {
                return;
            }

            #region Restroom
            // PictureFrame 상호작용
            if (hit.collider.CompareTag("PICTUREFRAME"))
                PictureFrame();

            // Diary 상호작용
            if (hit.collider.CompareTag("DIARY"))
                Diray();

            // RetroTelevision 상호작용
            if (hit.collider.CompareTag("RETROTELEVISION"))
                RetroTV();

            // Lamp상호작용
            if (hit.collider.CompareTag("LAMP"))
                Lamp();

            // RestroomDoor상호작용
            if (hit.collider.CompareTag("RESTROOMDOOR"))
                RestroomDoor();

            // ClipBoard상호작용
            if (hit.collider.CompareTag("CLIPBOARD"))
                Clipboard();

            // RemoteControl 상호작용
            if (hit.collider.CompareTag("REMOTECONTROL"))
                RemoteControl();

            // DeadDoctor 상호작용
            if (hit.collider.CompareTag("DEADDOCTOR"))
                DeadDoctor();

            // RestaurantRoomDoorKey 상호작용
            if (hit.collider.CompareTag("RESTAURANTDOORKEY"))
                RestaurantDoorKey();

            // Bag 상호작용
            if (hit.collider.CompareTag("BAG"))
                Bag();

            // TouchPad 상호작용
            if (hit.collider.CompareTag("TOUCHPAD"))
                TouchPad();

            // BookShelf 상호작용
            if (hit.collider.CompareTag("BOOKSHELF"))
                Bookshelf();

            // HiddenRoomDoor 상호작용
            if (hit.collider.CompareTag("HIDDENROOMDOOR"))
                HiddenroomDoor();

            // ClearDoor 상호작용
            if (hit.collider.CompareTag("CLEARDOOR"))
                Cleardoor();
            #endregion

            #region Lobby
            // MecanicEye 상호작용
            if (hit.collider.CompareTag("MECANICEYE"))
                MecanicEye();

            // LobbyKeyPad 상호작용
            if (hit.collider.CompareTag("LOBBYKEYPAD"))
                LobbyKeyPad();

            #endregion

            #region Restaurant
            // RestaurantDoor 상호작용
            if (hit.collider.CompareTag("RESTAURANTDOOR"))
                RestaurantDoor();

            //Metal 상호작용
            if (hit.collider.CompareTag("METAL"))
                Metal();

            // LastGame 상호작용
            if (hit.collider.CompareTag("LASTGAME"))
            {
                if (haveMetal && haveMicrowave_Gas)
                    LastGame();
            }
            #endregion

            #region Storage
            // Generator 상호작용
            if (hit.collider.CompareTag("GENERATOR"))
                Generator();

            // Bike 상호작용
            if (hit.collider.CompareTag("BIKE"))
                Bike();

            // StorageDoor 상호작용
            if (hit.collider.CompareTag("STORAGEDOOR"))
                StorageDoor();

            // Blade 상호작용
            if (hit.collider.CompareTag("BLADE"))
                Blade();

            // Option 상호작용
            if (hit.collider.CompareTag("OPTION"))
            {
                if(!haveboom && isInteractionMeatWall)
                    Option();
            }
            #endregion

            #region Bathroom
            // Pilers 상호작용
            if (hit.collider.CompareTag("PILERS"))
                Pilers();

            // BathroomDoor 상호작용
            if (hit.collider.CompareTag("BATHROOMDOOR"))
                BathroomDoor();
            #endregion

            #region Laboratory
            // Gore 오브젝트 상호작용
            if (hit.collider.CompareTag("GORE"))
                Gore();

            // Storage2KeyPad 상호작용
            if (hit.collider.CompareTag("STORAGE2KEYPAD"))
            {
                if(!storage2Unlock)
                {
                    Storage2KeyPad();
                }
            }

            // Lever 상호작용
            if (hit.collider.CompareTag("LEVER"))
            {
                if(!treatmentPlantDoorUnLock)
                    Lever();
            }
            #endregion

            #region Treatmentplant
            // MeatWall 오브젝트와 상호작용 할 때
            if (hit.collider.CompareTag("MEATWALL"))
                MeatWall();

            // MicroWave 상호작용
            if (hit.collider.CompareTag("MICROWAVE"))
                Microwave();

            // TrashCan 상호작용
            if (hit.collider.CompareTag("TRASHCAN"))
                TrashCan();
            #endregion
        }

        // 플레이어가 상호작용가능한 오브젝트와 멀어질 경우
        else
        {
            interactionUI.SetActive(false);
            isInteraction = false;
        }

        // 플레이어가 상호작용 중일 때 StopControl로 이동
        if (interating)
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
        if (player != null)
        {
            player.SetActive(false);
        }
    }

    #region Restroom
    void PictureFrame()
    {
        // 플레이어가 다른 오브젝트 또는 해당 오브젝트와 상호작용 중이지 않을 때 PictureFrame과 상호작용 가능 여부
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
            textField.text = "꽤나 큰 액자다...";
            textDisplayed = true;
        }

        // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
        else if (Input.GetKeyDown(KeyCode.E) && textDisplayed)
        {
            textField.text = "들춰낼까?";
            choiceText1Field.text = "1. 들춰낸다";
            choiceText2Field.text = "2. 내버려둔다";
            choicing = true;
        }

        // 선택지가 출력되었을 때 가능한 버튼
        if (choicing)
        {
            // 1번을 누를 때
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                textField.text = " "; // 상호작용 종료
                choiceText1Field.text = " ";
                choiceText2Field.text = " ";
                trap.SetActive(true); // 함정 생성
                trapSound.Play(); // 트랩 사운드 재생
                pictureFrame.SetActive(false); // 함정 생성

                StartCoroutine(GameOver());
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

    void Diray()
    {
        // 플레이어가 다른 오브젝트 또는 해당 오브젝트와 상호작용을 하지 않을 때
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
                bookSound[0].Play(); // 책 넘기는 사운드 재생
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
                bookSound[0].Play(); // 책 넘기는 사운드 재생
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

    void RetroTV()
    {
        Renderer renderer = tvScreen.GetComponent<Renderer>();

        // 플레이어가 다른 오브젝트 또는 해당 오브젝트와 상호작용을 하지 않을 때
        if (!isInteraction)
        {
            interactionUI.SetActive(true);
        }

        // 일반상호작용(인벤토리에 RemoteControl이 없을 경우)
        if (!haveRemoteControl)
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
        if (haveRemoteControl)
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
                    renderer.material = newScreenMaterial; // TV에 몬스터의 얼굴이 뜸.
                    trapSound.Play();

                    StartCoroutine(GameOver()); // 게임 오버
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

    void Lamp()
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

    void RestroomDoor()
    {
        if (!isInteraction)
        {
            interactionUI.SetActive(true);
        }

        // 상호작용 시작, 문이 열리고 닫힘.
        if (Input.GetKeyDown(KeyCode.E))
        {
            // 문 열리는 사운드 재생
            roomDoorSound.Play();

            if (restroomOpen)
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

    void Clipboard()
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
                bookSound[1].Play(); // 책 넘기는 소리 재생
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
                bookSound[1].Play(); // 책을 넘길 때마다 사운드 재생
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

    void RemoteControl()
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

        if (choicing)
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
                itemGetSound.Play(); // 아이템을 먹었을 때의 사운드 재생

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

    void DeadDoctor()
    {
        if (!isInteraction)
        {
            interactionUI.SetActive(true);
        }

        if (!unLockStorageDoor)
        {
            // 일반상호작용
            if (!isInteractionTouchPad)
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

            // 특수상호작용1(TouchPad 오브젝트와 한 번이라도 상호작용 했을 때)
            if (isInteractionTouchPad)
            {
                // 특수상호작용 1-1
                if (!haveAxe)
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
                if (haveAxe)
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

                    if (choicing)
                    {
                        // 1번을 누를 때
                        if (Input.GetKeyDown(KeyCode.Alpha1))
                        {
                            textField.text = " "; // 상호작용 종료
                            choiceText1Field.text = " ";
                            choiceText2Field.text = " ";
                            getItemTextField.text = "절단된 의사의 손 획득";
                            haveDoctorHand = true;
                            goreSound.Play();

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
        }

        if (lobbyPower)
        {
            // 특수상호작용2(MecanicEye와 한번이라도 상호작용 했을 때)
            if (isInteractionMecanicEye)
            {
                // 특수상호작용 2-1(인벤토리에 Blade가 없을 경우)
                if (!haveBlade)
                {
                    if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !textDisplayed2 && !isEvent)
                    {
                        isInteraction = true;
                        interating = true;
                        interactionUI.SetActive(false);
                        textField.text = "이 시체의 눈으로 인식이 가능할 것이다.";
                        textDisplayed = true;
                    }

                    else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !textDisplayed2 && !isEvent)
                    {
                        textField.text = "파낼만한 것을 찾아보자";
                        textDisplayed2 = true;
                    }

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
                // 특수상호작용 2-2(인벤토리에 Blade가 있을 경우)
                if (haveBlade)
                {
                    // 특수상호작용 2-1 시작
                    if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                    {
                        isInteraction = true;
                        interating = true;
                        interactionUI.SetActive(false);
                        textField.text = "시체의 눈을 파낼까?";
                        textDisplayed = true;
                    }

                    // 특수상호작용 2-1 선택지
                    else if (Input.GetKeyDown(KeyCode.E) && textDisplayed)
                    {
                        textField.text = "파낼까?";
                        choiceText1Field.text = "1. 파낸다";
                        choiceText2Field.text = "2. 내버려둔다";
                        choicing = true;
                    }

                    if (choicing)
                    {
                        // 1번을 누를 때
                        if (Input.GetKeyDown(KeyCode.Alpha1))
                        {
                            goreSound.Play(); // 사운드 재생
                            textField.text = " "; // 상호작용 종료
                            choiceText1Field.text = " ";
                            choiceText2Field.text = " ";
                            getItemTextField.text = "적출된 의사의 눈 획득";
                            haveDoctorEye = true;

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
        }
    }

    void RestaurantDoorKey()
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
            textField.text = "다른 방을 열 수 있는 열쇠인거 같다..";
            interactionUI.SetActive(false);
            haveRestaurantDoorKey = true;
            textDisplayed = true;
        }

        // 상호작용 종료
        else if (Input.GetKeyDown(KeyCode.E) && textDisplayed)
        {
            itemGetSound.Play();
            textField.text = " "; // 상호작용 종료
            choiceText1Field.text = " ";
            choiceText2Field.text = " ";
            getItemTextField.text = "식당 열쇠 획득";
            Destroy(restaurantRoomDoorKey);
            restroomCaution.SetActive(true);

            StartCoroutine(GetItem());
        }
    }

    void Bag()
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
                    itemGetSound.Play(); // 사운드 재생

                    StartCoroutine(GetItem()); // 상호작용 종료
                }
            }

            // 인벤토리에 Axe오브젝트가 있을 경우
            else if (haveAxe)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
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

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                textField.text = "아무것도 없다";
                choiceText1Field.text = " ";
                choiceText2Field.text = " ";
                choiceText3Field.text = " ";

                StartCoroutine(GetItem()); // 상호작용 종료
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                trapSound.Play(); // 사운드 재생
                textField.text = " ";
                choiceText1Field.text = " ";
                choiceText2Field.text = " ";
                choiceText3Field.text = " ";
                trapUI.SetActive(true); // 게임 오버

                StartCoroutine(GameOver());
            }
        }
    }

    void TouchPad()
    {
        if (!isInteraction)
        {
            interactionUI.SetActive(true);
        }

        //일반상호작용(인벤토리에 절단된 의사의 손이 없을 경우)
        if (!haveDoctorHand)
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
                padBeep.Play(); // 사운드 재생
                textField.text = "적당한 것을 찾아보자";

                StartCoroutine(GetItem());
            }
        }

        //특수상호작용(인벤토리에 절단된 의사의 손이 있을경우)
        if (haveDoctorHand)
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
                padClearSound.Play(); // 사운드 재생
                textField.text = " ";
                unLockStorageDoor = true;
                storageDoorAnimator.SetBool("IsUnlock", true); // 창고문이 열림

                player.SetActive(true); // 상호작용 강제 종료
                isInteraction = false;
                textDisplayed = false;
                interating = false;
                choicing = false;
            }
        }
    }

    void Bookshelf()
    {
        if(!isMoveBookShelf)
        {
            if (!isInteraction)
            {
                interactionUI.SetActive(true);
            }

            // 일반상호작용
            if (!isDiary3Open)
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
                    windSound.Play();
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

                if (choicing)
                {
                    // 1번을 누를 때
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        bookShelfSound.Play();
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
    }

    void HiddenroomDoor()
    {
        if (!isInteraction)
        {
            interactionUI.SetActive(true);
        }

        // BookShelf가 이동하지 않은 상태에서 HiddenRoomDoor와 상호작용이 되는 것을 방지
        if (isMoveBookShelf)
        {
            // 상호작용 시작, 문이 열리고 닫힘.
            if (Input.GetKeyDown(KeyCode.E))
            {
                // 문 열리는 사운드 재생
                roomDoorSound.Play();

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

    void Cleardoor()
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
                clearDoorSound.Play(); // 사운드 재생
                textField.text = " "; // 상호작용 종료
                choiceText1Field.text = " ";
                choiceText2Field.text = " ";
                player.SetActive(true); // 상호작용 종료
                isInteraction = false;
                textDisplayed = false;
                interating = false;
                choicing = false;

                clearDoorAnimator.SetBool("IsOpen", true);

                StartCoroutine(DestinationMonster());
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
    #endregion

    #region Lobby
    void MecanicEye()
    {
        if(!mecanicEyeUnlock)
        {
            if (!isInteraction)
            {
                interactionUI.SetActive(true);
            }

            // 일반상호작용 (발전기 스위칭을 하지 않았을 시)
            if (!lobbyPower)
            {
                // 일반상호작용 시작
                if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !textDisplayed2 && !isEvent)
                {
                    isInteractionMecanicEye = true;
                    isInteraction = true;
                    interating = true;
                    interactionUI.SetActive(false);
                    textField.text = "홍채인식으로 작동하는거 같다.";
                    textDisplayed = true;
                }

                // 일반상호작용 중
                else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !textDisplayed2 && !isEvent)
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

            // 특수상호작용1 (발전기 스위칭에 성공했을 때)
            if (lobbyPower)
            {
                // 특수상호작용 1-1 (인벤토리에 DoctorEye가 없을 때)
                if (!haveDoctorEye)
                {
                    if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !textDisplayed2 && !isEvent)
                    {
                        isInteractionMecanicEye = true;
                        isInteraction = true;
                        interating = true;
                        interactionUI.SetActive(false);
                        textField.text = "눈을 한번 대보자..";
                        textDisplayed = true;
                    }

                    else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !textDisplayed2 && !isEvent)
                    {
                        textField.text = "적합한 것을 찾아보자.";
                        padBeep.Play();
                        textDisplayed2 = true;

                    }

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

                // 특수상호작용 1-2(인벤토리에 DoctorEye가 있을경우)
                if (haveDoctorEye)
                {
                    // 상호작용 시작
                    if (Input.GetKeyDown(KeyCode.E) && !textDisplayed)
                    {
                        isInteraction = true;
                        interating = true;
                        interactionUI.SetActive(false);
                        textField.text = "이 눈을 기계에 대보자.";
                        textDisplayed = true;
                    }

                    // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
                    else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !isEvent)
                    {
                        textField.text = "인식시켜 볼까?";
                        choiceText1Field.text = "1. 인식시킨다";
                        choiceText2Field.text = "2. 내버려둔다";
                        choicing = true;
                    }

                    // 선택지가 출력되었을 때 가능한 버튼
                    if (choicing)
                    {
                        // 1번을 누를 때
                        if (Input.GetKeyDown(KeyCode.Alpha1))
                        {
                            // 키패드가 먼저 해금되었을경우
                            if (lobbyKeypadUnlock)
                            {
                                textField.text = " "; // 상호작용 종료
                                choiceText1Field.text = " ";
                                choiceText2Field.text = " ";
                                mecanicEyeUnlock = true;
                                padClearSound.Play(); // 사운드 재생
                                laboratoryLeftDoorAnimator.SetBool("IsOpen", true);
                                laboratoryRightDoorAnimator.SetBool("IsOpen", true);

                                player.SetActive(true); // 상호작용 종료
                                isInteraction = false;
                                textDisplayed = false;
                                interating = false;
                                choicing = false;
                            }

                            // 키패드가 해금되지 않았을 경우
                            else if (!lobbyKeypadUnlock)
                            {
                                textField.text = " "; // 상호작용 종료
                                choiceText1Field.text = " ";
                                choiceText2Field.text = " ";
                                trapSound.Play(); // 사운드 재생
                                lobbyTrap.SetActive(true);

                                StartCoroutine(GameOver());
                            }
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
        }
    }

    void LobbyKeyPad()
    {
        if(!lobbyKeypadUnlock)
        {
            if (!isInteraction)
            {
                interactionUI.SetActive(true);
            }

            // 일반상호작용(Generator가 기능하지 않았을 경우)
            if (!lobbyPower)
            {
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

            // 특수상호작용
            else
            {
                // 상호작용 시작
                if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                {
                    isInteraction = true;
                    interating = true;
                    interactionUI.SetActive(false);
                    textField.text = "전원이 들어왔다..";
                    textDisplayed = true;
                }

                // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
                else if (Input.GetKeyDown(KeyCode.E) && textDisplayed)
                {
                    textField.text = "패드를 조작할까?";
                    choiceText1Field.text = "1. 조작한다";
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

                // KeyPad 조작 시작
                if (isEvent && !choicing && !textDisplayed && isInteraction && interating)
                {
                    keyPadUI.SetActive(true); // KeyPadUI 출력
                    choiceText1Field.text = "비밀번호 확인 E";
                    choiceText2Field.text = "상호작용 종료 Q";

                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        keyPadUI.SetActive(false); // KeyPadUI창 닫기
                        choiceText1Field.text = " ";
                        choiceText2Field.text = " ";
                        padBeep.Play(); // 사운드 재생

                        player.SetActive(true); // 상호작용 강제 종료
                        isInteraction = false;
                        textDisplayed = false;
                        interating = false;
                        choicing = false;
                        isEvent = false;
                    }

                    CheckLobbyPassword();
                }
            }
        }
    }
    #endregion

    #region Restaurant
    void RestaurantDoor()
    {
        if (!isInteraction)
        {
            interactionUI.SetActive(true);
        }

        // 일반상호작용(식당 문 열쇠가 없을 경우)
        if (!haveRestaurantDoorKey)
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
        else if (haveRestaurantDoorKey)
        {
            // 상호작용 시작, 문이 열리고 닫힘.
            if (Input.GetKeyDown(KeyCode.E))
            {
                roomDoorSound.Play(); // 사운드 재생

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

    void Metal()
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
                itemGetSound.Play(); // 사운드 재생
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

    void LastGame()
    {
        if (!isInteraction)
        {
            interactionUI.SetActive(true);
        }

        // 일반상호작용(전자레인지 설치가 안됬을 때)
        if(!isLastKeySetting)
        {
            // 상호작용 시작
            if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
            {
                isInteraction = true;
                interating = true;
                interactionUI.SetActive(false);
                textField.text = "전자레인지안에 가스통과 식기를 설치하자";
                textDisplayed = true;
            }

            // 상호작용 종료
            else if (Input.GetKeyDown(KeyCode.E) && textDisplayed)
            {
                textField.text = " "; // 상호작용 종료
                choiceText1Field.text = " ";
                choiceText2Field.text = " ";

                // LastKey 세팅
                GameObject newPrefabInstance = Instantiate(lastKeyPrefab, lastGameZone.position, lastGameZone.rotation);
                newPrefabInstance.transform.position = lastGameZone.position;
                isLastKeySetting = true;
                itemGetSound.Play(); // 사운드 재생

                StartCoroutine(GetItem());
            }
        }

        // 특수상호작용(전자레인지가 설치 됬을 때
        else if(isLastKeySetting)
        {
            if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !textDisplayed2 && !isEvent)
            {
                isInteractionMecanicEye = true;
                isInteraction = true;
                interating = true;
                interactionUI.SetActive(false);
                textField.text = "타이밍에 맞춰 몬스터를 유인하고";
                textDisplayed = true;
            }

            else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !textDisplayed2 && !isEvent)
            {
                textField.text = "전자레인지를 기폭시킨다..";
                textDisplayed2 = true;
            }

            else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && textDisplayed2 && !isEvent)
            {
                textField.text = "준비는 끝났나?";
                choiceText1Field.text = "1. 가동시킨다";
                choiceText2Field.text = "2. 내버려둔다";
                choicing = true;
            }

            // 선택지가 출력되었을 때 가능한 버튼
            if (choicing)
            {
                // 1번을 누를 때
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    Destroy(backgroundSound); // 기존 배경음 삭제
                    lastGameSound.Play(); // 마지막 게임 사운드 재생
                    textField.text = " "; // 상호작용 종료
                    choiceText1Field.text = " ";
                    choiceText2Field.text = " ";

                    // 타이머 작동
                    lastTimer.SetActive(true);
                    currentTime = totalTime;
                    UpdateTimerUI();
                    // 1초마다 UpdateTimer 함수 호출
                    InvokeRepeating("UpdateTimer", 1f, 1f);

                    player.SetActive(true); // 상호작용 종료
                    isInteraction = false;
                    textDisplayed = false;
                    textDisplayed2 = false;
                    interating = false;
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
                    textDisplayed2 = false;
                    interating = false;
                    choicing = false;
                }
            }
        }
    }
    #endregion

    #region Storage
    void Generator()
    {
        if (!lobbyPower)
        {
            if (!isInteraction)
            {
                interactionUI.SetActive(true);
            }

            // 일반상호작용(인벤토리에 Pilers가 없을 경우)
            if (!havePilers)
            {
                // 상호작용 시작
                if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !isEvent)
                {
                    isInteraction = true;
                    interating = true;
                    interactionUI.SetActive(false);
                    generatorDoorLock.Play(); // 사운드 재생
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
            if (havePilers && !isOperable)
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
                    clearDoorSound.Play(); // 사운드 재생
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
            if (isOperable)
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
                    choiceText1Field.text = "강제 종료 Q";
                    choiceText2Field.text = "레버 당기기 E";

                    // 1번 스위치 조작
                    if (Input.GetKeyDown(KeyCode.Keypad1))
                    {
                        generatorButtonSound.Play(); // 사운드 재생

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
                        generatorButtonSound.Play(); // 사운드 재생

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

                    // 3번 스위치 조작
                    if (Input.GetKeyDown(KeyCode.Keypad3))
                    {
                        generatorButtonSound.Play(); // 사운드 재생

                        if (switch3)
                        {
                            switch3Animator.SetBool("IsMove", true);
                        }
                        else
                        {
                            switch3Animator.SetBool("IsMove", false);
                        }

                        switch3 = !switch3;
                    }

                    // 4번 스위치 조작
                    if (Input.GetKeyDown(KeyCode.Keypad4))
                    {
                        generatorButtonSound.Play(); // 사운드 재생

                        if (switch4)
                        {
                            switch4Animator.SetBool("IsMove", true);
                        }
                        else
                        {
                            switch4Animator.SetBool("IsMove", false);
                        }

                        switch4 = !switch4;
                    }

                    // 5번 스위치 조작
                    if (Input.GetKeyDown(KeyCode.Keypad5))
                    {
                        generatorButtonSound.Play(); // 사운드 재생

                        if (switch5)
                        {
                            switch5Animator.SetBool("IsMove", true);
                        }
                        else
                        {
                            switch5Animator.SetBool("IsMove", false);
                        }

                        switch5 = !switch5;
                    }

                    // 6번 스위치 조작
                    if (Input.GetKeyDown(KeyCode.Keypad6))
                    {
                        generatorButtonSound.Play(); // 사운드 재생

                        if (switch6)
                        {
                            switch6Animator.SetBool("IsMove", true);
                        }
                        else
                        {
                            switch6Animator.SetBool("IsMove", false);
                        }

                        switch6 = !switch6;
                    }

                    // 7번 스위치 조작
                    if (Input.GetKeyDown(KeyCode.Keypad7))
                    {
                        generatorButtonSound.Play(); // 사운드 재생

                        if (switch7)
                        {
                            switch7Animator.SetBool("IsMove", true);
                        }
                        else
                        {
                            switch7Animator.SetBool("IsMove", false);
                        }

                        switch7 = !switch7;
                    }

                    // 8번 스위치 조작
                    if (Input.GetKeyDown(KeyCode.Keypad8))
                    {
                        generatorButtonSound.Play(); // 사운드 재생

                        if (switch8)
                        {
                            switch8Animator.SetBool("IsMove", true);
                        }
                        else
                        {
                            switch8Animator.SetBool("IsMove", false);
                        }

                        switch8 = !switch8;
                    }

                    // 9번 스위치 조작
                    if (Input.GetKeyDown(KeyCode.Keypad9))
                    {
                        generatorButtonSound.Play(); // 사운드 재생

                        if (switch9)
                        {
                            switch9Animator.SetBool("IsMove", true);
                        }
                        else
                        {
                            switch9Animator.SetBool("IsMove", false);
                        }

                        switch9 = !switch9;
                    }

                    // 10번 스위치 조작
                    if (Input.GetKeyDown(KeyCode.Keypad0))
                    {
                        generatorButtonSound.Play(); // 사운드 재생

                        if (switch10)
                        {
                            switch10Animator.SetBool("IsMove", true);
                        }
                        else
                        {
                            switch10Animator.SetBool("IsMove", false);
                        }

                        switch10 = !switch10;
                    }

                    // 조작 도중 강제 종료
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        padBeep.Play();
                        generatorButtonUI.SetActive(false);
                        choiceText1Field.text = " ";
                        choiceText2Field.text = " ";
                        player.SetActive(true); // 상호작용 강제 종료
                        isInteraction = false;
                        textDisplayed = false;
                        interating = false;
                        choicing = false;
                        isEvent = false;
                    }

                    // 레버 당기기
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        generatorLeverAnimator.SetBool("IsMove", true);

                        // 스위치를 올바르게 옮겼을 때
                        if (switch1 && !switch2 && !switch3 && !switch4 && switch5 && !switch6 && switch7 && switch8 && !switch9 && !switch10)
                        {
                            // 로비에 있는 MecanicEye와 LobbyKeyPad 특수상호작용 가능
                            padClearSound.Play();
                            lobbyPower = true;
                            generatorButtonUI.SetActive(false);
                            choiceText1Field.text = " ";
                            choiceText2Field.text = " ";
                            player.SetActive(true); // 상호작용 강제 종료
                            isInteraction = false;
                            textDisplayed = false;
                            interating = false;
                            choicing = false;
                            isEvent = false;
                        }

                        // 스위치를 틀렸을 때
                        else
                        {
                            trapSound.Play();
                            Destroy(generator);
                            storageTrap.SetActive(true);

                            StartCoroutine(GameOver()); // 게임 오버
                        }
                    }
                }
            }
        }
    }

    void Bike()
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

    void StorageDoor()
    {
        if (!storageDoorOpen)
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

    void Blade()
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

        if (choicing)
        {
            // 1번을 누를 때
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                textField.text = " ";
                choiceText1Field.text = " ";
                choiceText2Field.text = " ";
                getItemTextField.text = "예리한 식칼 획득";
                itemGetSound.Play(); // 사운드 재생
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

    void Option()
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
            textField.text = "여러 상자들이 쌓여있다..";
            textDisplayed = true;
        }

        // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
        else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !isEvent)
        {
            textField.text = "쓸만한게 있을지 모른다";
            choiceText1Field.text = "1. 뒤져본다";
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
                getItemTextField.text = "폭탄 획득";
                itemGetSound.Play(); // 사운드 재생
                haveboom = true;

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
    #endregion

    #region Bathroom
    void Pilers()
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
                itemGetSound.Play(); // 사운드 재생
                Destroy(pilers); // 연장 비활성화
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

    void BathroomDoor()
    {
        if (!isInteraction)
        {
            interactionUI.SetActive(true);
        }

        // 상호작용 시작, 문이 열리고 닫힘.
        if (Input.GetKeyDown(KeyCode.E))
        {
            roomDoorSound.Play(); // 사운드 재생

            bathroomCaution.SetActive(true);

            if (bathroomDoorOpen)
            {
                bathroomDoorAnimator.SetBool("IsOpen", true);
            }
            else
            {
                bathroomDoorAnimator.SetBool("IsOpen", false);
            }

            bathroomDoorOpen = !bathroomDoorOpen;
        }
    }
    #endregion

    #region Laboratory
    void Gore()
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
            textField.text = "심하게 훼손된 시체다..";
            textDisplayed = true;
        }

        // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
        else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !isEvent)
        {
            textField.text = "이 시체들 중 앞으로 나아가는 힌트가 있다..";
            choiceText1Field.text = "1. 안을 뒤져본다";
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

                LaboratoryGameOver();
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

    void Storage2KeyPad()
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
            textField.text = "또 다른 키패드다..";
            textDisplayed = true;
        }

        // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
        else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !isEvent)
        {
            textField.text = "조작할까?";
            choiceText1Field.text = "1. 조작한다";
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

        // KeyPad 조작 시작
        if (isEvent && !choicing && !textDisplayed && isInteraction && interating)
        {
            keyPadUI.SetActive(true); // KeyPadUI 출력
            choiceText1Field.text = "비밀번호 확인 E";
            choiceText2Field.text = "상호작용 종료 Q";

            if (Input.GetKeyDown(KeyCode.Q))
            {
                padBeep.Play(); // 사운드 재생
                keyPadUI.SetActive(false); // KeyPadUI창 닫기
                choiceText1Field.text = " ";
                choiceText2Field.text = " ";

                player.SetActive(true); // 상호작용 강제 종료
                isInteraction = false;
                textDisplayed = false;
                interating = false;
                choicing = false;
                isEvent = false;
            }

            CheckStorage2Password();
        }
    }

    void Lever()
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
            textField.text = "수상한 레버다..";
            textDisplayed = true;
        }

        // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
        else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !isEvent)
        {
            textField.text = "조작할까?";
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

                player.SetActive(true); // 상호작용 종료
                isInteraction = false;
                textDisplayed = false;
                interating = false;
                choicing = false;

                // 레버가 움직임
                LeverAnimator.SetBool("IsMove", true);
                generatorButtonSound.Play(); // 사운드 재생

                // 처리실 문이 열림
                treatmentPlantDownDoorAnimator.SetBool("IsOpen", true);
                treatmentPlantUpDoorAnimator.SetBool("IsOpen", true);

                treatmentPlantDoorUnLock = true;
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
    #endregion

    #region TreatmentPlant
    void MeatWall()
    {
        if (!isInteraction)
        {
            interactionUI.SetActive(true);
        }

        // 일반상호작용 (인벤토리에 폭탄이 없을 시)
        if (!haveboom)
        {
            // 일반상호작용 시작
            if (Input.GetKeyDown(KeyCode.E) && !textDisplayed && !textDisplayed2 && !isEvent)
            {
                isInteractionMecanicEye = true;
                isInteraction = true;
                interating = true;
                interactionUI.SetActive(false);
                meatWallSound.Play(); // 사운드 재생
                textField.text = "이쪽 벽만 유독 물컹거린다.";
                textDisplayed = true;
                isInteractionMeatWall = true; // 
            }

            // 일반상호작용 중
            else if (Input.GetKeyDown(KeyCode.E) && textDisplayed && !textDisplayed2 && !isEvent)
            {
                textField.text = "이 벽을 없앨만한 것을 찾아보자";
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

        // 특수상호작용 (인벤토리에 폭탄이 있을 경우)
        if(haveboom)
        {
            // 상호작용 시작
            if (Input.GetKeyDown(KeyCode.E) && !textDisplayed)
            {
                isInteraction = true;
                interating = true;
                interactionUI.SetActive(false);
                textField.text = "폭탄으로 이 벽을 부술 수 있을것이다";
                textDisplayed = true;
            }

            // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
            else if (Input.GetKeyDown(KeyCode.E) && textDisplayed)
            {
                textField.text = "부숴볼까?";
                choiceText1Field.text = "1. 부순다";
                choiceText2Field.text = "2. 내버려둔다";
                choicing = true;
            }

            // 선택지가 출력되었을 때 가능한 버튼
            if (choicing)
            {
                // 1번을 누를 때
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    textField.text = " "; // 상호작용 종료
                    choiceText1Field.text = " ";
                    choiceText2Field.text = " ";
                    player.SetActive(true); // 상호작용 종료
                    isInteraction = false;
                    textDisplayed = false;
                    interating = false;
                    choicing = false;

                    boomSound.Play(); // 사운드 재생
                    Destroy(meatWall); // 고기벽 삭제
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

    void Microwave()
    {
        if(!isInteraction)
        {
            interactionUI.SetActive(true);
        }

        // 상호작용 시작
        if (Input.GetKeyDown(KeyCode.E) && !textDisplayed)
        {
            isInteraction = true;
            interating = true;
            interactionUI.SetActive(false);
            textField.text = "왜 이런곳에 전자레인지하고 가스가...";
            textDisplayed = true;
        }

        // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
        else if (Input.GetKeyDown(KeyCode.E) && textDisplayed)
        {
            textField.text = "챙길까?";
            choiceText1Field.text = "1. 챙긴다";
            choiceText2Field.text = "2. 내버려둔다";
            choicing = true;
        }

        // 선택지
        if (choicing)
        {
            // 1번을 누를 때
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                textField.text = " ";
                choiceText1Field.text = " ";
                choiceText2Field.text = " ";
                getItemTextField.text = "전자레인지와 가스 획득";
                Destroy(microwave_gas); // 전자레인지 삭제
                itemGetSound.Play(); // 사운드 재생
                haveMicrowave_Gas = true; // 인벤토리에 Microwave 획득

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

    void TrashCan()
    {
        if(!isInteraction)
        {
            interactionUI.SetActive(true);
        }

        // 상호작용 시작
        if (Input.GetKeyDown(KeyCode.E) && !textDisplayed)
        {
            isInteraction = true;
            interating = true;
            interactionUI.SetActive(false);
            textField.text = "뒤에 숨겨진 통로가 있다..";
            textDisplayed = true;
        }

        // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
        else if (Input.GetKeyDown(KeyCode.E) && textDisplayed)
        {
            textField.text = "여기로 빠져나갈수 있을것이다...";
            choiceText1Field.text = "1. 빠져나간다";
            choiceText2Field.text = "2. 가만히 있는다";
            choicing = true;
        }

        // 선택지
        if (choicing)
        {
            // 1번을 누를 때
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                textField.text = " ";
                choiceText1Field.text = " ";
                choiceText2Field.text = " ";
                trashCanSound.Play(); // 사운드 재생

                player.SetActive(true); // 상호작용 종료
                isInteraction = false;
                textDisplayed = false;
                interating = false;
                choicing = false;

                player.transform.position = gameOverZone.transform.position;

                StartCoroutine(DestinationTrashCanMonster());
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
    #endregion

    // LobbyKeypad의 InputField 비밀번호 해금 이벤트
    public void CheckLobbyPassword()
    {
        string input = passwordInputField.text; // 입력된 비밀번호

        if(Input.GetKeyDown(KeyCode.E))
        {
            // 비밀번호가 일치할 경우
            if (input == correctLobbyPassword)
            {
                choiceText1Field.text = " ";
                choiceText2Field.text = " ";
                lobbyKeypadUnlock = true; // 로비 키패드를 해금함.
                padClearSound.Play(); // 사운드 재생
                keyPadUI.SetActive(false); // KeyPadUI창 닫기

                player.SetActive(true); // 상호작용 강제 종료
                isInteraction = false;
                textDisplayed = false;
                interating = false;
                choicing = false;
                isEvent = false;
            }

            // 비밀번호가 실패할 경우 게임오버
            else
            {
                choiceText1Field.text = " ";
                choiceText2Field.text = " ";
                keyPadUI.SetActive(false); // KeyPadUI창 닫기
                trapSound.Play(); // 사운드 재생
                lobbyTrap.SetActive(true);
            }
        }
    }

    public void CheckStorage2Password()
    {
        string input = passwordInputField.text; // 입력된 비밀번호

        if (Input.GetKeyDown(KeyCode.E))
        {
            // 비밀번호가 일치할 경우 게임오버
            if (input == correctStorage2Password)
            {
                trapSound.Play(); // 사운드 재생
                choiceText1Field.text = " ";
                choiceText2Field.text = " ";
                keyPadUI.SetActive(false); // KeyPadUI창 닫기
                storage2Trap.SetActive(true); // 함정 가동

                StartCoroutine(GameOver()); // 게임 오버
            }

            // 비밀번호가 다를 경우 해금
            else
            {
                choiceText1Field.text = " ";
                choiceText2Field.text = " ";
                storage2Unlock = true; // 로비 키패드를 해금함.
                keyPadUI.SetActive(false); // KeyPadUI창 닫기
                caution2.SetActive(true);

                // Storage2Door 문이 열림
                padClearSound.Play(); // 사운드 재생
                storage2DownDoorAnimator.SetBool("IsOpen", true);
                storage2UpDoorAnimaotr.SetBool("IsOpen", true);

                player.SetActive(true); // 상호작용 강제 종료
                isInteraction = false;
                textDisplayed = false;
                interating = false;
                choicing = false;
                isEvent = false;
            }
        }
    }

    void LaboratoryGameOver()
    {
        trapSound.Play(); // 사운드 재생

        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(targetTag); // 태그로부터 모든 오브젝트 찾기

        foreach(GameObject obj in objectsWithTag)
        {
            // Gore 오브젝트의 위치, 회전, 부모 등의 정보를 저장
            Vector3 position = obj.transform.position;

            // 대체할 프리팹으로 새로운 오브젝트 생성
            GameObject newObject = Instantiate(laboratoryTrap, position, Quaternion.identity);

            // 플레이어를 바라보도록 설정
            newObject.transform.LookAt(transform.position);

            newObject.transform.eulerAngles = new Vector3(0f, newObject.transform.eulerAngles.y, newObject.transform.eulerAngles.z);

            // Gore 태그 오브젝트 삭제
            Destroy(obj);
        }
    }

    void UpdateTimer()
    {
        // 현재 시간을 1초 감소시킴
        currentTime -= 1f;

        // 시간이 0보다 작거나 같으면 게임 종료
        if (currentTime <= 0f)
        {
            currentTime = 0f;
            CancelInvoke("UpdateTimer");
            lastTimer.SetActive(false);
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        // UI에 현재 시간을 표시
        int min = Mathf.FloorToInt(currentTime / 60f);
        int sec = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = string.Format("{0:00} : {1:00}", min, sec);
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

        padClearSound.Play(); // 사운드 재생
        startLeftDoor.SetTrigger("IsOpen");
        startRightDoor.SetTrigger("IsOpen");
        startDoorOpen = true;
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("GameOverScene");
    }

    IEnumerator DestinationMonster()
    {
        yield return new WaitForSeconds(2f);

        monster.SetDestination(playerPosition.position); // 몬스터가 플레이어를 향해 이동
    }

    IEnumerator DestinationTrashCanMonster()
    {
        yield return new WaitForSeconds(2f);

        trashcanMonster.SetDestination(playerPosition.position); // 몬스터가 플레이어를 향해 이동
    }
}
