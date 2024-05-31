using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Device;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    AudioSource backgroundSound;

    [SerializeField]
    private GameObject fakeTitle; // 처음에 나오는 페이크 타이틀
    [SerializeField]
    private GameObject realTitle; // 나중에 나오는 진짜 타이틀창
    [SerializeField]
    private GameObject blackImage; // 암전 효과를 위한 어두운 이미지 창
    [SerializeField]
    private Transform titleText; // 타이틀 UI의 타이틀 텍스트
    [SerializeField]
    private GameObject buttonUI; // 버튼 UI;
    //[SerializeField]
    //private GameObject optionUI; // 옵션 UI;
    [SerializeField]
    private GameObject exitCaution; // exit 버튼을 눌렀을 때 뜨는 UI창
    [SerializeField]
    private GameObject stageSelectUI; // 스테이지 선택 UI;

    [SerializeField]
    private Texture[] randomImage; // 타이틀 랜덤 이미지
    [SerializeField]
    private RawImage titleImageUI; // 타이틀 이미지

    private bool isRealTitleOpen = false; // 타이틀이 변경되었는지에 대한 유무
    private bool isScreenFlickering = false; // 화면 깜빡임 여부

    private float screenTransitionTime = 5f; // 화면 전환 시간
    private float screenFlickeringStartTime = 2f; // 화면 깜빡임 시작 시간
    private float screenFlickeringTime = 1f; // 화면 깜빡임 전환

    private void Start()
    {
        backgroundSound = GetComponent<AudioSource>();
        StartCoroutine(ScreenTransition());
    }

    private void Update()
    {
        if (!isRealTitleOpen)
        {
            StartCoroutine(ScreenFlickerStart());
        }
    }

    IEnumerator ScreenTransition()
    {
        yield return new WaitForSeconds(screenTransitionTime);
        isRealTitleOpen = true;

        Destroy(fakeTitle);
        realTitle.SetActive(true);
        blackImage.SetActive(false);

        StartCoroutine(RealTitle());
    }

    IEnumerator ScreenFlickerStart()
    {
        yield return new WaitForSeconds(screenFlickeringStartTime);

        blackImage.SetActive(true);

        StartCoroutine(ScreenFlicking());
    }

    IEnumerator ScreenFlicking()
    {
        yield return new WaitForSeconds(screenFlickeringTime);

        blackImage.SetActive(false);

        screenFlickeringTime -= 0.1f;
    }

    IEnumerator RealTitle()
    {
        yield return new WaitForSeconds(2f);

        titleText.transform.position = new Vector2(600f, 800f);

        backgroundSound.Play(); // 오디오 재생

        // 디버그: 배열 길이를 로그로 출력하고 배열이 초기화되었는지 확인
        Debug.Log("randomImage 배열 길이: " + randomImage.Length);

        if (randomImage == null || randomImage.Length == 0)
        {
            Debug.LogError("randomImage 배열이 null이거나 비어 있습니다.");
            yield break;
        }

        // 타이틀 창에 이미지를 랜덤 배치
        int randomIndex = Random.Range(0, randomImage.Length);
        Debug.Log("랜덤 인덱스: " + randomIndex);

        if (randomImage[randomIndex] == null)
        {
            Debug.LogError("randomImage[" + randomIndex + "]이(가) null입니다.");
            yield break;
        }

        titleImageUI.texture = randomImage[randomIndex];

        // 디버그: 텍스처 변경 로그
        Debug.Log("titleImageUI 텍스처가 randomImage[" + randomIndex + "]로 설정되었습니다.");

        buttonUI.SetActive(true); // 버튼 UI 활성화
    }

    // GameStart버튼을 눌렀을 때 스테이지 선택 창 활성화
    public void GameStart()
    {
        stageSelectUI.SetActive(true);
    }

    // Option 버튼을 눌렀을 때 OptionUI 활성화
    //public void Option()
    //{
    //    optionUI.SetActive(true);
    //}

    // Exit 버튼을 눌렀을 때 CautionUI활성화
    public void Exit()
    {
        exitCaution.SetActive(true);
    }

    // ExitCaution에서 Exit를 눌렀을 경우
    public void RealExit()
    {
        UnityEngine.Application.Quit();
    }

    // ExitCaution에서 No를 눌렀을 경우
    public void No()
    {
        exitCaution.SetActive(false);
    }

    // Stage1 버튼을 눌렀을 때 씬 이동
    public void Stage1()
    {
        SceneManager.LoadScene("Stage1_Final");
    }

    // Stage2 버튼을 눌렀을 때 씬 이동
    public void Stage2()
    {
        SceneManager.LoadScene("Test_01");
    }

    // Stage3 버튼을 눌렀을 때 씬 이동
    public void Stage3()
    {
        SceneManager.LoadScene("Stage3");
    }

    // 로비 버튼을 눌렀을 때
    public void StageSelectLobby()
    {
        stageSelectUI.SetActive(false);
    }
}
