using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public List<BaseUnit> selectedHeros = new List<BaseUnit>();

    public List<Color> unitColorCodes = new List<Color>{
            new Color(223,113,38),
            new Color(118,66,138),
            new Color(63,63,116),
            new Color(230,186,5),
            new Color(172,50,50),
            new Color(1,1,1),
            new Color(1,1,1),
        };

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
            if (selectedHeros.Count == 1) {
                DataHeroOne = new DataUnit(1, unit, unit.UnitName, unit.UnitType, unit.MaxHealth, unit.CurrentHealth);
                DataHeroOne.weaponSlot = ((BaseHero) unit).StarterWeapon;
                DataHeroOne.weaponSlot.inHeroInventory = true;
                DataHeroOne.weaponSlot.belongingHero = translateStringTypeToUnitType(DataHeroOne.type);
                InventoryManager.Instance.addItemToInventory(DataHeroOne.weaponSlot);
            } else if (selectedHeros.Count == 2) {
                DataHeroTwo = new DataUnit(2, unit, unit.UnitName, unit.UnitType, unit.MaxHealth, unit.CurrentHealth);
                DataHeroTwo.weaponSlot = ((BaseHero) unit).StarterWeapon;
                DataHeroTwo.weaponSlot.inHeroInventory = true;
                DataHeroTwo.weaponSlot.belongingHero = translateStringTypeToUnitType(DataHeroTwo.type);
                InventoryManager.Instance.addItemToInventory(DataHeroTwo.weaponSlot);
            } else {
                DataHeroThree = new DataUnit(3, unit, unit.UnitName, unit.UnitType, unit.MaxHealth, unit.CurrentHealth);
                DataHeroThree.weaponSlot = ((BaseHero) unit).StarterWeapon;
                DataHeroThree.weaponSlot.inHeroInventory = true;
                DataHeroThree.weaponSlot.belongingHero = translateStringTypeToUnitType(DataHeroThree.type);
                InventoryManager.Instance.addItemToInventory(DataHeroThree.weaponSlot);
            }
        }
    }

    public void RemoveHeroFromList(BaseUnit unit) {
        if (selectedHeros.Count >0) {
            selectedHeros.Remove(unit);
            SelectHeroPanelManager.Instance.UpdateHeroText();
            if (selectedHeros.Count == 0) {
                DataHeroOne = null;
            } else if (selectedHeros.Count == 1) {
                DataHeroTwo = null;
            } else {
                DataHeroThree = null;
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

    public UnitType translateStringTypeToUnitType(string type) {
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

}
