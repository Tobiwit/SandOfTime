using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuiManager : MonoBehaviour
{
    public static GuiManager Instance;

    [SerializeField] private GameObject _selectedUnitNameObject,_selectedUnitTypeObject,_selectedUnitHPObject, _selectedUnitBlockObject, _selectedBlockIcon, _selectedHPIcon;
    [SerializeField] private GameObject _selContainer, _selAttackName, _selAttackDamage, _selAttackEffects, _selAttackDescription, _selAttackEnergy, _selAttackLineIndicator, _selActionIndiactor;
    [SerializeField] private GameObject _turnEnergy, _turnEnergyMax,_roundNumber;
    [SerializeField] private GameObject _effectUnit, _secEffectUnit;
    [SerializeField] private Sprite _indicatorBoth, _indicatorBack, _indicatorFront, _indicatorDamage, _indicatorHeal, _indicatorBlock;

    public GameObject effectUnitPrefab;
    public GameObject effectHolder;

    public Sprite tempImage;

    void Awake()
    {
        Instance = this;
    }

    #region effectDisplay

    public void UpdateEffectDisplay(BaseUnit unit)
    {
        foreach (Transform child in effectHolder.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        effectHolder.SetActive(true);
        var currentAppliedEffects = unit.AppliedEffects;
        int tempCounter = 0;
        foreach (KeyValuePair<EffectType, int> stat in currentAppliedEffects) {

            var effectUnit = Instantiate(effectUnitPrefab, effectHolder.transform.position, Quaternion.identity);
            effectUnit.transform.SetParent(effectHolder.transform);
            effectUnit.transform.localPosition = GetEffectUnitPosition(tempCounter);
            effectUnit.transform.localScale = new Vector3(1.5f, 1.5f, 0f);
            effectUnit.transform.GetChild(1).GetComponent<Image>().sprite = CuiManager.Instance.GetEffectImage(stat.Key);
            effectUnit.GetComponentInChildren<TextMeshProUGUI>().text = stat.Value.ToString() + "x";

            effectUnit.GetComponent<TooltipTrigger>().header = stat.Key.ToString();
            effectUnit.GetComponent<TooltipTrigger>().content = TooltipManager.Instance.GetTooltipContent(stat.Key);

            tempCounter++;
        }


        
    }

    private Vector3 GetEffectUnitPosition(int i)
    {
        return new Vector3(i * (120f) - 250f, 0f, 0f);
    }



    #endregion


    public void ShowTileInfo(Tile tile)
    {
        if (tile == null)
        {
            _selectedUnitNameObject.SetActive(false);
            _selectedUnitTypeObject.SetActive(false);
            _selectedUnitHPObject.SetActive(false);
            _selectedUnitBlockObject.SetActive(false);
            _selectedBlockIcon.SetActive(false);
            _selectedHPIcon.SetActive(false);
            effectHolder.SetActive(false);
            return;
        }

        if (tile.OccupiedUnit != null)
        {
            TextMeshProUGUI text1 = _selectedUnitNameObject.GetComponent<TextMeshProUGUI>();
            text1.text = tile.OccupiedUnit.UnitName;
            _selectedUnitNameObject.SetActive(true);
            TextMeshProUGUI text2 = _selectedUnitTypeObject.GetComponent<TextMeshProUGUI>();
            text2.text = tile.OccupiedUnit.UnitType;
            _selectedUnitTypeObject.SetActive(true);
            TextMeshProUGUI text3 = _selectedUnitHPObject.GetComponent<TextMeshProUGUI>();
            text3.text = tile.OccupiedUnit.CurrentHealth + "/" + tile.OccupiedUnit.MaxHealth;
            _selectedUnitHPObject.SetActive(true);
            TextMeshProUGUI text4 = _selectedUnitBlockObject.GetComponent<TextMeshProUGUI>();
            text4.text = tile.OccupiedUnit.CurrentBlock.ToString();
            _selectedUnitBlockObject.SetActive(true);
            _selectedBlockIcon.SetActive(true);
            _selectedHPIcon.SetActive(true);

            UpdateEffectDisplay(tile.OccupiedUnit);
        } else
        {
            _selectedUnitNameObject.SetActive(false);
            _selectedUnitTypeObject.SetActive(false);
            _selectedUnitHPObject.SetActive(false);
            _selectedUnitBlockObject.SetActive(false);
            _selectedBlockIcon.SetActive(false);
            _selectedHPIcon.SetActive(false);
            effectHolder.SetActive(false);
        }

    }


    


    public void EditAttackInfoDisplay(BaseAttack attack)
    {
        _selAttackName.GetComponent<TextMeshProUGUI>().text = attack.displayName;

        string actionAmountText = "";

        if (attack.actionAmount > 1)
        {
            actionAmountText = attack.actionAmount + "x ";
        }
        
        if (attack.actionMin == attack.actionMax)
        {
            actionAmountText += attack.actionMin;
        } else
        {
            actionAmountText += attack.actionMin + " - " + attack.actionMax;
        }

        switch (attack.actionType) {
            case ActionType.Attack:
                actionAmountText += " Dmg";
                _selActionIndiactor.SetActive(true);
                _selActionIndiactor.GetComponent<Image>().sprite = _indicatorDamage;
                break;
            case ActionType.Block:
                actionAmountText += " Hp";
                _selActionIndiactor.SetActive(true);
                _selActionIndiactor.GetComponent<Image>().sprite = _indicatorBlock;
                break;
            case ActionType.Heal:
                actionAmountText += " Hp";
                _selActionIndiactor.SetActive(true);
                _selActionIndiactor.GetComponent<Image>().sprite = _indicatorHeal;
                break;
            case ActionType.None:
                actionAmountText = "";
                _selActionIndiactor.SetActive(false);
                break;
        }
        _selAttackDamage.GetComponent<TextMeshProUGUI>().text = actionAmountText;

        _selAttackDescription.GetComponent<TextMeshProUGUI>().text = attack.description;
        string attackEnergyText = attack.energyCost.ToString();
        _selAttackEnergy.GetComponent<TextMeshProUGUI>().text = attackEnergyText;

        if (attack.lineType == LineType.Front)
        {
            _selAttackLineIndicator.GetComponent<Image>().sprite = _indicatorFront;
        } else if (attack.lineType == LineType.Back)
        {
            _selAttackLineIndicator.GetComponent<Image>().sprite = _indicatorBack;
        } else
        {
            _selAttackLineIndicator.GetComponent<Image>().sprite = _indicatorBoth;
        }


        if(attack.applyEffect)
        {
            tempImage = CuiManager.Instance.GetEffectImage(attack.effectType);
            _selAttackEffects.SetActive(true);
            if (attack.effectSelector != EffectSelector.None && attack.effectType != EffectType.Any)
            {
                
                _effectUnit.transform.GetChild(1).GetComponent<Image>().sprite = CuiManager.Instance.GetEffectImage(attack.effectType);
                _effectUnit.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = attack.effectAmount.ToString() + "x";

                _effectUnit.GetComponent<TooltipTrigger>().header = attack.effectType.ToString();
                _effectUnit.GetComponent<TooltipTrigger>().content = TooltipManager.Instance.GetTooltipContent(attack.effectType);

            }
            if (attack.secEffectSelector != EffectSelector.None && attack.secEffectType != EffectType.Any)
            {
                _secEffectUnit.transform.GetChild(1).GetComponent<Image>().sprite = CuiManager.Instance.GetEffectImage(attack.secEffectType);
                _secEffectUnit.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = attack.secEffectAmount.ToString() + "x";

                _secEffectUnit.GetComponent<TooltipTrigger>().header = attack.secEffectType.ToString();
                _secEffectUnit.GetComponent<TooltipTrigger>().content = TooltipManager.Instance.GetTooltipContent(attack.secEffectType);
            }

            string effectText = "";

            switch (attack.effectSelector)
            {
                case EffectSelector.None when attack.secEffectSelector == EffectSelector.None:
                    _effectUnit.SetActive(false);
                    _secEffectUnit.SetActive(false);
                    break;
                case EffectSelector.Self when attack.secEffectSelector == EffectSelector.None:
                    effectText += "Self:";
                    _effectUnit.SetActive(true);
                    _secEffectUnit.SetActive(false);
                    break;
                case EffectSelector.Self when attack.secEffectSelector == EffectSelector.Self:
                    effectText += "Self:";
                    _effectUnit.SetActive(true);
                    _secEffectUnit.SetActive(true);
                    _secEffectUnit.transform.localPosition = new Vector2(0f, 0f);
                    break;
                case EffectSelector.Self when attack.secEffectSelector == EffectSelector.Target:
                    effectText += "Self:	     Target:";
                    _effectUnit.SetActive(true);
                    _secEffectUnit.SetActive(true);
                    _secEffectUnit.transform.localPosition = new Vector2(160f, 0f);
                    break;
                case EffectSelector.Target when attack.secEffectSelector == EffectSelector.None:
                    effectText += "Target:";
                    _effectUnit.SetActive(true);
                    _secEffectUnit.SetActive(false);
                    break;
                case EffectSelector.Target when attack.secEffectSelector == EffectSelector.Target:
                    effectText += "Target:";
                    _effectUnit.SetActive(true);
                    _secEffectUnit.SetActive(true);
                    _secEffectUnit.transform.localPosition = new Vector2(0f, 0f);
                    break;
                default:
                    break;
            }

            _selAttackEffects.GetComponent<TextMeshProUGUI>().text = effectText;
        }
        else
        {
            _selAttackEffects.SetActive(false);
            _effectUnit.SetActive(false);
            _secEffectUnit.SetActive(false);
        }
    }

    public void ShowAttackInfoDisplay(bool value)
    {
        if (value)
        {
            _selContainer.transform.localPosition = new Vector2(730, -900);
            LeanTween.moveY(_selContainer, 350f, 0.5f).setEase(LeanTweenType.easeOutExpo);
        } else
        {
            _selContainer.LeanMoveLocalY(-900, 0.5f).setEaseInExpo();
        }
        //_selContainer.SetActive(value);
    }

    public void UpdateEnergyDisplay()
    {
        BaseHero hero = CombatManager.Instance.activeHero;
        int energyMax = hero.MaxTurnEnergy;
        int energy = hero.GetTurnEnergy();

        _turnEnergy.GetComponent<TextMeshProUGUI>().text = energy.ToString();
        string energyMaxText = "/ " + energyMax.ToString();
        _turnEnergyMax.GetComponent<TextMeshProUGUI>().text = energyMaxText;
    }

    public void UpdateRoundNumberDisplay(int round)
    {
        _roundNumber.GetComponent<TextMeshProUGUI>().text = "Round " + round.ToString();
    }


}