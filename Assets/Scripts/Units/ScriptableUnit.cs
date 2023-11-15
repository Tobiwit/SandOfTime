using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnit", menuName = "Custom Objects/Scriptable Unit")]

public class ScriptableUnit : ScriptableObject
{
    public Faction Faction;
    public BaseUnit UnitPrefab;
    public bool isObject;
}

public enum Faction
{
    Hero = 0,
    Enemy = 1,
    Object = 2
}
