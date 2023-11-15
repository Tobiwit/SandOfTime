using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New ContainerInventory", menuName = "Attack System/new ContainerInventory")]
public class AttacksContainerDisplay : ScriptableObject {
    public List<BaseAttackContainer> preComputeList = new List<BaseAttackContainer>();
    //public Dictionary<BaseAttackContainer, int> Container = new Dictionary<BaseAttackContainer, int>();
    public List<KeyValuePair<BaseAttackContainer, int>> Container = new List<KeyValuePair<BaseAttackContainer, int>>();

    public void AddAttack(BaseAttackContainer _attackContainer,int _level) {
        Container.Add(new KeyValuePair<BaseAttackContainer, int>(_attackContainer,_level));
    }

    public void ComputeListIntoDictionary() {
        Container.Clear();
        foreach(BaseAttackContainer container in preComputeList) {
            Container.Add(new KeyValuePair<BaseAttackContainer, int>(container, 1));
        }
    }
}
