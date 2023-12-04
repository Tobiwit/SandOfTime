using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    private List<ScriptableUnit> _units;

    public List<BaseUnit> _unitsOnField;

    public BaseHero SelectedHero;

    void Awake()
    {
        Instance = this;

        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
    }

    public void SpawnHeroes()
    {
        List<BaseUnit> heroList = HeroManager.Instance.selectedHeros;

        foreach (BaseUnit hero in heroList)
        {
            var randomPrefab = hero;
            var spawnedHero = Instantiate(randomPrefab);

            var randomSpawnTile = GridManager.Instance.GetHeroSpawnTile();

            _unitsOnField.Add(spawnedHero);

            randomSpawnTile.SetUnit(spawnedHero);
        }

        LoadUnitData(_unitsOnField[0],HeroManager.Instance.DataHeroOne);
        if (heroList.Count > 1) LoadUnitData(_unitsOnField[1], HeroManager.Instance.DataHeroTwo);
        if (heroList.Count > 2) LoadUnitData(_unitsOnField[2], HeroManager.Instance.DataHeroThree);

        GameManager.Instance.UpdateGameState(GameState.SpawnEnemies);
    }

    public void LoadUnitData(BaseUnit unit, HeroManager.DataUnit data) {
        unit.UnitName = data.name;
        unit.UnitType = data.type;
        unit.MaxHealth = data.maxHP;
        unit.CurrentHealth = data.currentHP;
        unit.RecalibrateHealthbar(unit.MaxHealth, unit.CurrentHealth);
        
        unit.Weapon = data.weaponSlot;
        unit.Equipment = data.equipmentSlot;
        unit.ConsumableOne = data.consumableSlotOne;
        unit.ConsumableTwo = data.consumableSlotTwo;
    }

    public void SpawnEnemies()
    {
        List<BaseUnit> nextEnemies = LevelManager.Instance.nextEnemies.enemyList;

        foreach (BaseUnit enemy in nextEnemies) {
            var randomPrefab = enemy;
            var spawnedEnemy = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetEnemySpawnTile();

            print(spawnedEnemy);

            if (randomSpawnTile == null) { 
                print("ERROR: Not enough space for Enemy");
                return; }

            _unitsOnField.Add(spawnedEnemy);

            randomSpawnTile.SetUnit(spawnedEnemy);

            if (LevelManager.Instance.nextEnemies.isBossfight) {
                //We dont check if there is enough space for multiple tiles!
                BaseBoss bossEnemy = (BaseBoss)enemy;
                bossEnemy.AddMultiTiles(randomSpawnTile, spawnedEnemy);
            }
        }
        
        GameManager.Instance.UpdateGameState(GameState.CreateTurnOrder);
    }


    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit
    {
        return (T)_units.Where(u => u.Faction == faction && !u.isObject).OrderBy(o => Random.value).First().UnitPrefab;
    }


    public void SetSelectedHero(BaseHero hero)
    {
        SelectedHero = hero;
    }
}
