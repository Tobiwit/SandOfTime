using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsSpecialFunctionManager : MonoBehaviour
{
    public static CardsSpecialFunctionManager Instance;

    private void Awake() {
        Instance = this;
    }


    public void computeSpecialFunction(string cardName, BaseUnit target, int amount, BaseUnit activeUnit) {
        print("computed Special Function");
        switch (cardName) {
            case "01501":
                target.AppliedEffects.Clear();
                break;
            case "00301":
                if (target.AppliedEffects.Count == 0) { return; }
                List<EffectType> keyList = new List<EffectType>(target.AppliedEffects.Keys);
                int rand = Random.Range(0,target.AppliedEffects.Count-1);
                target.AppliedEffects.Remove(keyList[rand]);
                break;
            case "00302":
                if (target.AppliedEffects.Count == 0) { return; }
                if (target.AppliedEffects.Count == 1) { target.AppliedEffects.Clear(); }
                for (int i = 0; i < 2; i++) {
                    List<EffectType> keyList3 = new List<EffectType>(target.AppliedEffects.Keys);
                    int rand3 = Random.Range(0, target.AppliedEffects.Count - 1);
                    target.AppliedEffects.Remove(keyList3[rand3]);
                }
                break;
            case "00303":
                if (target.AppliedEffects.Count == 0) { return; }
                if (target.AppliedEffects.Count <= 2) { target.AppliedEffects.Clear(); }
                for (int i = 0; i < 3; i++) {
                    List<EffectType> keyList3 = new List<EffectType>(target.AppliedEffects.Keys);
                    int rand3 = Random.Range(0, target.AppliedEffects.Count - 1);
                    target.AppliedEffects.Remove(keyList3[rand3]);
                }
                break;
            case "90101":
                activeUnit.Heal(amount);
                break;
            default:
                break;
        }
    }
}
