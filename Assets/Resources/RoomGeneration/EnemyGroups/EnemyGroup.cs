using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New EnemyGroup", menuName = "Room Generation/new EnemyGroup")]
public class EnemyGroup : ScriptableObject
{
    public List<BaseUnit> enemyList = new List<BaseUnit>();
    public int difficulty = 0;
    public bool isBossfight;
}
