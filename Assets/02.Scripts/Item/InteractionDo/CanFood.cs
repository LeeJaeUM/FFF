using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanFood : MonoBehaviour, IInteractable
{
    InventoryUI inventoryUI;

    readonly ItemCode knife = ItemCode.Knife;
    readonly ItemCode canFood = ItemCode.CanFood;

    private void Awake()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>();
    }

    List<string> texts = new List<string>();
    Dictionary<string, int> healthRestoreMap = new Dictionary<string, int>();

    private void Start()
    {
        texts.Add("윽, 정말 최악이다.");               // 체력 1회복
        texts.Add("어떻게 이런 맛이 날 수 있지..?");    // 체력 2회복
        texts.Add("썩 좋은 맛은 아닌 것 같다.");        // 체력 2회복
        texts.Add("몸에 활기가 돈다.");                // 체력 3회복
        texts.Add("뭐든 할 수 있을 것 같다.");         // 체력 3회복

        healthRestoreMap.Add(texts[0], 1);
        healthRestoreMap.Add(texts[1], 2);
        healthRestoreMap.Add(texts[2], 2);
        healthRestoreMap.Add(texts[3], 3);
        healthRestoreMap.Add(texts[4], 3);
    }

    public void Interact()
    {
        // 아이템 사용 시 동작 구현
        // Knife가 인벤토리에 있는지 확인
        if (inventoryUI.UseItemCheck(knife))
        {
            // 특수 상호작용: 체력 회복 동작 구현
            RestoreHealth();
        }
        else
        {
            // 일반 상호작용: 메시지 표시
            Stage1Manager.Instance.BottomTMPText = "뜯을 만한 것이 필요하다.";
        }
        // 플레이어는 기본적으로 체력이 4로 되어있음.
        // 하루가 지날 때마다 체력이 1씩 줄어들고 0이 되면 게임오버.
        // 맵에는 총 3개의 CanFood가 있고 1개의 CanFood를 먹을 때마다 체력이 출력된 텍스트에 따라 정해진 양만큼 증가,
        // 탈출하지 않고 총 10일을 버틸 수 있음.
    }

    private void RestoreHealth()
    {
        if (inventoryUI.UseItemCheck(canFood))
        {
            inventoryUI.UseItem(ItemCode.CanFood, 1);
            string selectedText = GetRandomText();
            int healthRestore = healthRestoreMap[selectedText];
            Stage1Manager.Instance.BottomTMPText = selectedText;
            //player.RestoreHealth(healthRestore);
            Debug.Log($"체력이 {healthRestore} 회복되었습니다.");
        }
    }

    string GetRandomText()
    {
        int randomIndex = Random.Range(0, texts.Count);
        return texts[randomIndex];
    }
}
