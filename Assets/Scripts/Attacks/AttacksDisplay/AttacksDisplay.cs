using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName ="Attack System/new Inventory")]
public class AttacksDisplay : ScriptableObject
{
    public List<BaseAttack> Container = new List<BaseAttack>();

    public void AddAttack(BaseAttack _attack)
    {
            Container.Add(_attack);
    }
}
