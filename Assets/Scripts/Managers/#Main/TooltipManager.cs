using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    public Tooltip tooltip;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public static void Show(string content, string header = "") {
        Instance.tooltip.SetText(content, header);
        Instance.tooltip.gameObject.SetActive(true);
    }

    public static void Hide() {
        Instance.tooltip.gameObject.SetActive(false);
    }

    public string GetTooltipContent(EffectType effectType) {
        switch (effectType) {
            case EffectType.Any:
                return "???";
            case EffectType.Survival:
                return "If Survival is triggered the Player would have been killed by the last attack. The next time the Player receives Damage he will be killed. You can remove this Effect by Healing at least 1 Health.";
            case EffectType.Poisioned:
                return "Poisioned Creatures will lose Health equivalent to their poisioned Amount at the Beginning of their turn. Poisioned will decrease by 1 every turn.";
            case EffectType.Burning:
                return "Burning Creatures will lose Health equivalent to their Burning Amount at the End of their turn. Burning will decrease by 1 every turn.";
            case EffectType.Strength:
                return "Creatures with the Strength effect will deal 25% more damage than normal.";
            case EffectType.Hasted:
                return "Hasted Creatures will start their turn with 1 additional Energy.";
            case EffectType.Invisible:
                return "Invisible Creatures can not be targeted directly by a hostile Creature, but will still be hit if affected by an area attack.";
            case EffectType.Stunned:
                return "Stunned Creatures have their Energy set to 0 and therefore can not use any Action this turn.";
            case EffectType.Constrained:
                return "Constrained Creatures can not move on their turn.";
            case EffectType.Weakened:
                return "Creatures with the Strength effect will deal 25% less damage than normal.";
            case EffectType.Enraged:
                return "Enraged Creatures will deal 25% more damage but lose 1 energy at the start of their turn.";
            case EffectType.Stressed:
                return "Stressed Creatures will lose 1 energy at the start of their turn.";
            case EffectType.Evasion:
                return "Creatures with the Evasion effect will be damaged or effected by debuffs from any area attack";
            case EffectType.Taunt:
                return "Creatures with the Taunt effect will make Opponents try to hit you.";
            case EffectType.Adrenaline:
                return "Creatures with the Adrenaline effect will instantly heal 1 HP after falling to 0 Health. Adrenaline will wear off after one round. When Adrenaline wears off a Creature is affected by Weakened and Stressed.";
            case EffectType.Swiftness:
                return "Creatures with the Swiftness effect will have one free move per Swiftness Amount.";
            case EffectType.CritUp:
                return "Creatures with the CritUp effect will crit on their next attack.";
            case EffectType.Shocked:
                return "Shocked Creatures have a 25% per Stack of Shocked Amount to miss their attack. Only stacks up to 4.";
            default:
                return "Error: Couldn't find Effect Content";
        }



    }
}