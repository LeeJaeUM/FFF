using System;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCode
{
    // Stage1
    Wood = 0,       // 나무
    Ironstone,      // 철광석
    Matches,        // 성냥
    Magazine,       // 잡지
    CanFood,        // 캔
    Gun,            // 총
    Dynamite,       // 다이너마이트
    FirstDoorKey,   // FirstKeyDoor를 여는 키   //인벤토리에서 int값을 고정으로 쓰고 있다면 위치를 변경해야함
    MasterKey,      // 마스터키

    Knife,          // 나이프            //인벤토리에서 int값을 고정으로 쓰고 있다면 위치를 변경해야함 
    Pick,           // 곡갱이
    Axe,            // 도끼
    WoodenPlanks,   // 나무판자
    IronPlanks,     // 철판

    // Stage2
    Brain = 14,     // 뇌
    Entrails,       // 내장
    GasMask,        // 가스마스크
    GoldHint_1,     // 힌트쪽지1
    GoldHint_2,     // 힌트쪽지2
    OldScissors,    // 낡은 가위
    Key,            // 열쇠
    FakeKey,        // 가짜열쇠
    Syringe,        // 주사기
    OldSyringe,     // 낡은 주사기
    Book_6,         // 책_6
    Book_29,        // 책_29
    Room3Hint,      // 3번째 방 힌트
    Room4Caution,   // 4번째 방 주의문
    Hammer,         // 망치
    Crowbar,        // 쇠지렛대
    BloodyKnife,
    Hint,
    SecondKey,
    ThirdKey,
    OilSyringe,     // 기름이 들어간 주사기

    Bucket,         // 기름통(오일없는)  //인벤토리에서 int값을 고정으로 쓰고 있다면 위치를 변경해야함
    OilBucket,      // 오일(기름통)      //인벤토리에서 int값을 고정으로 쓰고 있다면 위치를 변경해야함
    OldPick,        // 녹슨 곡괭이       //인벤토리에서 int값을 고정으로 쓰고 있다면 위치를 변경해야함 

}


public struct SlotColorHighlights
{
    public static Color Green
    { get { return new Color32(127, 223, 127, 255); } }
    public static Color Yellow
    { get { return new Color32(223, 223, 63, 255); } }
    public static Color Red
    { get { return new Color32(223, 127, 127, 255); } }
    public static Color Blue
    { get { return new Color32(159, 159, 223, 255); } }
    public static Color Blue2
    { get { return new Color32(191, 191, 223, 255); } }
    public static Color White
    { get { return new Color32(255, 255, 255, 255); } }
    public static Color Blue3
    {
        get { return new Color32(100, 160, 255, 255); }
    }
}

[Serializable]
public struct ItemContainList
{
    public ItemCode itemCode;
    public List<ItemContain> containList;
    public int itemCount;
    public float itemWeight;
}