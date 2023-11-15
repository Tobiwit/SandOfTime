using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CuiManager : MonoBehaviour
{

    public GameObject damageTextPrefab, blockTextPrefab, healTextPrefab, critTextPrefab;

    public GameObject effectPrefab;
    public Sprite poisioned, hasted, stunned, stressed, critUp, invisible, adrenaline, constrained, survival, burning, weakened, enraged, swiftness, taunt, strength, evasion, shocked;

    public static CuiManager Instance;

    private void Awake()
    {
        Instance = this;
    }


    public void SpawnDamageText(BaseUnit unit, int damageAmount)
    {
        Vector3 randomVector = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0f);
        GameObject DamageTextInstance = Instantiate(damageTextPrefab, unit.transform);
        DamageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().text = damageAmount.ToString();
        DamageTextInstance.transform.GetComponent<Transform>().localPosition += randomVector;
    }

    public void SpawnBlockText(BaseUnit unit, int blockAmount)
    {
        blockAmount *= 25;
        Vector3 randomVector = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0f);
        GameObject BlockTextInstance = Instantiate(blockTextPrefab, unit.transform);
        BlockTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().text = blockAmount.ToString() + "%";
        BlockTextInstance.transform.GetComponent<Transform>().localPosition += randomVector;
    }

    public void SpawnHealText(BaseUnit unit, int healAmount)
    {
        Vector3 randomVector = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0f);
        GameObject HealTextInstance = Instantiate(healTextPrefab, unit.transform);
        HealTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().text = healAmount.ToString();
        HealTextInstance.transform.GetComponent<Transform>().localPosition += randomVector;
    }

    public void SpawnCritText(BaseUnit unit) {
        Vector3 randomVector = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(0.4f, 0.5f), 0f);
        GameObject DamageTextInstance = Instantiate(critTextPrefab, unit.transform);
        DamageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().text = "*crit*";
        DamageTextInstance.transform.GetComponent<Transform>().localPosition += randomVector;
    }

    public void SpawnEffectImage(BaseUnit unit, EffectType effectType)
    {
        GameObject EffectImageInstance = Instantiate(effectPrefab, unit.transform);
        switch (effectType)
        {
            case EffectType.Poisioned:
                EffectImageInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = poisioned;
                break;
            case EffectType.Hasted:
                EffectImageInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = hasted;
                break;
            case EffectType.Burning:
                EffectImageInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = burning;
                break;
            case EffectType.Strength:
                EffectImageInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = strength;
                break;
            case EffectType.Invisible:
                EffectImageInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = invisible;
                break;
            case EffectType.Survival:
                EffectImageInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = survival;
                break;
            case EffectType.Stunned:
                EffectImageInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = stunned;
                break;
            case EffectType.Constrained:
                EffectImageInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = constrained;
                break;
            case EffectType.Weakened:
                EffectImageInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = weakened;
                break;
            case EffectType.Enraged:
                //EffectImageInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = enraged;
                break;
            case EffectType.Stressed:
                EffectImageInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = stressed;
                break;
            case EffectType.Evasion:
                //EffectImageInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = evasion;
                break;
            case EffectType.Taunt:
                //EffectImageInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = taunt;
                break;
            case EffectType.Adrenaline:
                EffectImageInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = adrenaline;
                break;
            case EffectType.Swiftness:
                EffectImageInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = swiftness;
                break;
            case EffectType.CritUp:
                EffectImageInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = critUp;
                break;
            case EffectType.Shocked:
                //EffectImageInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = shocked;
                break;
            default:
                break;
        }
    }

    public Sprite GetEffectImage(EffectType effectIconType)
    {
        switch (effectIconType)
        {
            case EffectType.Poisioned:
                return poisioned;
            case EffectType.Hasted:
                return hasted;
            case EffectType.Burning:
                return burning;
            case EffectType.Strength:
                return strength;
            case EffectType.Invisible:
                return invisible;
            case EffectType.Survival:
                return survival;
            case EffectType.Stunned:
                return stunned;
            case EffectType.Constrained:
                return constrained;
            case EffectType.Weakened:
                return weakened;
            case EffectType.Enraged:
                //return enraged;
            case EffectType.Stressed:
                return stressed;
            case EffectType.Evasion:
                //return evasion;
            case EffectType.Taunt:
                //return taunt;
            case EffectType.Adrenaline:
                return adrenaline;
            case EffectType.Swiftness:
                return swiftness;
            case EffectType.CritUp:
                return critUp;
            case EffectType.Shocked:
                //return shocked;
            default:
                return stunned;
        }
    }
}
