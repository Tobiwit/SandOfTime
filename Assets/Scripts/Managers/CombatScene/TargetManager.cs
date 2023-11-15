using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance;
    public List<Tile> targetedTiles;


    private void Awake()
    {
        Instance = this;
    }

    public void ShowTargetedTiles(BaseAttack attack, BaseUnit unit)
    {
        if (GameManager.Instance.State != GameState.PlayerTurn)
        {
            return;
        }
        HideTargetedTiles();
        if (attack.targetPattern.targetingType == TargetingType.Target)
        {
            return;
        }
        targetedTiles = GetTargetedTiles(attack, unit);
        BaseUnit hitableUnitInLine = GetTargetedInLineUnit(unit);
        foreach (Tile tile in targetedTiles)
        {
            if (attack.targetPattern.targetingType == TargetingType.FixedAOE)
            {
                tile._TargetHighlight.SetActive(true);
                tile._AoeTargetHighlight.SetActive(true);
                tile.isTargeted = true;
            } else if (attack.targetPattern.targetingType == TargetingType.TargetRestricted)
            {
                if (attack.targetPattern.canBeBlocked)
                {
                    if (tile.OccupiedUnit != null && tile.OccupiedUnit == hitableUnitInLine)
                    {
                        SetTileHitable(tile);
                    } else
                    {
                        SetTileUnhitable(tile);
                    }
                    
                } else
                {
                    if (tile.OccupiedUnit != null)
                    {
                        SetTileHitable(tile);
                    } else
                    {
                        SetTileUnhitable(tile);
                    }
                }
            } else
            {
                tile.isTargeted = true;
            }
            
        }
    }

    private void SetTileHitable(Tile tile)
    {
        tile._AoeTargetHighlight.SetActive(true);
        tile._AoeTargetHighlight.GetComponent<SpriteRenderer>().color = Color.white;
        tile.isTargeted = true;
    }

    private void SetTileUnhitable(Tile tile)
    {
        tile._AoeTargetHighlight.SetActive(true);
        tile._AoeTargetHighlight.GetComponent<SpriteRenderer>().color = Color.black;
    }


    public BaseUnit GetTargetedInLineUnit(BaseUnit unit)
    {
        int y = unit.OccupiedTile.yVector;
        int attackerX = unit.OccupiedTile.xVector;
        BaseUnit hitableUnit = null;
        for (int x = GridManager.Instance._width - 1; x > attackerX; x--)
        {
            Tile tile = GridManager.Instance.GetTileAtPosition(new Vector2(x, y));
            if (tile.OccupiedUnit != null && !tile.OccupiedUnit.AppliedEffects.ContainsKey(EffectType.Invisible))
            {
                hitableUnit = tile.OccupiedUnit;
            }
        }
        if (hitableUnit != null)
        {
            return hitableUnit;
        }

        return null;
    }

    public void HideTargetedTiles()
    {
        GridManager.Instance.removeTargetedTiles();
        targetedTiles.Clear();
    }

    public List<Tile> GetTargetedTiles(BaseAttack attack, BaseUnit unit)
    {
        List<Tile> targetedTiles = new List<Tile>();
        var pattern = attack.targetPattern;
        Vector2 unitTilePos = new Vector2((float)unit.OccupiedTile.xVector,(float)unit.OccupiedTile.yVector);
        if (pattern.targetingType == TargetingType.TargetRestricted || pattern.targetingType == TargetingType.FixedAOE)
        {
            switch (pattern.aoeType) {
                case AOEType.None:
                    break;
                case AOEType.OneLine:
                    for (int i = 0; unitTilePos.x + i < GridManager.Instance._width; i++)
                    {
                        if (unitTilePos.x + i > 1)
                        {
                            Vector2 targetedTilePos = new Vector2(unitTilePos.x + i, unitTilePos.y);
                            targetedTiles.Add(GridManager.Instance.GetTileAtPosition(targetedTilePos));
                        }
                    }
                    break;
                case AOEType.FrontLine:
                    for (int i = 0; i < GridManager.Instance._height; i++)
                    {
                        Vector2 targetedTilePos = new Vector2(GridManager.Instance._width - 2, i);
                        targetedTiles.Add(GridManager.Instance.GetTileAtPosition(targetedTilePos));

                    }
                    break;
                case AOEType.BackLine:
                    for (int i = 0; i < GridManager.Instance._height; i++)
                    {
                        Vector2 targetedTilePos = new Vector2(GridManager.Instance._width - 1, i);
                        targetedTiles.Add(GridManager.Instance.GetTileAtPosition(targetedTilePos));

                    }
                    break;
                case AOEType.SquareSelector:
                    for (int i = 0; i < 3; i++) {
                        for (int j = 0; j < 3; j++) {
                            int x = (int)unitTilePos.x + 1 + j;
                            int y = (int)unitTilePos.y - 1 + i;
                            if (y < 0) {
                                y = 0;
                            } else if (y > GridManager.Instance._height - 1) {
                                y = GridManager.Instance._height - 1;
                            }
                            if (x < 2) {
                                x = 2;
                            } else if (x > GridManager.Instance._width - 1) {
                                x = GridManager.Instance._width - 1;
                            }
                            Vector2 targetedTilePos = new Vector2(x, y);
                            targetedTiles.Add(GridManager.Instance.GetTileAtPosition(targetedTilePos));
                        }
                    }
                    break;
                case AOEType.ThreeLine:
                    for (int i = 0; i < 3; i++)
                    {
                        int x = (int) unitTilePos.x + 2;
                        int y = (int) unitTilePos.y - 1 + i;
                        if (y < 0) {
                            y = 0;
                        } else if (y > GridManager.Instance._height-1)
                        {
                            y = GridManager.Instance._height - 1;
                        }
                        Vector2 targetedTilePos = new Vector2(x,y);
                        targetedTiles.Add(GridManager.Instance.GetTileAtPosition(targetedTilePos));

                    }
                    break;
                case AOEType.ThreeOneLine:
                    break;
                case AOEType.Star:
                    for (int i = 0; i < 3; i++) {
                        int x = (int)unitTilePos.x + 3;
                        int y = (int)unitTilePos.y - 1 + i;
                        if (y < 0) {
                            y = 0;
                        } else if (y > GridManager.Instance._height - 1) {
                            y = GridManager.Instance._height - 1;
                        }
                        Vector2 targetedTilePos = new Vector2(x, y);
                        targetedTiles.Add(GridManager.Instance.GetTileAtPosition(targetedTilePos));

                    }
                    if (unitTilePos.x == 0) {
                        Vector2 targetedTilePos1 = new Vector2(2, unitTilePos.y);
                        targetedTiles.Add(GridManager.Instance.GetTileAtPosition(targetedTilePos1));
                        Vector2 targetedTilePos2 = new Vector2(4, unitTilePos.y);
                        targetedTiles.Add(GridManager.Instance.GetTileAtPosition(targetedTilePos2));
                    } else if (unitTilePos.x == 1) {
                        Vector2 targetedTilePos3 = new Vector2(3, unitTilePos.y);
                        targetedTiles.Add(GridManager.Instance.GetTileAtPosition(targetedTilePos3));
                    }
                    break;
                case AOEType.Close:
                    int x1 = (int)unitTilePos.x;
                    int y1 = (int)unitTilePos.y;
                    if (x1 == 0) {
                        Vector2 targetedTilePos = new Vector2(1, y1);
                        targetedTiles.Add(GridManager.Instance.GetTileAtPosition(targetedTilePos));
                    } else {
                        Vector2 targetedTilePos = new Vector2(0, y1);
                        targetedTiles.Add(GridManager.Instance.GetTileAtPosition(targetedTilePos));
                    }
                    if (y1 > 0) {
                        Vector2 targetedTilePos = new Vector2(x1, y1 - 1);
                        targetedTiles.Add(GridManager.Instance.GetTileAtPosition(targetedTilePos));
                    }
                    if (y1 < GridManager.Instance._height - 1) {
                        Vector2 targetedTilePos = new Vector2(x1, y1 + 1);
                        targetedTiles.Add(GridManager.Instance.GetTileAtPosition(targetedTilePos));
                    }
                    break;
                case AOEType.OneRandomTile:
                    int randY = Random.Range(0, GridManager.Instance._height);
                    int randX = Random.Range(0, GridManager.Instance._width);

                    targetedTiles.Add(GridManager.Instance.GetTileAtPosition(new Vector2(randX, randY)));

                    break;
            }
        }
        return targetedTiles;
    }
}
