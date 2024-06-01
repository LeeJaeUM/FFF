using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureGameObject : MonoBehaviour, IInteractable
{
    public RawImage captureImage; // 캡처 이미지를 표시할 RawImage UI
    public float captureDisplayDuration = 2.0f; // 캡처 이미지 표시 시간
    public int downscaleFactor = 1; // 이미지 축소 비율
    public int captureScaleFactor = 50; // 이미지 확대 비율
    public int captureWidth = 300; // 캡처할 이미지의 가로 크기
    public int captureHeight = 300; // 캡처할 이미지의 세로 크기

    private Texture2D capturedTexture;
    private bool isCapturing = false;

    void Start()
    {
        captureImage.gameObject.SetActive(false);
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

        // RawImage에 렌더링된 이미지를 설정하여 화면에 표시
        captureImage.texture = renderTexture;
        captureImage.gameObject.SetActive(true);

        // 캡처 이미지 표시 시간 대기
        yield return new WaitForSeconds(captureDisplayDuration);

        // 캡처 이미지 비활성화
        captureImage.gameObject.SetActive(false);

        isCapturing = false;
    }

    void AddBorder(Texture2D texture, int borderSize, Color borderColor)
    {
        // 상단 테두리
        for (int y = texture.height - borderSize; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                texture.SetPixel(x, y, borderColor);
            }
        }
        // 하단 테두리
        for (int y = 0; y < borderSize; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                texture.SetPixel(x, y, borderColor);
            }
        }
        // 좌측 테두리
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < borderSize; x++)
            {
                texture.SetPixel(x, y, borderColor);
            }
        }
        // 우측 테두리
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = texture.width - borderSize; x < texture.width; x++)
            {
                texture.SetPixel(x, y, borderColor);
            }
        }

        texture.Apply();
    }
}
//AddBorder(enlargedTexture, borderSize, Color.red);