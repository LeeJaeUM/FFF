using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;

public class PlayerGimicStage3 : MonoBehaviour
{
    RaycastHit hit;

    [SerializeField]
    private GameObject interactionUI; // 상호작용이 가능한지 나타내는 UI 오브젝트
    [SerializeField]
    private TextMeshProUGUI textField; // 게임의 진행을 위한 Text
    [SerializeField]
    private TextMeshProUGUI choiceText1Field; // 선택지 1
    [SerializeField]
    private TextMeshProUGUI choiceText2Field; // 선택지 2
    [SerializeField]
    private GameObject player; // 플레이어 캐릭터
    [SerializeField]
    private Animator restroomDoorAnimator;

    [SerializeField]
    private GameObject trap; // Trap 오브젝트
    [SerializeField]
    private GameObject pictureFrame; // PictureFrame 오브젝트

    [SerializeField]
    private GameObject[] diaryUI; // Diary UI 오브젝트
    [SerializeField]
    private GameObject[] researchLog; // Page UI 오브젝트
    private int currentPageIndex = 0;

    private float maxDistance = 2f; // Raycast 최대 범위

    private bool isInteraction = false; // 플레이어의 상호작용 여부
    private bool textDisplayed = false; // 상호작용 중의 텍스트 중복 방지 여부
    private bool interating = false; // 플레이어가 현재 상호작용 중인지 아닌지 여부
    private bool choicing = false; // 선택지가 떴을 때 여부
    private bool restroomOpen = false; // RestRoomDoor의 열림 여부

    private bool isEvent = false; // 이벤트 발생 동안

    private bool isGettingRemoteControl; // 인벤토리에 RemoteControl이 있을 경우

    private void Start()
    {
        
    }

    private void Update()
    {
        if(Physics.Raycast(transform.position, transform.forward ,out hit, maxDistance))
        {
            // PictureFrame 상호작용
            if(hit.collider.CompareTag("PICTUREFRAME"))
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
                        player.SetActive(true); // 상호작용 종료
                        isInteraction = false;
                        textDisplayed = false;
                        interating = false;
                        choicing = false;
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
                if(!isGettingRemoteControl)
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
    }

    // 플레이어는 캐릭터 조종 불가 및 카메라 회전 불가
    void StopControl()
    {
        if(player!= null)
        {
            player.SetActive(false);
        }
    }
}
