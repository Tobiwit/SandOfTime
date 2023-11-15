using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BaseAttackContainer", menuName = "Attack System/new Container")]
public class BaseAttackContainer : ScriptableObject {
    public List<BaseAttack> attacks = new List<BaseAttack>();

    public BaseAttack GetBaseAttackByLevel(int level) {
        if (level <= attacks.Count && level > 0) {
            return attacks[level - 1];
        } else if (level == 0) {
            return attacks[0];
        } else { return null; }
    }
}
