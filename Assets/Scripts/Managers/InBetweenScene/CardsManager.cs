using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardsManager : MonoBehaviour {
    public static CardsManager Instance;

    public AttacksDisplay inBetweenDisplay;
    public AttacksContainerDisplay inputInventoryContainer;

    public Image prefab;

    public GameObject detailCard;
    public Sprite _tempImage;
    public Sprite _indicatorHeal, _indicatorDamage, _indicatorBlock, _indicatorFront, _indicatorBack, _indicatorBoth;

    public GameObject scrollViewHolder;

    public List<Image> attacksDisplayed;

    public int selectedCardIndex = -1;

    private void Awake() {
        Instance = this;

    }

    private void Start() {
        //UpdateCurrentCardDeck();
        UnpackContainerToInBetweenDisplay();
        CreateDisplay();
        scrollViewHolder.GetComponent<RectTransform>().localPosition = new Vector3(260f, -625f, 0);
    }

    private void OnApplicationQuit() {
        inBetweenDisplay.Container.Clear();
    }

    private void UnpackContainerToInBetweenDisplay() {
        inBetweenDisplay.Container.Clear();
        foreach (KeyValuePair<BaseAttackContainer,int> pair in inputInventoryContainer.Container) {
            if (pair.Value == 0) {
                inBetweenDisplay.AddAttack(DeckManager.Instance.unpowered);
            } else {
                inBetweenDisplay.AddAttack(pair.Key.GetBaseAttackByLevel(pair.Value));
            }
        }
    }

    public void UpdateCurrentCardDeck() {
        //if (inBetweenDisplay.Container.Count != inputInventory.Container.Count) 
        inBetweenDisplay.Container.Clear();
        UnpackContainerToInBetweenDisplay();
        foreach (Transform child in scrollViewHolder.transform) {
            GameObject.Destroy(child.gameObject);
        }
        scrollViewHolder.GetComponent<RectTransform>().localPosition = new Vector3(260f, -625f, 0);
        scrollViewHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(250f, (inBetweenDisplay.Container.Count * 75f) + 125f);
        CreateDisplay();

    }

    public void CreateDisplay() {
        attacksDisplayed.Clear();
        for (int i = 0; i < inBetweenDisplay.Container.Count; i++) {
            var obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, scrollViewHolder.transform);
            obj.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);

            KeyValuePair<BaseAttackContainer, int> cardPair = DeckManager.Instance.GetPairByIndex(i);

            if (inBetweenDisplay.Container[i] == DeckManager.Instance.unpowered) {
                string altName = cardPair.Key.GetBaseAttackByLevel(1).displayName;
                obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = inBetweenDisplay.Container[i].displayName + " " + altName;
                obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().fontSize = 10f;
            } else {
                obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = inBetweenDisplay.Container[i].displayName;
            }
            obj.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = inBetweenDisplay.Container[i].energyCost.ToString();

            
            if (inBetweenDisplay.Container[i] == DeckManager.Instance.unpowered) {
                obj.transform.GetChild(1).GetComponent<Image>().sprite = cardPair.Key.GetBaseAttackByLevel(1).skillImage;
            } else {
                if (inBetweenDisplay.Container[i].skillImage != null) {
                    obj.transform.GetChild(1).GetComponent<Image>().sprite = inBetweenDisplay.Container[i].skillImage;
                }
            }


            obj.GetComponent<DeckAttackPrefab>().SetScript(inBetweenDisplay.Container[i]);
            obj.GetComponent<DeckAttackPrefab>().index = i;

            attacksDisplayed.Add(obj.GetComponent<Image>());
        }
        
    }



    public void DeactivateAllAttacks() {
        GuiManager.Instance.ShowAttackInfoDisplay(false);
        foreach (var attack in attacksDisplayed) {
            attack.gameObject.GetComponent<DeckAttackPrefab>().SetAttackActivity(false);
        }
    }

    public void DeactivateAllAttacksExcept(int index) {
        foreach (var attack in attacksDisplayed) {
            if (attack != attacksDisplayed[index]) {
                attack.gameObject.GetComponent<DeckAttackPrefab>().SetAttackActivity(false);
            }
        }
    }

    public void UpgradeCard() {
        if (selectedCardIndex == -1) { return; }
        if (DeckManager.Instance.GetScarabCrystals() < 1) { return; }
        KeyValuePair<BaseAttackContainer,int> cardPair = DeckManager.Instance.GetPairByIndex(selectedCardIndex);
        if (cardPair.Value > 2) { return; }
        DeckManager.Instance.UpdateScarabCrystals(-1);
        DeckManager.Instance.UpdatePairValueByIndex(selectedCardIndex,1);
        EditAttackInfoDisplay(cardPair.Key.GetBaseAttackByLevel(cardPair.Value +1));
        UpdateCurrentCardDeck();
    }

    public void DowngradeCard() {
        if (selectedCardIndex == -1) { return; }
        KeyValuePair<BaseAttackContainer, int> cardPair = DeckManager.Instance.GetPairByIndex(selectedCardIndex);
        if (cardPair.Value < 1) { return; }
        DeckManager.Instance.UpdateScarabCrystals(1);
        DeckManager.Instance.UpdatePairValueByIndex(selectedCardIndex, -1);
        if (cardPair.Value - 1 == 0) {
            EditAttackInfoDisplay(cardPair.Key.GetBaseAttackByLevel(1));
        } else {
            EditAttackInfoDisplay(cardPair.Key.GetBaseAttackByLevel(cardPair.Value - 1));
        }
        UpdateCurrentCardDeck();
    }


    public void EditAttackInfoDisplay(BaseAttack attack) {
        EditDetailCardDisplay(attack, detailCard);
    }

    public void EditAttackInfoDisplayUnpowered(BaseAttack attack) {
        EditDetailCardDisplay(DeckManager.Instance.unpowered, detailCard, attack);
    }

    public void EditDetailCardDisplay(BaseAttack attack, GameObject cardObject, BaseAttack altAttack = null) {
        GameObject _selAttackName = cardObject.transform.GetChild(0).gameObject;
        GameObject _selAttackDescription = cardObject.transform.GetChild(1).gameObject;
        GameObject _selAttackDamage = cardObject.transform.GetChild(2).gameObject;
        GameObject _selAttackEffects = cardObject.transform.GetChild(3).gameObject;
        GameObject _selAttackEnergy = cardObject.transform.GetChild(4).gameObject;
        GameObject _selAttackLineIndicator = cardObject.transform.GetChild(6).gameObject;
        GameObject _selActionIndiactor = cardObject.transform.GetChild(7).gameObject;

        GameObject _effectUnit = cardObject.transform.GetChild(5).GetChild(0).gameObject;
        GameObject _secEffectUnit = cardObject.transform.GetChild(5).GetChild(1).gameObject;


        if (altAttack != null) {
            _selAttackName.GetComponent<TextMeshProUGUI>().text = attack.displayName + " " + altAttack.displayName;
        } else {
            _selAttackName.GetComponent<TextMeshProUGUI>().text = attack.displayName;
        }

        string actionAmountText = "";

        if (attack.actionAmount > 1) {
            actionAmountText = attack.actionAmount + "x ";
        }

        if (attack.actionMin == attack.actionMax) {
            actionAmountText += attack.actionMin;
        } else {
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

        if (attack.lineType == LineType.Front) {
            _selAttackLineIndicator.GetComponent<Image>().sprite = _indicatorFront;
        } else if (attack.lineType == LineType.Back) {
            _selAttackLineIndicator.GetComponent<Image>().sprite = _indicatorBack;
        } else {
            _selAttackLineIndicator.GetComponent<Image>().sprite = _indicatorBoth;
        }


        if (attack.applyEffect) {
            _tempImage = CuiManager.Instance.GetEffectImage(attack.effectType);
            _selAttackEffects.SetActive(true);
            if (attack.effectSelector != EffectSelector.None && attack.effectType != EffectType.Any) {

                _effectUnit.transform.GetChild(1).GetComponent<Image>().sprite = CuiManager.Instance.GetEffectImage(attack.effectType);
                _effectUnit.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = attack.effectAmount.ToString() + "x";
            }
            if (attack.secEffectSelector != EffectSelector.None && attack.secEffectType != EffectType.Any) {
                _secEffectUnit.transform.GetChild(1).GetComponent<Image>().sprite = CuiManager.Instance.GetEffectImage(attack.secEffectType);
                _secEffectUnit.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = attack.secEffectAmount.ToString() + "x";
            }

            string effectText = "";

            switch (attack.effectSelector) {
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
        } else {
            _selAttackEffects.SetActive(false);
            _effectUnit.SetActive(false);
            _secEffectUnit.SetActive(false);
        }
    }

}
