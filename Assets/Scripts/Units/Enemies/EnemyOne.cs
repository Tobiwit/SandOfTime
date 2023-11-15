using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyOne : BaseEnemy
{

    public BaseAttack strike;

    public override void TurnLogic()
    {
        turnState = TurnState.preAction;
        BaseUnit potTarget;
        while (turnState != TurnState.idle)
        {
            switch (turnState)
            {
                case TurnState.idle:
                    break;
                case TurnState.preAction:
                    turnState = TurnState.action;
                    break;
                case TurnState.action:
                    potTarget = GetRandomTarget();
                    CombatManager.Instance.ExecuteEnemyAttack(strike, potTarget);
                    turnState = TurnState.idle;
                    break;
                case TurnState.secondAction:
                    break;
                case TurnState.postMovement:
                    break;
            }
        }
    }
}