using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockData", menuName = "ScriptableObjects/WallData", order = 1)]
public class BlockData : ScriptableObject
{
    public enum MaterialType
    {
        Wood = 0,
        Stone,
        Iron
    }


    public MaterialType materialType;
    public GameObject wallPrefab_Ho;
    public GameObject wallPrefab_Ve;
    public GameObject floorPrefab;
    public Material blockMaterial;
    //public float width = 3f;
    //public float height = 3f;
    //public float depth = 0.2f;
}