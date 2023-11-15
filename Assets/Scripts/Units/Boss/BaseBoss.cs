using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBoss : BaseEnemy
{
    bool multiTileBoss = true;

    public int width;
    public int height;


    public void AddMultiTiles(Tile spawnTile, BaseUnit spawnedPrefab) {
        if (multiTileBoss) {
            print(spawnTile.xVector + " + " + spawnTile.yVector);
            Tile addedTile;
            if (GridManager.Instance._height == spawnTile.yVector) {
                addedTile = GridManager.Instance.GetTileAtPosition(new Vector2(spawnTile.xVector, spawnTile.yVector -1));
                addedTile.OccupiedUnit = spawnedPrefab;
                addedTile = GridManager.Instance.GetTileAtPosition(new Vector2(spawnTile.xVector-1, spawnTile.yVector -1));
                addedTile.OccupiedUnit = spawnedPrefab;
            } else {
                addedTile = GridManager.Instance.GetTileAtPosition(new Vector2(spawnTile.xVector, spawnTile.yVector +1));
                addedTile.OccupiedUnit = spawnedPrefab;
                addedTile = GridManager.Instance.GetTileAtPosition(new Vector2(spawnTile.xVector-1, spawnTile.yVector +1));
                addedTile.OccupiedUnit = spawnedPrefab;
            }
            addedTile = GridManager.Instance.GetTileAtPosition(new Vector2(spawnTile.xVector-1, spawnTile.yVector));
            addedTile.OccupiedUnit = spawnedPrefab;
        }
    }
}
