using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStage3MiniGame : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro caution2Text; // caution2text의 UI시스템

    private bool caution2_1 = false; // caution2의 텍스트가 빨강,red일 경우
    private bool caution2_2 = false; // caution2의 텍스트가 빨강,black일 경우
    private bool caution2_3 = false; // caution2의 텍스트가 검정,red일 경우
    private bool caution2_4 = false; // caution2의 텍스트가 검정,black일 경우

    // Start is called before the first frame update
    void Start()
    {
        // Caution2Text를 랜덤으로 '검정' 또는 '빨강' 중 하나를 표기한다.
        string[] texts = { "빨강", "검정" };
        string randomText = texts[Random.Range(0, texts.Length)];

        // Caution2Text의 색을 랜덤으로 '검정' 또는 '빨강'으로 표시한다.
        Color[] colors = { Color.red, Color.black };
        Color randomColor = colors[Random.Range(0, colors.Length)];

        // 텍스트 UI에 랜덤으로 선택된 텍스트를 표시한다.
        caution2Text.text = randomText;
        caution2Text.color = randomColor;

        // 출력된 텍스트 및 색에 따라 지나야하는 블럭이 정해짐
        if (caution2Text.text == "빨강" && caution2Text.color == Color.red)
            caution2_1 = true;

        else if (caution2Text.text == "빨강" && caution2Text.color == Color.black)
            caution2_2 = true;

        else if (caution2Text.text == "검정" && caution2Text.color == Color.red)
            caution2_3 = true;

        else if (caution2Text.text == "검정" && caution2Text.color == Color.black)
            caution2_4 = true;
    }

    // Storage2의 미니게임
    private void OnTriggerEnter(Collider other)
    {
        // 빨강이 아닌 다른 블록을 밟았을 경우 게임 오버
        if (caution2_1)
        {
            if (other.CompareTag("BLACKBLOCK"))
                SceneManager.LoadScene("GameOverScene");
            else if (other.CompareTag("WHITEBLOCK"))
                SceneManager.LoadScene("GameOverScene");
        }

        // 검정이 아닌 다른 블록을 밟았을 경우
        else if (caution2_2)
        {
            if (other.CompareTag("REDBLOCK"))
                SceneManager.LoadScene("GameOverScene");
            else if (other.CompareTag("WHITEBLOCK"))
                SceneManager.LoadScene("GameOverScene");
        }

        // 검정이 아닌 다른 블록을 밟았을 경우
        else if (caution2_3)
        {
            if (other.CompareTag("REDBLOCK"))
                SceneManager.LoadScene("GameOverScene");
            else if (other.CompareTag("WHITEBLOCK"))
                SceneManager.LoadScene("GameOverScene");
        }

        // 빨강이 아닌 다른 블록을 밟았을 경우
        else if (caution2_4)
        {
            if (other.CompareTag("BLACKBLOCK"))
                SceneManager.LoadScene("GameOverScene");
            else if (other.CompareTag("WHITEBLOCK"))
                SceneManager.LoadScene("GameOverScene");
        }

        // 몬스터와 충돌했을 경우
        if(other.CompareTag("MONSTER"))
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
