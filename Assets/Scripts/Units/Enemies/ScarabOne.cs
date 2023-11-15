using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarabOne : BaseEnemy
{
    // Delete unused BaseAttack Slots
    public BaseAttack aOne, aTwo, aThree;
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
                randomAttack = GetRandomAttack(10, 50, 40);
                Tile moveTile = null;
                switch (randomAttack) {
                    // A1 = Simple Bite
                    case RandomAttack.a1:
                        moveTile = ShouldIMove(TargetingType.TargetRestricted, AOEType.ThreeLine, LineType.Front);
                        if (moveTile == OccupiedTile) {
                            turnState = TurnState.action;
                        } else if (moveTile != null) {
                            moveTile.SetUnit(this);
                            moved = true;
                            turnState = TurnState.action;
                        } else { randomAttack = ChooseNextAttack(randomAttack); }
                        break;

                    // A2 = Granting Luck
                    case RandomAttack.a2:
                        if(GetRandomTeamMember(this) == null) { randomAttack = ChooseNextAttack(randomAttack); }
                        moveTile = ShouldIMove(TargetingType.TargetRestricted, AOEType.None, LineType.Back);
                        if (moveTile == OccupiedTile) {
                            turnState = TurnState.action;
                        } else if (moveTile != null) {
                            moveTile.SetUnit(this);
                            moved = true;
                            turnState = TurnState.action;
                        } else { randomAttack = ChooseNextAttack(randomAttack); }
                        break;

                    // A3 = Sharp Blades
                    case RandomAttack.a3:
                        moveTile = ShouldIMove(TargetingType.TargetRestricted, AOEType.FrontLine, LineType.Front);
                        if (moveTile == OccupiedTile) {
                            turnState = TurnState.action;
                        } else if (moveTile != null) {
                            moveTile.SetUnit(this);
                            moved = true;
                            turnState = TurnState.action;
                        } else { randomAttack = ChooseNextAttack(randomAttack); }
                        break;

                    // A4 = *skipped*
                    case RandomAttack.a4:
                        break;
                }
                break;

            case TurnState.action:

                switch (randomAttack) {
                    // Fetch Target and Execute Attack
                    case RandomAttack.a1:
                        CombatManager.Instance.ExecuteEnemyAttack(aOne, GetEnemyThreeLine(OccupiedTile));
                        break;
                    case RandomAttack.a2:
                        CombatManager.Instance.ExecuteEnemyAttack(aTwo, GetRandomTeamMember(this));
                        break;
                    case RandomAttack.a3:
                        CombatManager.Instance.ExecuteEnemyAttack(aThree, GetEnemyFrontLine());
                        break;
                    case RandomAttack.a4:
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
