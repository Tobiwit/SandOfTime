using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroPageManager : MonoBehaviour
{
    public static HeroPageManager Instance;

    public GameObject _inventoryBox;

    public GameObject _heroSprite, _heroName, _heroHealth;

    public GameObject _buttonOne, _buttonTwo, _buttonThree;

    public GameObject _slotWeapon, _slotEquipment, _slotConsumableOne, _slotConsumableTwo;

    public HeroManager.DataUnit selectedHero;
    
    private void Awake() {
        Instance = this;
        SetupHeroScreen();
        SetupMainInventory();
    }

    private void Start() {
        TabButton(0);
    }

    public void TabButton(int heroNumber) {
        DestroyHeroSlotItems();
        if (HeroManager.Instance.selectedHeros.Count <= heroNumber) {
            return;
        }
        switch (heroNumber) {
            case 0:
                selectedHero = HeroManager.Instance.DataHeroOne;
                break;
            case 1:
                selectedHero = HeroManager.Instance.DataHeroTwo;
                break;
            case 2:
                selectedHero = HeroManager.Instance.DataHeroThree;
                break;
            default:
                return;
        }
        //_heroSprite.GetComponent<Image>().Image.sprite = selectedHero._prefab.
        _heroName.GetComponent<TextMeshProUGUI>().text = selectedHero.name;
        _heroHealth.GetComponent<TextMeshProUGUI>().text = " " + selectedHero.maxHP + " / " + selectedHero.currentHP + " ";

        if (selectedHero.weaponSlot) {
            InventoryManager.Instance.SpawnItem(selectedHero.weaponSlot,_slotWeapon.gameObject, true, (int) HeroManager.Instance.TranslateStringTypeToUnitType(selectedHero.type));
        }
        if (selectedHero.equipmentSlot) {
            InventoryManager.Instance.SpawnItem(selectedHero.equipmentSlot,_slotEquipment.gameObject, true, (int) HeroManager.Instance.TranslateStringTypeToUnitType(selectedHero.type));
        }
        if (selectedHero.consumableSlotOne) {
            InventoryManager.Instance.SpawnItem(selectedHero.consumableSlotOne,_slotConsumableOne.gameObject, true, (int) HeroManager.Instance.TranslateStringTypeToUnitType(selectedHero.type));
        }
        if (selectedHero.consumableSlotTwo) {
            InventoryManager.Instance.SpawnItem(selectedHero.consumableSlotTwo,_slotConsumableTwo.gameObject, true, (int) HeroManager.Instance.TranslateStringTypeToUnitType(selectedHero.type));
        }

    }

    public void SetupHeroScreen() {
        int count = HeroManager.Instance.selectedHeros.Count;
        if (count > 0) {
            _buttonOne.SetActive(true);
            _buttonOne.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = HeroManager.Instance.DataHeroOne.type;
            if (count > 1) {
                _buttonTwo.SetActive(true);
                _buttonTwo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = HeroManager.Instance.DataHeroTwo.type;
                if (count > 2) {
                    _buttonThree.SetActive(true);
                    _buttonThree.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = HeroManager.Instance.DataHeroThree.type;
                }
            }
        }
    }


    public void SetupMainInventory() {
        GameObject InventoryContainer = _inventoryBox.transform.GetChild(1).gameObject;

        InventoryManager.Instance.SetupMainInventory(InventoryContainer, true);
    }

    public void DestroyHeroSlotItems() {
        if (_slotWeapon.transform.childCount > 0) {
            Object.Destroy(_slotWeapon.transform.GetChild(0).gameObject);
        }
        if (_slotEquipment.transform.childCount > 0) {
            Object.Destroy(_slotEquipment.transform.GetChild(0).gameObject);
        }
        if (_slotConsumableOne.transform.childCount > 0) {
            Object.Destroy(_slotConsumableOne.transform.GetChild(0).gameObject);
        }
        if (_slotConsumableTwo.transform.childCount > 0) {
            Object.Destroy(_slotConsumableTwo.transform.GetChild(0).gameObject);
        }
    
    }
}
