using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnviromentData", menuName = "ScriptableObjects/EnviromentData", order = 2)]
public class EnviromentData : ScriptableObject
{
    public enum MaterialType
    {
        Wood = 0,
        Stone
    }
    public MaterialType materialType;
    public GameObject enviroPrefab;
}