using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTile : Tile
{
    public string effectType;

    void Start()
    {
        isEffectTile = true;
    }
}
