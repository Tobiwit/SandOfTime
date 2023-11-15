using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New TargetPattern", menuName = "Attack System/new Target Pattern")]
public class BaseTargetPattern : ScriptableObject
{
    public TargetingType targetingType;

    public AOEType aoeType;

    public bool autoRelease;
    public bool canBeBlocked;

}

public enum TargetingType
{
    Target,
    TargetRestricted,
    FixedAOE
}

public enum AOEType
{
    None,
    OneLine,
    FrontLine,
    BackLine,
    SquareSelector,
    ThreeLine,
    ThreeOneLine,
    Star,
    Close,
    OneRandomTile
}
