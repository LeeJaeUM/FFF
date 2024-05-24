using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private Transform skyLight;
    [SerializeField]
    private NavMeshAgent monsterAI;
    [SerializeField]
    private Transform player;

    private float totalTime = 5;
    private float nowTime = 0;

    private bool isMonsterMove = false;

    //public InventroyManager inventory;

    public InventoryUI inven;

    public ButtonManager buttonManager;

    protected override void Awake()
    {
        base.Awake();

        inven = FindObjectOfType<InventoryUI>();
        buttonManager = FindObjectOfType<ButtonManager>();
    }

    private void Update()
    {
        nowTime += Time.deltaTime;

        if(nowTime >= totalTime)
        {
            RotateSkyLight();
            isMonsterMove = !isMonsterMove;
            nowTime = 0;
        }

        if (isMonsterMove == true && player != null)
        {
            //monsterAI.SetDestination(player.position);
        }
        else
        {
            //monsterAI.ResetPath();
        }
    }

    void RotateSkyLight()
    {
        if(skyLight != null)
        {
            skyLight.transform.Rotate(180f, 0, 0);
        }
    }
}
