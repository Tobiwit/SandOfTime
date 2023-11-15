using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum TurnState
{
    idle,
    preAction,
    action,
    secondAction,
    postMovement

}

public enum RandomAttack
{
    a1,
    a2,
    a3,
    a4
}

public class BaseEnemy : BaseUnit
{
    public TurnState turnState = TurnState.idle;
    public List<Tile> resultTile = new List<Tile>();
    public int calcCount = 0;
    public bool moved = false;

    public virtual void TurnLogic() { }

    public RandomAttack GetRandomAttack(int i, int j = 0, int k = 0, int l = 0)
    {
        j = i + j;
        k = j + k;
        l = k + l;
        int rand = Random.Range(0, 100);
        if (rand < i)
        {
            return RandomAttack.a1;
        }
        else if (rand >= i && rand < j)
        {
            return RandomAttack.a2;
        }
        else if (rand >= j && rand < k)
        {
            return RandomAttack.a3;
        }
        else if (rand >= k && rand < l)
        {
            return RandomAttack.a4;
        }
        else { return RandomAttack.a1; }
    }

    public Tile ShouldIMove(TargetingType type, AOEType pattern = AOEType.None, LineType linePosition = LineType.Both, int range = 1)
    {
        if (type == TargetingType.Target) { return OccupiedTile; }

        BaseUnit tauntTarget = CombatManager.Instance.turnOrder.Where(t => t.Faction == Faction.Hero && !t.AppliedEffects.ContainsKey(EffectType.Taunt)).OrderBy(t => Random.value).FirstOrDefault();
        resultTile.Clear();
        List<Tile> posMoves = GetAllPossibleMoves(OccupiedTile);
        if (AppliedEffects.ContainsKey(EffectType.Constrained))
        {
            posMoves.Clear();
        }
        posMoves.Add(OccupiedTile);
        //List<Tile> resultTile = new List<Tile>();
        foreach (Tile tile in posMoves)
        {
            if (linePosition == LineType.Front && tile.xVector == 4) { }
            else if (linePosition == LineType.Back && tile.xVector == 3) { }
            else
            {
                BaseUnit unit = CheckForPotentialTargets(pattern, tile);
                if (unit != null && unit.Faction == Faction.Hero)
                {
                    if (tauntTarget == null)
                    {
                        if (unit == tauntTarget)
                        {
                            resultTile.Add(tile);
                        }
                    } else
                    {
                        resultTile.Add(tile);
                    }
                }
            }
        }
        if (resultTile.Count == 0) { return null; }
        else if (resultTile.Count == 1) { return resultTile[0]; }
        else
        {
            int rIndex = Random.Range(0, resultTile.Count);
            return resultTile[rIndex];
        }

    }

    public BaseUnit CheckForPotentialTargets(AOEType pattern, Tile potAttackerTile)
    {
        switch (pattern) {
            case AOEType.None:
                return GetRandomTarget();
            case AOEType.OneLine:
                return CheckForEnemyInLine(potAttackerTile);
            case AOEType.FrontLine:
                return GetEnemyFrontLine();
            case AOEType.BackLine:
                break;
            case AOEType.SquareSelector:
                break;
            case AOEType.ThreeLine:
                return GetEnemyThreeLine(potAttackerTile);
        }
        return null;
    }

