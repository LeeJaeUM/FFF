using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoFrame : MonoBehaviour, IInteractable
{
    public GameObject secondKey;
    public InventoryUI inventory;
    private string announsText = string.Empty;
    public int itemcode = 0;
    enum Frametype
    {
        None = 0,
        crowbar
    }
    [SerializeField] private Frametype frametype = Frametype.None;
    private void Awake()
    {
        inventory = FindAnyObjectByType<InventoryUI>();
        secondKey.SetActive(false);
    }
    protected virtual void Start()
    {
        switch (frametype)
        {
            case Frametype.crowbar:
                announsText = ("뜯어낼 수 있을 것 같다");
                itemcode = (int)ItemCode.Crowbar;
            break;
        }
    }
    public void Interact()
    {
        if (inventory.UseItemCheck((ItemCode)itemcode))
        {
            // CrowBar가 있을 때 상호작용
            OpenFrame();
        }
        else
        {
            // CrowBar가 없을 때 메시지 표시
            Stage1Manager.Instance.BottomTMPText = ("손으로는 열 수 없다. 땔 수 있는 장비가 필요하다.");
        }
    }

    private void OpenFrame()
    {
        // 사진액자 열기
        Stage1Manager.Instance.BottomTMPText = ("액자가 열렸습니다.");
        // PhotoFrame 오브젝트 비활성화
        gameObject.SetActive(false);
        // SecondKey 오브젝트 활성화
        secondKey.SetActive(true);
    }
}
