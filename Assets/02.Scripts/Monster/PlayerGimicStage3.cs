using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerGimicStage3 : MonoBehaviour
{
    RaycastHit hit; // 메인 카메라의 레이 캐스트

    [SerializeField]
    private GameObject interactionUI; // 상호작용이 가능한지 나타내는 UI 오브젝트
    [SerializeField]
    private TextMeshProUGUI textField; // 게임의 진행을 위한 Text
    [SerializeField]
    private GameObject player; // 플레이어 캐릭터
    [SerializeField]
    private GameObject pictureFrameButton; // PictureFrame의 선택지
    [SerializeField]
    private GameObject trap; // Trap 오브젝트
    [SerializeField]
    private GameObject pictureFrame; // PictureFrame 오브젝트

    private int maxDistance = 2; // Raycast 최대 범위

    private bool isInteraction = false; // 플레이어의 상호작용 여부
    private bool textDisplayed = false; // 상호작용 중의 텍스트 중복 방지 여부
    private bool interating = false; // 플레이어가 현재 상호작용 중인지 아닌지 여부

    private void Start()
    {
        
    }

    private void Update()
    {
        if(Physics.Raycast(transform.position,transform.forward,out hit,maxDistance))
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
                    textField.gameObject.SetActive(true); // 텍스트 출력
                    textField.text = "꽤나 큰 액자다...";
                    textDisplayed = true;
                }

                // 상호작용 중(E키를 한번 더 누르면 선택지 출력)
                else if(Input.GetKeyDown(KeyCode.E) && textDisplayed)
                {
                    textField.text = "들춰낼까?";
                    pictureFrameButton.SetActive(true);
                }

                // 선택지가 출력되었을 때 가능한 버튼
                if(pictureFrameButton)
                {
                    // 1번을 누를 때
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        textField.text = " "; // 상호작용 종료
                        pictureFrameButton.SetActive(false); // 상호작용 종료
                        trap.SetActive(true); // 함정 생성
                        pictureFrame.SetActive(false); // 함정 생성
                        player.SetActive(true); // 상호작용 종료
                        interating = false; // 상호작용 종료
                        textDisplayed = false; // 상호작용 종료
                    }

                    // 2번을 누를 때
                    else if(Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        textField.text = " "; // 상호작용 종료
                        interating = false; // 상호작용 종료
                        textDisplayed = false; // 상호작용 종료
                        player.SetActive(true); // 상호작용 종료
                        pictureFrameButton.SetActive(false); // 상호작용 종료
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
