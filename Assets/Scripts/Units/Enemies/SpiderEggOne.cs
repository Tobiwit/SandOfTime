using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEggOne : BaseEnemy
{
    public BaseUnit spiderBabyPrefab;
    // Delete unused BaseAttack Slots
    public BaseAttack aOne;
    public RandomAttack randomAttack;

    public override void TurnLogic() {
        StartTurnLogic();
        randomAttack = RandomAttack.a1;

        while (turnState != TurnState.idle) {
            // Change calcCount to Amount of different Attacks
            if (AppliedEffects.ContainsKey(EffectType.Stunned) || calcCount > 2) {
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
                randomAttack = GetRandomAttack(75, 25);
                switch (randomAttack) {
                    // A1 = Spawn BabySpider
                    case RandomAttack.a1:
                        if (BackStepPossible()) {
                            turnState = TurnState.action;
                        } else { randomAttack = ChooseNextAttack(randomAttack); }
                        break;

                    // A2 = Fortify Eggshell
                    case RandomAttack.a2:
                        turnState = TurnState.action;
                        break;
                }
                break;

            case TurnState.action:

                switch (randomAttack) {
                    // Fetch Target and Execute Attack
                    case RandomAttack.a1:
                        Tile tile = GridManager.Instance.GetTileAtPosition(new Vector2(OccupiedTile.xVector + 1, OccupiedTile.yVector));
                        var babyUnit = Instantiate(spiderBabyPrefab);
                        CombatManager.Instance.nextUpInTurnOrder.Add(babyUnit);
                        tile.SetUnit(babyUnit);
                        KillUnit();
                        break;
                    case RandomAttack.a2:
                        CombatManager.Instance.ExecuteEnemyAttack(aOne, this);
                        break;
                }
                turnState = TurnState.idle;
                break;
            case TurnState.postMovement:
                if (moved) {
                    turnState = TurnState.idle;
                } else {
                    Tile move = GetRandomPossibleTile();
                    move.SetUnit(this);
                }
                break;

        }
    }
}
