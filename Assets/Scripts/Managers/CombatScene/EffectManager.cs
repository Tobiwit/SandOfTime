using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum EffectState
{
    OnTurnStart,
    OnTurnEnd,
    OnAttacking,
    OnBeingAttacked,
    OnApply,
    OnApplySelf

}


public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;
    private int effectAmount;

    private void Awake()
    {
        Instance = this;
    }

    public void HandleEffects(EffectType effectType, EffectState effectState, BaseUnit unit, int effectAmount = 0)
    {
        this.effectAmount = effectAmount;
        switch (effectType)
        {
            case EffectType.Any:
                break;
            case EffectType.Poisioned:
                ExecutePoisioned(effectState, unit);
                break;
            case EffectType.Hasted:
                ExecuteHasted(effectState, unit);
                break;
            case EffectType.Burning:
                ExecuteBurning(effectState, unit);
                break;
            case EffectType.Strength:
                ExecuteStrength(effectState, unit);
                break;
            case EffectType.Invisible:
                ExecuteInvisible(effectState, unit);
                break;
            case EffectType.Survival:
                break;
            case EffectType.Stunned:
                ExecuteStunned(effectState, unit);
                break;
            case EffectType.Constrained:
                ExecuteConstrained(effectState, unit);
                break;
            case EffectType.Weakened:
                ExecuteWeakened(effectState, unit);
                break;
            case EffectType.Enraged:
                break;
            case EffectType.Stressed:
                ExecuteStressed(effectState, unit);
                break;
            case EffectType.Evasion:
                break;
            case EffectType.Taunt:
                ExecuteTaunt(effectState, unit);
                break;
            case EffectType.Adrenaline:
                ExecuteAdrenaline(effectState, unit);
                break;
            case EffectType.Swiftness:
                ExecuteSwiftness(effectState, unit);
                break;
            case EffectType.CritUp:
                ExecuteCritUp(effectState, unit);
                break;
            default:
                break;
        }
    }

    public void TriggerAllEffects(EffectState effectState, BaseUnit unit)
    {
        if (unit.AppliedEffects.Count > 0)
        {
            List<EffectType> keys = new List<EffectType>(unit.AppliedEffects.Keys);
            foreach (EffectType key in keys)
            {
                HandleEffects(key, effectState, unit);
            }
        }
        
        if (effectState == EffectState.OnTurnEnd)
        {
            foreach (KeyValuePair<EffectType,int> pair in unit.SelfAppliedEffects)
            {
                HandleEffects(pair.Key, EffectState.OnApply, unit, pair.Value);
            }
            unit.SelfAppliedEffects.Clear();
        }
    }

    private void ExecutePoisioned(EffectState effectState, BaseUnit unit)
    {
        EffectType type = EffectType.Poisioned;
        int FX = unit.AppliedEffects.ContainsKey(type) ? unit.AppliedEffects[type] : 0;
        switch (effectState)
        {
            case EffectState.OnTurnStart:
                if (FX > 0)
                {
                    unit.TakeDamage(FX);
                    FX--;
                }
                break;
            case EffectState.OnApply:
                FX += effectAmount;
                break;
            case EffectState.OnApplySelf:
                FX += effectAmount;
                break;
            default:
                break;

        }

        if (!(FX == 0))
        {
            unit.AppliedEffects[type] = FX;
        }
        else
        {
            unit.AppliedEffects.Remove(type);
        }
    }

    private void ExecuteHasted(EffectState effectState, BaseUnit unit)
    {
        EffectType type = EffectType.Hasted;
        int FX = unit.AppliedEffects.ContainsKey(type) ? unit.AppliedEffects[type] : 0;
        switch (effectState)
        {
            case EffectState.OnTurnStart:
                if (FX > 0)
                {
                    unit.MaxTurnEnergy++;
                }
                break;
            case EffectState.OnTurnEnd:
                if (FX > 0)
                {
                    unit.MaxTurnEnergy--;
                    FX--;
                }
                break;
            case EffectState.OnApply:
                FX += effectAmount;
                if (FX > 3)
                {
                    FX = 3;
                }
                break;
            case EffectState.OnApplySelf:
                unit.SelfAppliedEffects[type] = unit.SelfAppliedEffects.ContainsKey(type) ? unit.SelfAppliedEffects[type] + effectAmount : effectAmount;
                break;
            default:
                break;

        }

        if (!(FX == 0))
        {
            unit.AppliedEffects[type] = FX;
        }
        else
        {
            unit.AppliedEffects.Remove(type);
        }
    }

    private void ExecuteStressed(EffectState effectState, BaseUnit unit)
    {
        EffectType type = EffectType.Stressed;
        int FX = unit.AppliedEffects.ContainsKey(type) ? unit.AppliedEffects[type] : 0;
        switch (effectState)
        {
            case EffectState.OnTurnStart:
                if (FX > 0)
                {
                    unit.MaxTurnEnergy--;
                }
                break;
            case EffectState.OnTurnEnd:
                if (FX > 0)
                {
                    unit.MaxTurnEnergy++;
                    FX--;
                }
                break;
            case EffectState.OnApply:
                FX += effectAmount;
                if (FX > 3)
                {
                    FX = 3;
                }
                break;
            case EffectState.OnApplySelf:
                unit.SelfAppliedEffects[type] = unit.SelfAppliedEffects.ContainsKey(type) ? unit.SelfAppliedEffects[type] + effectAmount : effectAmount;
                break;
            default:
                break;

        }

        if (!(FX == 0))
        {
            unit.AppliedEffects[type] = FX;
        }
        else
        {
            unit.AppliedEffects.Remove(type);
        }
    }

    private void ExecuteInvisible(EffectState effectState, BaseUnit unit)
    {
        EffectType type = EffectType.Invisible;
        int FX = unit.AppliedEffects.ContainsKey(type) ? unit.AppliedEffects[type] : 0;
        switch (effectState)
        {
            case EffectState.OnAttacking:
                if (FX > 0)
                {
                    FX = 0;
                    unit.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                }
                break;
            case EffectState.OnTurnEnd:
                if (FX > 0)
                {
                    FX--;
                }
                if (FX == 0) {
                    unit.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                }
                break;
            case EffectState.OnBeingAttacked:
                if (FX > 0)
                {
                    FX = 0;
                    unit.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                }
                break;
            case EffectState.OnApply:
                FX += effectAmount;
                if (FX > 1)
                {
                    FX = 1;
                }
                unit.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 0.75f);
                break;
            case EffectState.OnApplySelf:
                FX += effectAmount+1;
                if (FX > 2)
                {
                    FX = 2;
                }
                unit.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 0.75f);
                break;
            default:
                break;
        }

        if (!(FX == 0))
        {
            unit.AppliedEffects[type] = FX;
        }
        else
        {
            unit.AppliedEffects.Remove(type);
        }
    }

    private void ExecuteStunned(EffectState effectState, BaseUnit unit)
    {
        EffectType type = EffectType.Stunned;
        int FX = unit.AppliedEffects.ContainsKey(type) ? unit.AppliedEffects[type] : 0;
        switch (effectState)
        {
            case EffectState.OnTurnStart:
                if (FX > 0)
                {
                    if (unit.Faction == Faction.Hero)
                        unit.RemoveEnergy(unit.GetTurnEnergy());
                }
                break;
            case EffectState.OnTurnEnd:

                if (FX > 0)
                {
                    FX--;
                }
                break;
            case EffectState.OnApply:
                FX += effectAmount;
                if (FX > 1)
                {
                    FX = 1;
                }
                break;
            case EffectState.OnApplySelf:
                unit.SelfAppliedEffects[type] = unit.SelfAppliedEffects.ContainsKey(type) ? unit.SelfAppliedEffects[type] + effectAmount : effectAmount;
                break;
        }

        if (!(FX == 0))
        {
            unit.AppliedEffects[type] = FX;
        }
        else
        {
            unit.AppliedEffects.Remove(type);
        }
    }

    private void ExecuteAdrenaline(EffectState effectState, BaseUnit unit)
    {
        EffectType type = EffectType.Adrenaline;
        int FX = unit.AppliedEffects.ContainsKey(type) ? unit.AppliedEffects[type] : 0;
        int FX_Weakened = unit.AppliedEffects.ContainsKey(EffectType.Weakened) ? unit.AppliedEffects[EffectType.Weakened] : 0;
        int FX_Stressed = unit.AppliedEffects.ContainsKey(EffectType.Stressed) ? unit.AppliedEffects[EffectType.Stressed] : 0;
        int FX_Survival = unit.AppliedEffects.ContainsKey(EffectType.Survival) ? unit.AppliedEffects[EffectType.Survival] : 0;
        switch (effectState)
        {
            case EffectState.OnTurnStart:
                if (FX > 0)
                {
                    FX_Weakened++;
                    FX_Stressed++;
                    unit.MaxTurnEnergy--;
                    FX--;
                }
                break;
            case EffectState.OnBeingAttacked:
                if (FX_Survival > 0)
                {
                    if (FX > 0)
                    {
                        unit.Heal(1);
                        FX_Weakened++;
                        FX_Stressed++;
                        FX--;
                    }
                }
                break;
            case EffectState.OnApply:
                if (FX_Survival > 0)
                {
                    unit.Heal(1);
                    FX_Weakened++;
                    FX_Stressed++;
                    FX--;
                } else
                {
                    if (FX < 1)
                    {
                        FX++;
                    }
                }
                break;
            case EffectState.OnApplySelf:
                if (FX_Survival > 0)
                {
                    unit.Heal(1);
                    FX_Weakened++;
                    FX_Stressed++;
                    FX--;
                }
                else
                {
                    if (FX < 1)
                    {
                        FX++;
                    }
                }
                break;
        }


        if (!(FX == 0))
        {
            unit.AppliedEffects[type] = FX;
        } else
        {
            unit.AppliedEffects.Remove(type);
        }
        if (!(FX_Weakened == 0))
        {
            unit.AppliedEffects[EffectType.Weakened] = FX_Weakened;
        }
        else
        {
            unit.AppliedEffects.Remove(EffectType.Weakened);
        }
        if (!(FX_Stressed == 0))
        {
            unit.AppliedEffects[EffectType.Stressed] = FX_Stressed;
        }
        else
        {
            unit.AppliedEffects.Remove(EffectType.Stressed);
        }
        if (!(FX_Survival == 0))
        {
            unit.AppliedEffects[EffectType.Survival] = FX_Survival;
        }
        else
        {
            unit.AppliedEffects.Remove(EffectType.Survival);
        }
    }

    private void ExecuteBurning(EffectState effectState, BaseUnit unit)
    {
        EffectType type = EffectType.Burning;
        int FX = unit.AppliedEffects.ContainsKey(type) ? unit.AppliedEffects[type] : 0;
        switch (effectState)
        {
            case EffectState.OnTurnEnd:
                if (FX > 0)
                {
                    unit.TakeDamage(FX);
                    FX--;
                }
                break;
            case EffectState.OnApply:
                FX += effectAmount;
                break;
            case EffectState.OnApplySelf:
                unit.SelfAppliedEffects[type] = unit.SelfAppliedEffects.ContainsKey(type) ? unit.SelfAppliedEffects[type] + effectAmount : effectAmount;
                break;
            default:
                break;

        }

        if (!(FX == 0))
        {
            unit.AppliedEffects[type] = FX;
        }
        else
        {
            unit.AppliedEffects.Remove(type);
        }
    }

    private void ExecuteWeakened(EffectState effectState, BaseUnit unit)
    {
        EffectType type = EffectType.Weakened;
        int FX = unit.AppliedEffects.ContainsKey(type) ? unit.AppliedEffects[type] : 0;
        switch (effectState)
        {
            case EffectState.OnTurnEnd:
                if (FX > 0)
                {
                    FX--;
                }
                break;
            case EffectState.OnApply:
                FX += effectAmount;
                break;
            case EffectState.OnApplySelf:
                unit.SelfAppliedEffects[type] = unit.SelfAppliedEffects.ContainsKey(type) ? unit.SelfAppliedEffects[type] + effectAmount : effectAmount;
                break;
            default:
                break;

        }

        if (!(FX == 0))
        {
            unit.AppliedEffects[type] = FX;
        }
        else
        {
            unit.AppliedEffects.Remove(type);
        }
    }

    private void ExecuteStrength(EffectState effectState, BaseUnit unit)
    {
        EffectType type = EffectType.Strength;
        int FX = unit.AppliedEffects.ContainsKey(type) ? unit.AppliedEffects[type] : 0;
        switch (effectState)
        {
            case EffectState.OnTurnEnd:
                if (FX > 0)
                {
                    FX--;
                }
                break;
            case EffectState.OnApply:
                FX += effectAmount;
                break;
            case EffectState.OnApplySelf:
                unit.SelfAppliedEffects[type] = unit.SelfAppliedEffects.ContainsKey(type) ? unit.SelfAppliedEffects[type] + effectAmount : effectAmount;
                break;
            default:
                break;

        }

        if (!(FX == 0))
        {
            unit.AppliedEffects[type] = FX;
        }
        else
        {
            unit.AppliedEffects.Remove(type);
        }
    }

    private void ExecuteSwiftness(EffectState effectState, BaseUnit unit)
    {
        EffectType type = EffectType.Swiftness;
        int FX = unit.AppliedEffects.ContainsKey(type) ? unit.AppliedEffects[type] : 0;
        switch (effectState)
        {
            case EffectState.OnTurnEnd:
                if (FX > 0)
                {
                    FX = 0;
                }
                break;
            case EffectState.OnApply:
                FX += effectAmount;
                if (FX > 3)
                {
                    FX = 3;
                }
                break;
            case EffectState.OnApplySelf:
                unit.SelfAppliedEffects[type] = unit.SelfAppliedEffects.ContainsKey(type) ? unit.SelfAppliedEffects[type] + effectAmount : effectAmount;
                break;
            default:
                break;

        }

        if (!(FX == 0))
        {
            unit.AppliedEffects[type] = FX;
        }
        else
        {
            unit.AppliedEffects.Remove(type);
        }
    }

    private void ExecuteConstrained(EffectState effectState, BaseUnit unit)
    {
        EffectType type = EffectType.Constrained;
        int FX = unit.AppliedEffects.ContainsKey(type) ? unit.AppliedEffects[type] : 0;
        switch (effectState)
        {
            case EffectState.OnTurnEnd:
                if (FX > 0)
                {
                    FX--;
                }
                break;
            case EffectState.OnApply:
                FX += effectAmount;
                if (FX > 1)
                {
                    FX = 1;
                }
                break;
            case EffectState.OnApplySelf:
                unit.SelfAppliedEffects[type] = unit.SelfAppliedEffects.ContainsKey(type) ? unit.SelfAppliedEffects[type] + effectAmount : effectAmount;
                break;
        }

        if (!(FX == 0))
        {
            unit.AppliedEffects[type] = FX;
        }
        else
        {
            unit.AppliedEffects.Remove(type);
        }
    }

    private void ExecuteTaunt(EffectState effectState, BaseUnit unit)
    {
        EffectType type = EffectType.Taunt;
        int FX = unit.AppliedEffects.ContainsKey(type) ? unit.AppliedEffects[type] : 0;
        switch (effectState)
        {
            case EffectState.OnTurnStart:
                if (FX > 0)
                {
                    FX--;
                }
                break;
            case EffectState.OnBeingAttacked:
                if (FX > 0)
                {
                    FX--;
                }
                break;
            case EffectState.OnApply:
                FX += effectAmount;
                if (FX > 1)
                {
                    FX = 1;
                }
                break;
            case EffectState.OnApplySelf:
                FX += effectAmount;
                if (FX > 1)
                {
                    FX = 1;
                }
                break;
        }

        if (!(FX == 0))
        {
            unit.AppliedEffects[type] = FX;
        }
        else
        {
            unit.AppliedEffects.Remove(type);
        }
    }


    private void ExecuteCritUp(EffectState effectState, BaseUnit unit) {
        EffectType type = EffectType.CritUp;
        int FX = unit.AppliedEffects.ContainsKey(type) ? unit.AppliedEffects[type] : 0;
        switch (effectState) {
            case EffectState.OnTurnStart:
                break;
            case EffectState.OnApply:
                FX += effectAmount;
                break;
            case EffectState.OnApplySelf:
                FX += effectAmount;
                break;
            default:
                break;

        }

        if (!(FX == 0)) {
            unit.AppliedEffects[type] = FX;
        } else {
            unit.AppliedEffects.Remove(type);
        }
    }




}