using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoundationData", menuName = "ScriptableObjects/FoundationData", order = 1)]
public class FoundationData : ScriptableObject
{
    public enum MaterialType
    {
        Wood = 0,
        Stone
    }

    public MaterialType wallType;
    public float width = 2.0f;
    public float height = 2.0f;
    public float depth = 2.0f;
    public Material foundationMaterial;
    public GameObject foundationPrefab;
}
