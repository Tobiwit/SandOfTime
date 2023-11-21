using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New BaseAttack", menuName = "Attack System/new Attack")]
public class BaseAttack : ScriptableObject
{
    public string displayName;
    public GameObject prefab;
    [TextArea(15, 20)]
    public string description;

    public int energyCost;
    public ActionType actionType;
    public ActionTeam actionTeam;
    public int actionMin;
    public int actionMax;
    public int actionAmount = 1;
    public PlayerClass playerClass;
    public LineType lineType;

    public BaseTargetPattern targetPattern;
    public Sprite skillImage = null;

    public bool applyEffect;
    public EffectSelector effectSelector;
    public EffectType effectType;
    public int effectAmount;

    public EffectSelector secEffectSelector;
    public EffectType secEffectType;
    public int secEffectAmount;

    public bool applySpecialFunction;
    public string CardID;
}

public enum ActionType
{
    Attack,
    Block,
    Heal,
    BuffOnly,
    DebuffOnly,
    OnlyTargetSelf,
    None
}

public enum ActionTeam {
    Enemy,
    TeamOnly,
    SelfOnly,
    TeamAndSelf,
    Everybody
}

public enum PlayerClass
{
    All,
    Archeologist,
    Tombraider,
    Guard,
    Priestress,
    Slave,
    Scribe,
    Merchant
}

public enum LineType
{
    Both,
    Front,
    Back
}

public enum SelectionType
{
    Target,
    AOE,
    Self,
    TeamTarget,
    TeamAOE
}

public enum EffectSelector
{
    None,
    Self,
    Target
}

public enum EffectType
{
    Any,
    Survival,
    Poisioned,
    Burning,
    Strength,
    Hasted,
    Invisible,
    Stunned,
    Constrained,
    Weakened,
    Enraged,
    Stressed,
    Evasion,
    Taunt,
    Adrenaline,
    Swiftness,
    CritUp,
    Shocked
}
