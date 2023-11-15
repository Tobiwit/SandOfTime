using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SnakeOne : BaseEnemy
{

    public BaseAttack poisionSpit, bite;
    public int rand = 0;
    public BaseUnit potTarget;

    public override void TurnLogic()
    {
        StartTurnLogic();
        rand = Random.Range(1, 10);
        while (turnState != TurnState.idle) {
            // Change calcCount to Amount of different Attacks
            if (AppliedEffects.ContainsKey(EffectType.Stunned) || calcCount > 1) {
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
                potTarget = CheckForEnemyInLine(OccupiedTile);
                if (rand < 5)
                {
                    if (potTarget != null)
                    {
                        if (InBackLane())
                        {
                            turnState = TurnState.action;
                            break;
                        }
                        else
                        {
                            if (BackStepPossible())
                            {
                                MakeBackStep();
                                turnState = TurnState.action;
                                break;
                            }
                            else
                            {
                                rand = 9;
                                break;
                            }
                        }

                    }
                    else
                    {
                        if (SideStepPossible())
                        {
                            MakeSideStep();
                            if (InBackLane())
                            {
                                potTarget = CheckForEnemyInLine(OccupiedTile);
                                if (potTarget != null)
                                {
                                    turnState = TurnState.action;
                                    break;
                                }
                                else
                                {
                                    rand = 10;
                                    break;
                                }
                                
                            }
                            else
                            {
                                potTarget = CheckForEnemyInLine(OccupiedTile);
                                if (potTarget != null)
                                {
                                    break;
                                }
                                else
                                {
                                    rand = 10;
                                    break;
                                }

                            }
                        }
                        else
                        {
                            rand = 10;
                            break;
                        }
                    }

                }
                else
                {
                    if (InBackLane() && FrontStepPossible())
                    {
                        MakeFrontStep();
                        turnState = TurnState.action;
                        break;
                    }
                    else if (!InBackLane())
                    {
                        turnState = TurnState.action;
                        break;
                    }
                    else
                    {
                        if (SideStepPossible())
                        {
                            MakeSideStep();
                        }
                        turnState = TurnState.action;
                        break;
                    }
                }
            case TurnState.action:
                List<BaseUnit> tempTurnOrder = CombatManager.Instance.turnOrder;
                if (rand < 5)
                {
                    CombatManager.Instance.ExecuteEnemyAttack(poisionSpit, potTarget);
                }
                else if (!InBackLane())
                {
                    if (potTarget != null)
                    {
                        CombatManager.Instance.ExecuteEnemyAttack(bite, potTarget);
                    }
                    else
                    {
                        potTarget = GetRandomTarget();
                        CombatManager.Instance.ExecuteEnemyAttack(bite, potTarget);
                    }

                }
                turnState = TurnState.postMovement;
                break;
            case TurnState.postMovement:
                int rand2 = Random.Range(0, 10);
                turnState = TurnState.idle;
                break;

        }
    }
}