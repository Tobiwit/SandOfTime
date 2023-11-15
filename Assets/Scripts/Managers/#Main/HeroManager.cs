using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour
{

    public class DataUnit {
        public int ID;
        public BaseUnit prefab;
        public string name, type;
        public int maxHP, currentHP;

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
            } else if (selectedHeros.Count == 2) {
                DataHeroTwo = new DataUnit(2, unit, unit.UnitName, unit.UnitType, unit.MaxHealth, unit.CurrentHealth);
            } else {
                DataHeroThree = new DataUnit(3, unit, unit.UnitName, unit.UnitType, unit.MaxHealth, unit.CurrentHealth);
            }
        }
    }

    public void SafeHeroStatus(List<BaseUnit> heroes) {
        for (int i = 0; i < heroes.Count; i++) {
            BaseUnit hero = heroes[i];
            UpdateHeroData(i + 1, hero, hero.UnitName, hero.UnitType, hero.MaxHealth, hero.CurrentHealth);
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

}
