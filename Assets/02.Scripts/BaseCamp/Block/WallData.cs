using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WallData", menuName = "ScriptableObjects/WallData", order = 1)]
public class WallData : ScriptableObject
{
    public enum MaterialType
    {
        Wood = 0,
        Stone
    }

    public MaterialType wallType;
    public float width = 3f;
    public float height = 3f;
    public float depth = 0.2f;
    public Material wallMaterial;
    public GameObject wallPrefab;
}