using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpionOne : BaseEnemy
{
    public BaseUnit rockPrefab;
    public BaseAttack poisionSpit, bite;
    public int rand = 0;
    public BaseUnit potTarget;

    public override void TurnLogic()
    {
        StartTurnLogic();
        rand = Random.Range(1, 10);
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


    private void HandleTurns()
    {
        switch (turnState)
        {
            case TurnState.idle:
                break;
            case TurnState.preAction:
                turnState = TurnState.action;
                break;
            case TurnState.action:
                if (GetFreeMiddleTileIfPossible() != null)
                {
                    Tile tile = GetFreeMiddleTileIfPossible();
                    var rockUnit = Instantiate(rockPrefab);
                    tile.SetUnit(rockUnit);
                }
                turnState = TurnState.idle;
                break;
            case TurnState.postMovement:
                break;

        }
    }
}
