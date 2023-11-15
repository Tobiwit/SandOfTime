using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour {
    public static CombatManager Instance;

    public List<BaseUnit> turnOrder;
    public List<BaseUnit> nextUpInTurnOrder;
    public int turn;

    public BaseHero activeHero;
    public BaseEnemy activeEnemy;

    public BaseAttack activeAttack;
    public int activeAttackNum;

    public BaseAttack demoAttackEnemy;

    private int roundNumber = 1;
    private bool skipPostTurn = false;

    private void Awake() {
        Instance = this;
    }

    public void SetupTurnOrder() {
        foreach (BaseUnit unit in nextUpInTurnOrder) {
            SpawnManager.Instance._unitsOnField.Add(unit);
        }
        nextUpInTurnOrder.Clear();
        turnOrder = Shuffle<BaseUnit>(SpawnManager.Instance._unitsOnField);
        TurnOrderDisplayManager.Instance.SetupTurnOrderDisplay(turnOrder);
        turn = 0;
        if (turnOrder[0].Faction == Faction.Hero) {
            activeHero = (BaseHero)turnOrder[0];
            PreTurnLogic();
            GameManager.Instance.UpdateGameState(GameState.PlayerTurn);
        } else {
            StartEnemyTurn();
        }
    }

    public void RemoveUnitFromTurnOrder(BaseUnit unit) {
        if (turnOrder.Contains(unit)) {
            int index = turnOrder.IndexOf(unit);
            SpawnManager.Instance._unitsOnField.Remove(unit);
            CheckForWinOrLose();
            if (index == turn) {
                turn--;
                skipPostTurn = true;
                AdvanceTurn();
                return;
            } else if (index < turn) {
                turn--;
                return;
            } else if (index > turn) {
                return;
            }
        }
        if (nextUpInTurnOrder.Contains(unit)) {
            nextUpInTurnOrder.Remove(unit);
        }
    }

    public void SafeHeroData() {
        List<BaseUnit> LivingHeros = new List<BaseUnit>();
        foreach (BaseUnit checkingUnit in SpawnManager.Instance._unitsOnField) {
            if (checkingUnit.Faction == Faction.Hero) {
                LivingHeros.Add(checkingUnit);
            }
        }
        HeroManager.Instance.SafeHeroStatus(LivingHeros);
    }

    public void AdvanceTurn() {
        SafeHeroData();
        if (!skipPostTurn) {
            PostTurnLogic();
        } else {
            skipPostTurn = false;
        }

        CheckForWinOrLose();

        GuiManager.Instance.ShowTileInfo(null);

        if (turn < 0) {
            turn = 0;
        } else {
            turn++;
        }


        TurnOrderDisplayManager.Instance.UpdateTurnOrderDisplay(turnOrder, turn);
        if (turn < turnOrder.Count) {
            if (turnOrder[turn].Faction == Faction.Hero) {
                activeHero = (BaseHero)turnOrder[turn];
                PreTurnLogic();
                GameManager.Instance.UpdateGameState(GameState.PlayerTurn);
                TurnLogic();
            } else {
                StartEnemyTurn();
            }
        } else if (turn == turnOrder.Count) {
            roundNumber++;
            GuiManager.Instance.UpdateRoundNumberDisplay(roundNumber);
            SetupTurnOrder();
            AttacksDisplayManager.Instance.RedrawAttackDisplay();
        }
    }

    private void CheckForWinOrLose() {
        int herosAlive = 0;
        int enemiesAlive = 0;
        foreach (BaseUnit checkingUnit in SpawnManager.Instance._unitsOnField) {
            if (checkingUnit.Faction == Faction.Hero) {
                herosAlive++;
            } else if (checkingUnit.Faction == Faction.Enemy) {
                enemiesAlive++;
            }
        }
        if (herosAlive == 0) {
            GameManager.Instance.UpdateGameState(GameState.Lose);
        } else if (enemiesAlive == 0 && nextUpInTurnOrder != null) {
            SafeHeroData();
            GameManager.Instance.UpdateGameState(GameState.Victory);
        }
    }

    private void PreTurnLogic() {
        if (activeHero != null) {
            activeHero.CurrentBlock = 0;
            AttacksDisplayManager.Instance.SetSpecialAttack(activeHero._specialAbilityAttack);
            GuiManager.Instance.ShowTileInfo(activeHero.OccupiedTile);
            activeHero.OccupiedTile._activeCharacterHighlight.SetActive(true);
            EffectManager.Instance.TriggerAllEffects(EffectState.OnTurnStart, activeHero);
            activeHero.SetTurnEnergy();
        }


    }

    private void TurnLogic() {

    }

    private void PostTurnLogic() {
        EffectManager.Instance.TriggerAllEffects(EffectState.OnTurnEnd, (BaseUnit)turnOrder[turn]);
        if (activeHero != null) {
            AttacksDisplayManager.Instance.RemoveSpecialAttack();
            activeHero.OccupiedTile._activeCharacterHighlight.SetActive(false);
            activeHero = null;
        }
        AttacksDisplayManager.Instance.DeactivateAllAttacks();
        GuiManager.Instance.ShowAttackInfoDisplay(false);
        if (activeAttack != null) {
            activeAttack = null;
        }
    }

    private void StartEnemyTurn() {
        activeEnemy = (BaseEnemy)turnOrder[turn];
        EffectManager.Instance.TriggerAllEffects(EffectState.OnTurnStart, activeEnemy);
        GameManager.Instance.UpdateGameState(GameState.EnemyTurn);
        activeEnemy.OccupiedTile._activeCharacterHighlight.SetActive(true);
    }

    public void EndTurnButton() {
        if (GameManager.Instance.State == GameState.PlayerTurn) {
            AdvanceTurn();
        } else {
            return;
        }
    }

    public void EnemyTurnAI() {
        CheckForWinOrLose();
        activeEnemy.TurnLogic();
    }

    public List<T> Shuffle<T>(List<T> _list) {
        for (int i = 0; i < _list.Count; i++) {
            T temp = _list[i];
            int randomIndex = Random.Range(i, _list.Count);
            _list[i] = _list[randomIndex];
            _list[randomIndex] = temp;
        }
        return _list;
    }

    public bool CheckIfAttackPossible(BaseAttack attack) {
        if (GameManager.Instance.State != GameState.PlayerTurn) {
            return false;
        }
        /*
        if (attack.playerClass != PlayerClass.All) {
            //REWORK with ACTIVE_HERO
            return false;
        }
        */
        if (!activeHero.CheckEnergyCost(attack.energyCost)) {
            EventTextManager.Instance.NewWarningText("You don't have enough energy");
            return false;
        }
        if (!CheckForRightLine(attack.lineType)) {
            return false;
        }
        return true;
    }

    private bool CheckForRightLine(LineType lineType) {
        switch (lineType) {
            case LineType.Both:
                return true;
            case LineType.Front:
                if (activeHero.OccupiedTile.xVector == 0) {
                    return false;
                }
                break;
            case LineType.Back:
                if (activeHero.OccupiedTile.xVector == 1) {
                    return false;
                }
                break;
        }
        return true;
    }

    public void SetActiveAttack(BaseAttack attack, int number) {
        activeAttack = attack;
        activeAttackNum = number;
    }

    public void TriggerActiveAttack(BaseUnit targetedUnit) {
        if (activeAttack != null) {


            if (!CheckIfTargetIsInTheRightTeam(activeAttack, targetedUnit)) {
                EventTextManager.Instance.NewWarningText("Invalid Target!");
                AttacksDisplayManager.Instance.DeactivateAllAttacks();
                return;
            }

            if (activeAttack.targetPattern.targetingType == TargetingType.TargetRestricted) {
                if (targetedUnit.OccupiedTile.isTargeted) {
                    List<BaseUnit> tempTargets = new List<BaseUnit> { targetedUnit };
                    //AttacksDisplayManager.Instance.UpdateDisplayNewCard(activeAttack);
                    ExecuteAttack(activeAttack, tempTargets);
                } else {
                    //SAY NO VALID SQUARE SELECTED
                    AttacksDisplayManager.Instance.DeactivateAllAttacks();
                }
            } else if (activeAttack.targetPattern.targetingType == TargetingType.FixedAOE) {
                List<BaseUnit> tempTargets = new List<BaseUnit> { };
                foreach (BaseUnit tempUnit in turnOrder) {
                    if (tempUnit != null && tempUnit.OccupiedTile.isTargeted) {
                        tempTargets.Add(tempUnit);
                    }
                }
                if (tempTargets != null) {
                    //AttacksDisplayManager.Instance.UpdateDisplayNewCard(activeAttack);
                    ExecuteAttack(activeAttack, tempTargets);
                } else {
                    //SAY NO UNIT IN AOE
                    AttacksDisplayManager.Instance.DeactivateAllAttacks();
                }
            } else {
                List<BaseUnit> tempTargets = new List<BaseUnit> { targetedUnit };
                //AttacksDisplayManager.Instance.UpdateDisplayNewCard(activeAttack);
                ExecuteAttack(activeAttack, tempTargets);
            }
        } else {
            //SAY NO VALID ATTACK SELECTED
            AttacksDisplayManager.Instance.DeactivateAllAttacks();
        }
        TargetManager.Instance.HideTargetedTiles();
        GuiManager.Instance.ShowTileInfo(targetedUnit.OccupiedTile);
    }

    public bool CheckIfTargetIsInTheRightTeam(BaseAttack attack, BaseUnit targetedUnit) {
        ActionTeam attackTeam = attack.actionTeam;
        switch (attackTeam) {
            case ActionTeam.Enemy:
                return (targetedUnit.Faction != Faction.Hero);
            case ActionTeam.TeamOnly:
                return (targetedUnit.Faction == Faction.Hero && targetedUnit != activeHero);
            case ActionTeam.SelfOnly:
                return (targetedUnit == activeHero);
            case ActionTeam.TeamAndSelf:
                return (targetedUnit.Faction == Faction.Hero);
            case ActionTeam.Everybody:
                return true;
        }
        return false;
    }

    public void ExecuteAttack(BaseAttack attack, List<BaseUnit> targetList) {
        foreach (BaseUnit target in targetList) {
            for (int actionAmount = 0; actionAmount < attack.actionAmount; actionAmount++) {
                int randomActionAmount = Random.Range(attack.actionMin, attack.actionMax);
                int finalActionAmount = CalculateFinalDamageAmount(randomActionAmount, target);
                switch (attack.actionType) {
                    case ActionType.Attack:
                        if (target.Faction == Faction.Enemy) {
                            target.TakeDamage(finalActionAmount);
                            EffectManager.Instance.TriggerAllEffects(EffectState.OnAttacking, activeHero);
                        } else { return; }
                        break;
                    case ActionType.Block:
                        if (target.Faction == Faction.Hero) {
                            target.Block(finalActionAmount);
                        } else { return; }
                        break;
                    case ActionType.Heal:
                        if (target.Faction == Faction.Hero) {
                            target.Heal(finalActionAmount);
                        } else { return; }
                        break;
                    case ActionType.None:
                        break;
                    default:
                        break;
                }
                if (attack.applyEffect) {
                    if (target == activeHero) {
                        EffectManager.Instance.HandleEffects(attack.effectType, EffectState.OnApplySelf, activeHero, attack.effectAmount);
                        CuiManager.Instance.SpawnEffectImage(activeHero, attack.effectType);

                        if (attack.secEffectSelector != EffectSelector.None) {
                            EffectManager.Instance.HandleEffects(attack.secEffectType, EffectState.OnApplySelf, activeHero, attack.secEffectAmount);
                            CuiManager.Instance.SpawnEffectImage(activeHero, attack.secEffectType);
                        }
                    } else {
                        if (attack.effectSelector == EffectSelector.Target) {
                            EffectManager.Instance.HandleEffects(attack.effectType, EffectState.OnApply, target, attack.effectAmount);
                            CuiManager.Instance.SpawnEffectImage(target, attack.effectType);
                        } else if (attack.effectSelector == EffectSelector.Self) {
                            EffectManager.Instance.HandleEffects(attack.effectType, EffectState.OnApplySelf, activeHero, attack.effectAmount);
                            CuiManager.Instance.SpawnEffectImage(activeHero, attack.effectType);
                        } else { }

                        if (attack.secEffectSelector == EffectSelector.Target) {
                            EffectManager.Instance.HandleEffects(attack.secEffectType, EffectState.OnApply, target, attack.secEffectAmount);
                            CuiManager.Instance.SpawnEffectImage(target, attack.secEffectType);
                        } else if (attack.secEffectSelector == EffectSelector.Self) {
                            EffectManager.Instance.HandleEffects(attack.secEffectType, EffectState.OnApplySelf, activeHero, attack.secEffectAmount);
                            CuiManager.Instance.SpawnEffectImage(activeHero, attack.secEffectType);
                        } else { }
                    }
                }
                if (attack.applySpecialFunction) {
                    CardsSpecialFunctionManager.Instance.computeSpecialFunction(attack.CardID, target, finalActionAmount, activeHero);
                }
            }
        }

        

        AttacksDisplayManager.Instance.UpdateDisplayNewCard(activeAttack,activeAttackNum);

        EventTextManager.Instance.NewEventText("You used " + attack.displayName);

        activeHero.RemoveEnergy(attack.energyCost);
        activeAttack = null;
        AttacksDisplayManager.Instance.DeactivateAllAttacks();

    }

    private int CalculateFinalDamageAmount(int randomAmount, BaseUnit unit) {
        BaseUnit attackerUnit = turnOrder[turn];

        int critRandom = Random.Range(0, 100);

        if (attackerUnit.AppliedEffects.ContainsKey(EffectType.CritUp)) {
            critRandom = 0;
            attackerUnit.AppliedEffects.Remove(EffectType.CritUp);
        }


        if (critRandom < 5) {
            randomAmount *= 2;
            CuiManager.Instance.SpawnCritText(unit);
        }

        if (attackerUnit.AppliedEffects.ContainsKey(EffectType.Weakened) && attackerUnit.AppliedEffects.ContainsKey(EffectType.Strength)) {
            return randomAmount;
        } else if (attackerUnit.AppliedEffects.ContainsKey(EffectType.Weakened)) {
            return (int)Mathf.Floor(randomAmount * 0.75f);
        } else if (attackerUnit.AppliedEffects.ContainsKey(EffectType.Strength)) {
            return (int)Mathf.Ceil(randomAmount * 1.25f);
        } else {
            return randomAmount;
        }
    }


    #region EnemyExecuteAttack
    public void ExecuteEnemyAttack(BaseAttack attack, BaseUnit target) {
        int randomActionAmount = Random.Range(attack.actionMin, attack.actionMax);
        int finalActionAmount = CalculateFinalDamageAmount(randomActionAmount, target);
        switch (attack.actionType) {
            case ActionType.Attack:
                if (target.Faction == Faction.Hero) {
                    target.TakeDamage(finalActionAmount);
                    EffectManager.Instance.TriggerAllEffects(EffectState.OnAttacking, activeEnemy);
                } else { return; }
                break;
            case ActionType.Block:
                if (target.Faction == Faction.Enemy) {
                    target.Block(finalActionAmount);
                } else { return; }
                break;
            case ActionType.Heal:
                if (target.Faction == Faction.Enemy) {
                    target.Heal(finalActionAmount);
                } else { return; }
                break;
            case ActionType.None:
                break;
            default:
                break;
        }
        if (attack.applyEffect) {
            EffectManager.Instance.HandleEffects(attack.effectType, EffectState.OnApply, target, attack.effectAmount);
            CuiManager.Instance.SpawnEffectImage(target, attack.effectType);
        }
        if (attack.applySpecialFunction) {
            CardsSpecialFunctionManager.Instance.computeSpecialFunction(attack.CardID, target, finalActionAmount, activeEnemy);
        }



        EventTextManager.Instance.NewEventText(activeEnemy.UnitName + " used " + attack.displayName);

    }


    #endregion EnemyExecuteAttack

    public Tile GetActiveHeroTile() {
        if (activeHero == null) {
            return null;
        } else {
            return activeHero.OccupiedTile;
        }

    }


}
