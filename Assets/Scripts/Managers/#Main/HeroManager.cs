using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UnitType{
    Archeologist,
    Tombraider,
    Guard,
    Priestress,
    Slave,
    Scribe,
    Druid,
    None,
}

public class HeroManager : MonoBehaviour
{

    public class DataUnit {
        public int ID;
        public BaseUnit prefab;
        public string name, type;
        public int maxHP, currentHP;

        public BaseItem weaponSlot;

        public BaseItem equipmentSlot;

        public BaseItem consumableSlotOne;

        public BaseItem consumableSlotTwo;

        public DataUnit(int _ID, BaseUnit _prefab, string _name, string _type, int _maxHP, int _currentHP) {
            ID = _ID;
            prefab = _prefab;
            name = _name;
            type = _type;
            maxHP = _maxHP;
            currentHP = _currentHP;
        }
    }

    public static HeroManager Instance;

    public DataUnit DataHeroOne, DataHeroTwo, DataHeroThree;

    public List<BaseUnit> selectedHeros = new();

    public List<Color32> unitColorCode = new()
    {
            new Color32(223,113,38,255),
            new Color32(118,66,138,255),
            new Color32(63,63,116,255),
            new Color32(230,186,5,255),
            new Color32(172,50,50,255),
            new Color32(1,1,1,255),
            new Color32(1,1,1,255),
        };

    Color32 colorOne = new Color32(223,113,38,255);
    Color32 colorTwo = new Color32(118,66,138,255);
    Color32 colorThree = new Color32(63,63,116,255);


    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }


    public void AddHeroToList(BaseUnit unit) {
        if (selectedHeros.Count < 3) {
            selectedHeros.Add(unit);
            SelectHeroPanelManager.Instance.UpdateHeroText();
            ReinitiateDataFromList();
        } else {
            Debug.Log("ERROR: Only 3 Heros Allowed");
        }
    }

    public void RemoveHeroFromList(BaseUnit unit) {
        if (selectedHeros.Count >0) {
            selectedHeros.Remove(unit);
            SelectHeroPanelManager.Instance.UpdateHeroText();
            ReinitiateDataFromList();
        }
    }

    public void ReinitiateDataFromList() {
        DataHeroOne = null;
        DataHeroTwo = null;
        DataHeroThree = null;
        InventoryManager.Instance.ClearAllItems();

            for (int i = 0; i < selectedHeros.Count; i++) {

                switch (i) {
                    case 0:
                        DataHeroOne = new DataUnit(i+1, selectedHeros[i], selectedHeros[i].UnitName, selectedHeros[i].UnitType, selectedHeros[i].MaxHealth, selectedHeros[i].CurrentHealth);
                        DataHeroOne.weaponSlot = ((BaseHero) selectedHeros[i]).StarterWeapon;
                        InventoryManager.Instance.addItemToInventory(DataHeroOne.weaponSlot, (int) TranslateStringTypeToUnitType(DataHeroOne.type));
                        break;
                    case 1:
                        DataHeroTwo = new DataUnit(i+1, selectedHeros[i], selectedHeros[i].UnitName, selectedHeros[i].UnitType, selectedHeros[i].MaxHealth, selectedHeros[i].CurrentHealth);
                        DataHeroTwo.weaponSlot = ((BaseHero) selectedHeros[i]).StarterWeapon;
                        InventoryManager.Instance.addItemToInventory(DataHeroTwo.weaponSlot, (int) TranslateStringTypeToUnitType(DataHeroTwo.type));
                        break;
                    case 2:
                        DataHeroThree = new DataUnit(i+1, selectedHeros[i], selectedHeros[i].UnitName, selectedHeros[i].UnitType, selectedHeros[i].MaxHealth, selectedHeros[i].CurrentHealth);
                        DataHeroThree.weaponSlot = ((BaseHero) selectedHeros[i]).StarterWeapon;
                        InventoryManager.Instance.addItemToInventory(DataHeroThree.weaponSlot, (int) TranslateStringTypeToUnitType(DataHeroThree.type));
                        break;
                    default:
                        break;
                }
            }
    }

    public void SafeHeroStatus(List<BaseUnit> heroes) {
        for (int i = 0; i < heroes.Count; i++) {
            BaseUnit hero = heroes[i];
            UpdateHeroData(i + 1, hero, hero.UnitName, hero.UnitType, hero.MaxHealth, hero.CurrentHealth);
            UpdateHeroInventory(i+1, hero.Weapon, hero.Equipment, hero.ConsumableOne, hero.ConsumableTwo);
        }
    }

    public void UpdateHeroData(int _ID, BaseUnit _prefab, string _name, string _type, int _maxHP, int _currentHP) {
        switch (_ID) {
            case 1:
                DataHeroOne.prefab = _prefab;
                DataHeroOne.name = _name;
                DataHeroOne.type = _type;
                DataHeroOne.maxHP = _maxHP;
                DataHeroOne.currentHP = _currentHP;
                break;
            case 2:
                DataHeroTwo.prefab = _prefab;
                DataHeroTwo.name = _name;
                DataHeroTwo.type = _type;
                DataHeroTwo.maxHP = _maxHP;
                DataHeroTwo.currentHP = _currentHP;
                break;
            case 3:
                DataHeroThree.prefab = _prefab;
                DataHeroThree.name = _name;
                DataHeroThree.type = _type;
                DataHeroThree.maxHP = _maxHP;
                DataHeroThree.currentHP = _currentHP;
                break;
            default:
                break;
        }
        
    }

    public void UpdateHeroInventory(int _ID, BaseItem weapon, BaseItem equipment, BaseItem consumableOne, BaseItem consumableTwo) {
        switch (_ID)
        {
            case 1:
                DataHeroOne.weaponSlot = weapon;
                DataHeroOne.equipmentSlot = equipment;
                DataHeroOne.consumableSlotOne = consumableOne;
                DataHeroOne.consumableSlotTwo = consumableTwo;
                break;
            case 2:
                DataHeroTwo.weaponSlot = weapon;
                DataHeroTwo.equipmentSlot = equipment;
                DataHeroTwo.consumableSlotOne = consumableOne;
                DataHeroTwo.consumableSlotTwo = consumableTwo;
                break;
            case 3:
                DataHeroThree.weaponSlot = weapon;
                DataHeroThree.equipmentSlot = equipment;
                DataHeroThree.consumableSlotOne = consumableOne;
                DataHeroThree.consumableSlotTwo = consumableTwo;
                break;
            default:
                break;
        }
    }

    public void PrintHeroHP() {
        if ( DataHeroOne != null) {
            print(DataHeroOne.type +" : " +DataHeroOne.currentHP);
        }
        if (DataHeroTwo != null) {
            print(DataHeroTwo.type + " : " + DataHeroTwo.currentHP);
        }
        if (DataHeroThree != null) {
            print(DataHeroThree.type + " : " + DataHeroThree.currentHP);
        }

    }

    public UnitType TranslateStringTypeToUnitType(string type) {
        switch (type)
        {
            case "Archeologist":
                return UnitType.Archeologist;
            case "Tomb Raider":
                return UnitType.Tombraider;
            case "Guard":
                return UnitType.Guard;
            case "Priestress":
                return UnitType.Priestress;
            case "Slave":
                return UnitType.Slave;
            case "Scribe":
                return UnitType.Scribe;
            case "Druid":
                return UnitType.Druid;
            default:
                return UnitType.None;
                
        }
    }

    /*
    public Color32 GetUnitColor(int id) {
        //return UnitColorCodes[id];
        //return null;
    }
    */

}
