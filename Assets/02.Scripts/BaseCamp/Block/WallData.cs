using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WallData", menuName = "ScriptableObjects/WallData", order = 1)]
public class WallData : ScriptableObject
{
    public enum WallType
    {
        Wood = 0,
        Stone
    }

    public WallType wallType;
    public float width = 2f;
    public float height = 2f;
    public Material wallMaterial;
}