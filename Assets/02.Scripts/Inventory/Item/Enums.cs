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
    FirstDoorKey,   // 첫번째 문 열쇠
    MasterKey,      // 마스터키

    Knife,          // 칼
    Pick,           // 곡갱이
    Axe,            // 도끼
    WoodenPlanks,   // 나무판자
    IronPlanks,     // 철판


    // Stage2
    Brain,          // 뇌
    Entrails,       // 내장
    GasMask,        // 가스마스크
    Syrings,        // 주사기
    FakeKey,        // 가짜열쇠
    noteHint,       // 쪽지 힌트
    noteCaution,    // 주의문
    Book,           // 책
    Hammer,         // 망치
    Crowbar,        // 쇠지렛대

    // Stage3
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

public struct ItemContainList
{
    public ItemCode itemCode;
    public List<ItemContain> containList;
    public int itemCount;
}