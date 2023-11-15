using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderOne : BaseEnemy {
    // Delete unused BaseAttack Slots
    public BaseAttack aOne, aTwo;
    public BaseUnit spiderEggPrefab;
    public RandomAttack randomAttack;

    public override void TurnLogic() {

        StartTurnLogic();

        randomAttack = RandomAttack.a1;

        while (turnState != TurnState.idle) {
            // Change calcCount to Amount of different Attacks
            if (AppliedEffects.ContainsKey(EffectType.Stunned) || calcCount > 3) {
                turnState = TurnState.idle;
            } else {
                HandleTurns();
            }
        }
        EndTurnLogic();
    }


    private void HandleTurns() {
        switch (turnState) {
            case TurnState.idle:
                break;
            case TurnState.preAction:

                // Set Probability of Attacks (sum = 100)
                randomAttack = GetRandomAttack(25, 50, 25);
                Tile moveTile = null;
                switch (randomAttack) {
                    // A1 = Webbed
                    case RandomAttack.a1:
                        moveTile = ShouldIMove(TargetingType.TargetRestricted, AOEType.None, LineType.Back);
                        if (moveTile == OccupiedTile) {
                            turnState = TurnState.action;
                        } else if (moveTile != null) {
                            moveTile.SetUnit(this);
                            moved = true;
                            turnState = TurnState.action;
                        } else { randomAttack = ChooseNextAttack(randomAttack); }
                        break;

                    // A2 = Bite
                    case RandomAttack.a2:
                        moveTile = ShouldIMove(TargetingType.TargetRestricted, AOEType.ThreeLine, LineType.Front);
                        if (moveTile == OccupiedTile) {
                            turnState = TurnState.action;
                        } else if (moveTile != null) {
                            moveTile.SetUnit(this);
                            moved = true;
                            turnState = TurnState.action;
                        } else { randomAttack = ChooseNextAttack(randomAttack); }
                        break;

                    // A3 = Summon Baby Spider
                    case RandomAttack.a3:
                        Tile summonTile = GetFreeMiddleTileIfPossible();
                        if (summonTile != null) {
                            turnState = TurnState.action;
                        }
                        break;
                }
                break;

            case TurnState.action:

                switch (randomAttack) {
                    // Fetch Target and Execute Attack
                    case RandomAttack.a1:
                        CombatManager.Instance.ExecuteEnemyAttack(aOne, GetRandomTarget());
                        break;
                    case RandomAttack.a2:
                        CombatManager.Instance.ExecuteEnemyAttack(aTwo, GetEnemyThreeLine(OccupiedTile));
                        break;
                    case RandomAttack.a3:
                        if (GetFreeMiddleTileIfPossible() != null) {
                            Tile tile = GetFreeMiddleTileIfPossible();
                            var eggUnit = Instantiate(spiderEggPrefab);
                            CombatManager.Instance.nextUpInTurnOrder.Add(eggUnit);
                            tile.SetUnit(eggUnit);
                        }
                        break;
                    case RandomAttack.a4:
                        break;
                }
                turnState = TurnState.idle;
                break;
            case TurnState.postMovement:
                if (!moved) {
                    Tile move = GetRandomPossibleTile();
                    move.SetUnit(this);
                }
                turnState = TurnState.idle;
                break;

        }
    }
}
