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

    public MaterialType materialType;
    //public float width = 3.0f;
   // public float height = 2.0f;
   // public float depth = 3.0f;
    public Material foundationMaterial;
    public GameObject foundationPrefab;
}
