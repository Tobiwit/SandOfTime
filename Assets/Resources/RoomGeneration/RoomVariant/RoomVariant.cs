using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SpecialTile {
    public int rank;
    public int position;
    public Tile tile;
}

[CreateAssetMenu(fileName = "New RoomVariant", menuName = "Room Generation/new RoomVariant")]
public class RoomVariant : ScriptableObject
{
    public int tileSize = 4;
    public float difficultyMultiplyer = 1f;

    
    public List<SpecialTile> specialTileList = new List<SpecialTile>();
}