    public List<Tile> GetAllPossibleMoves(Tile currentTile)
    {
        List<Tile> matchedTiles = new List<Tile>();
        int x = currentTile.xVector;
        int y = currentTile.yVector;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Tile tempTile = GridManager.Instance.GetTileAtPosition(new Vector2(x - 1 + i, y - 1 + j));
                if (tempTile != null)
                {
                    if (tempTile.OccupiedUnit == null && tempTile.xVector > 2)
                    {
                        matchedTiles.Add(tempTile);
                    }
                }
            }
        }
        return matchedTiles;
    }

    public Tile GetRandomPossibleTile()
    {
        List<Tile> possibleTiles = GetAllPossibleMoves(OccupiedTile);
        /*int rIndex = Random.Range(0, possibleTiles.Count);
        return possibleTiles[rIndex];*/
        return GetRandomFromList<Tile>(possibleTiles);
    }

    public RandomAttack ChooseNextAttack(RandomAttack rAttack)
    {
        calcCount++;
        switch (rAttack)
        {
            case RandomAttack.a1:
                return RandomAttack.a2;
            case RandomAttack.a2:
                return RandomAttack.a3;
            case RandomAttack.a3:
                return RandomAttack.a4;
            case RandomAttack.a4:
                return RandomAttack.a1;
            default:
                return RandomAttack.a1;
        }
    }

    public RandomAttack ChooseFirstAttack()
    {
        calcCount++;
        return RandomAttack.a1;
    }

    public BaseUnit CheckForEnemyInLine(Tile attackerTile)
    {
        int y = attackerTile.yVector;
        int enemyX = attackerTile.xVector;
        BaseUnit hitableUnit = null;
        for (int x = 0; x < enemyX; x++)
        {
            Tile tile = GridManager.Instance.GetTileAtPosition(new Vector2(x,y));
            if (tile.OccupiedUnit != null && !tile.OccupiedUnit.AppliedEffects.ContainsKey(EffectType.Invisible))
            {
                if (tile.OccupiedUnit != this)
                {
                    hitableUnit = tile.OccupiedUnit;
                }
            }
        }
        if (hitableUnit != null && hitableUnit.Faction == Faction.Hero)
        {
            return hitableUnit;
        }

        return null;
    }

    public Tile GetFreeMiddleTileIfPossible()
    {
        int y = OccupiedTile.yVector;
        Tile tile = GridManager.Instance.GetTileAtPosition(new Vector2(2, y));
        if (tile.OccupiedUnit == null)
        {
            return tile;
        } else
        {
            return null;
        }
    }

    public BaseUnit GetEnemyThreeLine(Tile attackerTile) {
        List<BaseUnit> potTargets = new List<BaseUnit>();
        for (int i = 0; i < 3; i++) {
            int x = (int)attackerTile.xVector - 2;
            int y = (int)attackerTile.yVector - 1 + i;
            if (y < 0) {
                y = 0;
            } else if (y > GridManager.Instance._height - 1) {
                y = GridManager.Instance._height - 1;
            }
            Vector2 targetedTilePos = new Vector2(x, y);
            Tile tile = GridManager.Instance.GetTileAtPosition(targetedTilePos);
            if (tile.OccupiedUnit != null && !tile.OccupiedUnit.AppliedEffects.ContainsKey(EffectType.Invisible) && tile.OccupiedUnit.Faction == Faction.Hero) {
                if (tile.OccupiedUnit != this) {
                    potTargets.Add(tile.OccupiedUnit);
                }
            }
        }
        if (potTargets.Count == 0) { return null; }
        else {
            return GetRandomFromList<BaseUnit>(potTargets);
        } 

    }

    public BaseUnit GetEnemyFrontLine() {
        List<BaseUnit> potTargets = new List<BaseUnit>();
        for (int i = 0; i < GridManager.Instance._height; i++) {
            int x = 1;
            int y = 0 + i;
            Vector2 targetedTilePos = new Vector2(x, y);
            Tile tile = GridManager.Instance.GetTileAtPosition(targetedTilePos);
            if (tile.OccupiedUnit != null && !tile.OccupiedUnit.AppliedEffects.ContainsKey(EffectType.Invisible) && tile.OccupiedUnit.Faction == Faction.Hero) {
                if (tile.OccupiedUnit != this) {
                    potTargets.Add(tile.OccupiedUnit);
                }
            }
        }
        if (potTargets.Count == 0) { return null; } else {
            return GetRandomFromList<BaseUnit>(potTargets);
        }

    }

    public bool SideStepPossible()
    {
        int y = OccupiedTile.yVector;
        if (y-1 >= 0)
        {
            Tile tile = GridManager.Instance.GetTileAtPosition(new Vector2(OccupiedTile.xVector, y-1));
            if (tile.OccupiedUnit == null)
            {
                return true;
            }
        }
        if (y + 1 < GridManager.Instance._height)
        {
            Tile tile = GridManager.Instance.GetTileAtPosition(new Vector2(OccupiedTile.xVector, y+1));
            if (tile.OccupiedUnit == null)
            {
                return true;
            }
        }
        return false;
    }

    public BaseUnit GetRandomTarget()
    {
        BaseUnit randomTarget;
        BaseUnit tauntTarget = CombatManager.Instance.turnOrder.Where(t => t.Faction == Faction.Hero && t.AppliedEffects.ContainsKey(EffectType.Taunt)).OrderBy(t => Random.value).FirstOrDefault();
        if (tauntTarget != null)
        {
            return tauntTarget;
        }
        randomTarget = CombatManager.Instance.turnOrder.Where(t => t.Faction == Faction.Hero && !t.AppliedEffects.ContainsKey(EffectType.Invisible)).OrderBy(t => Random.value).FirstOrDefault();
        return randomTarget;
    }

    public BaseUnit GetRandomTeamMember(BaseUnit self) {
        BaseUnit randomTarget;
        randomTarget = CombatManager.Instance.turnOrder.Where(t => t.Faction == Faction.Enemy && t != self).OrderBy(t => Random.value).FirstOrDefault();
        return randomTarget;
    }

    public T GetRandomFromList<T>(List<T> unitList) {
        int r = Random.Range(0,unitList.Count);
        return unitList[r];
    }

    public void MakeSideStep()
    {
        int y = OccupiedTile.yVector;

        if (!(y - 1 >= 0))
        {
            Tile tile = GridManager.Instance.GetTileAtPosition(new Vector2(OccupiedTile.xVector, y + 1));
            if (tile.OccupiedUnit == null)
            {
                tile.SetUnit(this);
            }
        } else if (!(y + 1 < GridManager.Instance._height))
        {
            Tile tile = GridManager.Instance.GetTileAtPosition(new Vector2(OccupiedTile.xVector, y - 1));
            if (tile.OccupiedUnit == null)
            {
                tile.SetUnit(this);
            }
        } else
        {
            int rand = Random.Range(0, 1);
            if (rand == 0)
            {
                Tile tile = GridManager.Instance.GetTileAtPosition(new Vector2(OccupiedTile.xVector, y - 1));
                if (tile.OccupiedUnit == null)
                {
                    tile.SetUnit(this);
                }
                else
                {
                    Tile tile2 = GridManager.Instance.GetTileAtPosition(new Vector2(OccupiedTile.xVector, y + 1));
                    tile2.SetUnit(this);
                }
            }
            else
            {
                Tile tile = GridManager.Instance.GetTileAtPosition(new Vector2(OccupiedTile.xVector, y + 1));
                if (tile.OccupiedUnit == null)
                {
                    tile.SetUnit(this);
                }
                else
                {
                    Tile tile2 = GridManager.Instance.GetTileAtPosition(new Vector2(OccupiedTile.xVector, y - 1));
                    tile2.SetUnit(this);
                }
            }
        }
    }

    public bool InBackLane()
    {
        if (OccupiedTile.xVector >= (GridManager.Instance._width-1))
        {
            return true;
        } else
        {
            return false;
        }

    }

    public bool FrontStepPossible()
    {
        int x = OccupiedTile.xVector;
        int y = OccupiedTile.yVector;
        Tile tile = GridManager.Instance.GetTileAtPosition(new Vector2(x - 1, y));
        if (tile.OccupiedUnit == null)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public void MakeFrontStep()
    {
        int x = OccupiedTile.xVector;
        int y = OccupiedTile.yVector;
        Tile tile = GridManager.Instance.GetTileAtPosition(new Vector2(x - 1, y));
        if (tile.OccupiedUnit == null)
        {
            tile.SetUnit(this);
        }
    }

    public bool BackStepPossible()
    {
        int x = OccupiedTile.xVector;
        int y = OccupiedTile.yVector;
        if (x+1 <= GridManager.Instance._width)
        {
            Tile tile = GridManager.Instance.GetTileAtPosition(new Vector2(x + 1, y));
            if (tile.OccupiedUnit == null)
            {
                return true;
            }
        }
        return false;
    }

    public void MakeBackStep()
    {
        int x = OccupiedTile.xVector;
        int y = OccupiedTile.yVector;
        Tile tile = GridManager.Instance.GetTileAtPosition(new Vector2(x + 1, y));
        if (tile.OccupiedUnit == null)
        {
            tile.SetUnit(this);
        }
    }

    public void StartTurnLogic() {
        turnState = TurnState.preAction;
        moved = false;
        calcCount = 0;
        CurrentBlock = 0;
    }

    public void EndTurnLogic() {
        //TRIGGERS AFTER THE END OF TURN
    }

}
