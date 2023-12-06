using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{

    public Tile OccupiedTile;
    public Faction Faction;

    public GameObject HealthbarPrefab;
    public float HealthbarOffset;
    private HealthbarBehaviour healthbarLogic;

    public float XOffset;
    public float YOffset;

    public string UnitName;
    public string UnitType;

    public int MaxHealth;
    public int CurrentHealth;

    public int MaxTurnEnergy;
    private int TurnEnergy;

    public int CurrentBlock;


    #region EffectApplication

    public Dictionary<EffectType, int> AppliedEffects = new Dictionary<EffectType, int>();
    public Dictionary<EffectType, int> SelfAppliedEffects = new Dictionary<EffectType, int>();

    #endregion

    #region Inventory

    public BaseItem Weapon;

    public BaseItem Equipment;

    public BaseItem ConsumableOne;

    public BaseItem ConsumableTwo;

    #endregion

    private void Awake()
    {

        if (HealthbarPrefab != null)
        {
            var Healthbar = Instantiate(HealthbarPrefab, gameObject.transform);
            healthbarLogic = Healthbar.gameObject.GetComponent<HealthbarBehaviour>();
            healthbarLogic.SetupHealthbar(MaxHealth, HealthbarOffset, CurrentHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        if (CurrentBlock > 0) {
            /* ## Old Block Mechanic
            if (CurrentBlock >= damage)
            {
                CurrentBlock -= damage;
                CuiManager.Instance.SpawnBlockText(this, damage);
            } else
            {
                int damageAfterBlock = damage - CurrentBlock;
                CurrentHealth -= damageAfterBlock;
                healthbarLogic.TakeDamage(damageAfterBlock);
                CuiManager.Instance.SpawnBlockText(this, CurrentBlock);
                CuiManager.Instance.SpawnDamageText(this, damageAfterBlock);
                CurrentBlock = 0;
            } */
            //## New Block
            int damageAfterBlock = (int)(damage * (1 - (CurrentBlock * 0.25)));
            if (damageAfterBlock < 0) { damageAfterBlock = 0; }
            CurrentHealth -= damageAfterBlock;
            healthbarLogic.TakeDamage(damageAfterBlock);
            CuiManager.Instance.SpawnBlockText(this, CurrentBlock);
            CuiManager.Instance.SpawnDamageText(this, damageAfterBlock);
        } else {
            CurrentHealth -= damage;
            healthbarLogic.TakeDamage(damage);
            CuiManager.Instance.SpawnDamageText(this, damage);
        }
        if (CurrentHealth < 0) {
            CurrentHealth = 0;
        }
        if (CurrentHealth == 0)
        {
            if(Faction == Faction.Enemy)
            {
                CombatManager.Instance.RemoveUnitFromTurnOrder(this);
                Destroy(gameObject);
            } else
            {
                if(AppliedEffects.ContainsKey(EffectType.Survival))
                {
                    CombatManager.Instance.RemoveUnitFromTurnOrder(this);
                    Destroy(gameObject);
                } else
                {
                    AppliedEffects[EffectType.Survival] = 1;
                    CuiManager.Instance.SpawnEffectImage(this,EffectType.Survival);
                }
            }
        }
        EffectManager.Instance.TriggerAllEffects(EffectState.OnBeingAttacked, this);
        if(CombatManager.Instance.activeHero == this) {
            GuiManager.Instance.ShowTileInfo(OccupiedTile);
        }
    }

    public void Heal(int healAmount)
    {
        if (AppliedEffects.ContainsKey(EffectType.Survival)) { AppliedEffects.Remove(EffectType.Survival); }
        if (CurrentHealth + healAmount <= MaxHealth)
        {
            CurrentHealth += healAmount;
            healthbarLogic.RestoreHealth(healAmount);
            CuiManager.Instance.SpawnHealText(this, healAmount);
        }
        else
        {
            healthbarLogic.RestoreHealth(MaxHealth - CurrentHealth);
            CuiManager.Instance.SpawnHealText(this, MaxHealth-CurrentHealth);
            CurrentHealth = MaxHealth;
        }
        if(CombatManager.Instance.activeHero == this) {
            GuiManager.Instance.ShowTileInfo(OccupiedTile);
        }
    }

    public void Block(int blockAmount)
    {
        /* ## Old Block
        CurrentBlock += blockAmount;
        CuiManager.Instance.SpawnBlockText(this, blockAmount); */
        // ## New Block
        CurrentBlock += blockAmount;
        CuiManager.Instance.SpawnBlockText(this, blockAmount);
        if (CurrentBlock > 4) {
            CurrentBlock = 4;
        }
        if(CombatManager.Instance.activeHero == this) {
            GuiManager.Instance.ShowTileInfo(OccupiedTile);
        }
    }

    public void KillUnit()
    {
        CombatManager.Instance.RemoveUnitFromTurnOrder(this);
        Destroy(gameObject);
    }

    public bool CheckEnergyCost(int energyCost)
    {
        if(energyCost <= TurnEnergy)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public void RemoveEnergy(int energyCost)
    {
        TurnEnergy -= energyCost;
        GuiManager.Instance.UpdateEnergyDisplay();
        if (TurnEnergy < 0)
        {
            TurnEnergy = 0;
        }
    }

    public void AddEnergy(int energyCost)
    {
        TurnEnergy += energyCost;
        GuiManager.Instance.UpdateEnergyDisplay();
        if (TurnEnergy > MaxTurnEnergy)
        {
            TurnEnergy = MaxTurnEnergy;
        }
    }

    public void SetTurnEnergy()
    {
        TurnEnergy = MaxTurnEnergy;
        GuiManager.Instance.UpdateEnergyDisplay();
    }

    public int GetTurnEnergy()
    {
        return TurnEnergy;
    }

    public void RecalibrateHealthbar(int maxHealth, int currentHealth) {
        healthbarLogic.SetupHealthbar(maxHealth, HealthbarOffset, currentHealth);
    }

}
