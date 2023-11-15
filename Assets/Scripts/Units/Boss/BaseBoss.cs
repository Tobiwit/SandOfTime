using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBoss : BaseEnemy
{
    bool multiTileBoss = true;


    public void AddMultiTiles() {
        if (multiTileBoss) {
            Tile tile = this.OccupiedTile;
            print(tile.xVector + " + " + tile.yVector);
            Tile addedTile = GridManager.Instance.GetTileAtPosition(new Vector2(tile.xVector, tile.yVector - 1));
            addedTile.OccupiedUnit = this;
        }
    }
}
