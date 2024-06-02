using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureGameObject : MonoBehaviour, IInteractable
{
    public RawImage captureImage; // 캡처 이미지를 표시할 RawImage UI
    public Image borderImage; // 테두리를 표시할 Image UI
    public float captureDisplayDuration = 2.0f; // 캡처 이미지 표시 시간
    public int downscaleFactor = 10; // 이미지 축소 비율
    public int captureScaleFactor = 10; // 이미지 확대 비율
    public int captureWidth = 100; // 캡처할 이미지의 가로 크기
    public int captureHeight = 100; // 캡처할 이미지의 세로 크기
    public Color borderColor = Color.red; // 테두리 색상

    private Texture2D capturedTexture;
    private bool isCapturing = false;

    void Start()
    {
        captureImage.gameObject.SetActive(false);
        borderImage.gameObject.SetActive(false);
        borderImage.color = borderColor; // 테두리 색상 설정
    }

    public virtual void Interact()
    {
        CaptureScreen();
    }

    void Update()
    {

    }

    void CaptureScreen()
    {
        if (!isCapturing)
        {
            StartCoroutine(CaptureAndDisplayCoroutine());
        }
    }

    IEnumerator CaptureAndDisplayCoroutine()
    {
        isCapturing = true;

        yield return new WaitForEndOfFrame();

        // 화면 캡처
        int scaledWidth = Screen.width / downscaleFactor;
        int scaledHeight = Screen.height / downscaleFactor;
        capturedTexture = new Texture2D(scaledWidth, scaledHeight, TextureFormat.RGB24, false);
        capturedTexture.ReadPixels(new Rect(0, 0, scaledWidth, scaledHeight), 0, 0);
        capturedTexture.Apply();

        // 중앙 부분을 캡처하여 새로운 Texture2D에 저장
        int startX = (scaledWidth - captureWidth) / 2;
        int startY = (scaledHeight - captureHeight) / 2;
        Color[] pixels = capturedTexture.GetPixels(startX, startY, captureWidth, captureHeight);
        Texture2D newTexture = new Texture2D(captureWidth, captureHeight);
        newTexture.SetPixels(pixels);
        newTexture.Apply();

        // 화면에 캡처 이미지를 직접 렌더링
        RenderTexture renderTexture = new RenderTexture(captureWidth * captureScaleFactor, captureHeight * captureScaleFactor, 0);
        Graphics.Blit(newTexture, renderTexture);

        /*테두리 이미지 크기 설정 및 활성화
        borderImage.rectTransform.sizeDelta = new Vector2(captureWidth * captureScaleFactor, captureHeight * captureScaleFactor);
        borderImage.gameObject.SetActive(true);*/

        // RawImage에 렌더링된 이미지를 설정하여 화면에 표시
        captureImage.texture = renderTexture;
        captureImage.gameObject.SetActive(true);

        

        // 캡처 이미지 표시 시간 대기
        yield return new WaitForSeconds(captureDisplayDuration);

        // 캡처 이미지 및 테두리 비활성화
        captureImage.gameObject.SetActive(false);
        borderImage.gameObject.SetActive(false);

        isCapturing = false;
    }
}