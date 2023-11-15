using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenSnakeOne : BaseEnemy
{
    public BaseAttack aOne, aTwo;
    public RandomAttack randomAttack;

    public override void TurnLogic()
    {
        StartTurnLogic();
        randomAttack = RandomAttack.a1;

        while (turnState != TurnState.idle)
        {
            if (AppliedEffects.ContainsKey(EffectType.Stunned) || calcCount > 2)
            {
                turnState = TurnState.idle;
            }
            else
            {
                HandleTurns();
            }
        }
        EndTurnLogic();
    }


    private void HandleTurns()
    {
        switch (turnState)
        {
            case TurnState.idle:
                break;
            case TurnState.preAction:

                // Set Probability of Attacks (sum = 100)
                randomAttack = GetRandomAttack(60, 40);
                Tile moveTile = null;
                switch (randomAttack)
                {
                    // A1 = Poision Strike
                    case RandomAttack.a1:
                        moveTile = ShouldIMove(TargetingType.TargetRestricted, AOEType.OneLine, LineType.Back);
                        if (moveTile == OccupiedTile)
                        {
                            turnState = TurnState.action;
                        }
                        else if (moveTile != null)
                        {
                            moveTile.SetUnit(this);
                            moved = true;
                            turnState = TurnState.action;
                        }
                        else { randomAttack = ChooseNextAttack(randomAttack); }
                        break;

                    // A2 = Bite - "Strike"
                    case RandomAttack.a2:
                        moveTile = ShouldIMove(TargetingType.TargetRestricted,AOEType.None,LineType.Front);
                        if (moveTile == OccupiedTile)
                        {
                            turnState = TurnState.action;
                        }
                        else if (moveTile != null)
                        {
                            moveTile.SetUnit(this);
                            moved = true;
                            turnState = TurnState.action;
                        }
                        else { randomAttack = ChooseFirstAttack(); }
                        break;

                }
                break;

            case TurnState.action:

                switch (randomAttack)
                {
                    // Fetch Target and Execute Attack
                    case RandomAttack.a1:
                        CombatManager.Instance.ExecuteEnemyAttack(aOne, CheckForEnemyInLine(OccupiedTile));
                        break;
                    case RandomAttack.a2:
                        CombatManager.Instance.ExecuteEnemyAttack(aTwo, GetRandomTarget());
                        break;
                    case RandomAttack.a3:
                        break;
                    case RandomAttack.a4:
                        break;
                }
                turnState = TurnState.idle;
                break;
            case TurnState.postMovement:
                if (moved)
                {
                    turnState = TurnState.idle;
                }
                else
                {
                    Tile move = GetRandomPossibleTile();
                    move.SetUnit(this);
                }
                break;

        }
    }
}
