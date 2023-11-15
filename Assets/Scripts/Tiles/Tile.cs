using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{

    [SerializeField] public GameObject _highlight;
    [SerializeField] public GameObject _TargetHighlight;
    [SerializeField] public GameObject _AoeTargetHighlight;
    [SerializeField] public GameObject _activeCharacterHighlight;
    [SerializeField] private bool _isWalkable = true;

    public int xVector;
    public int yVector;
    public bool validMove = false;
    public bool isTargeted = false;
    public bool isObjectTile = false;
    public bool isEffectTile = false;

    public BaseUnit OccupiedUnit;
    public bool Walkable => _isWalkable && OccupiedUnit == null;

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
        GuiManager.Instance.ShowTileInfo(this);

    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
        GuiManager.Instance.ShowTileInfo(CombatManager.Instance.GetActiveHeroTile());
        
    }

    void OnMouseDown()
    {

        if (GameManager.Instance.State != GameState.PlayerTurn) return;

        //Clicked on Unit
        if (OccupiedUnit != null)
        {
            //Clicked on Hero
            if (OccupiedUnit.Faction == Faction.Hero)
            {
                //Clicked Hero is Active Hero
                if (OccupiedUnit == CombatManager.Instance.activeHero)
                {
                    //Trigger Self Heal / Buff / Block
                    if (CombatManager.Instance.activeAttack != null) {
                        if (CombatManager.Instance.activeAttack.actionTeam != ActionTeam.Enemy && CombatManager.Instance.activeAttack.actionTeam != ActionTeam.TeamOnly) {
                            var self = (BaseHero)OccupiedUnit;
                            CombatManager.Instance.TriggerActiveAttack(self);
                            SpawnManager.Instance.SetSelectedHero(null);
                            GridManager.Instance.removeValidMove();
                        } else {
                            EventTextManager.Instance.NewWarningText("Invalid Target!");
                        }
                    //Deselect Active Hero
                    } else if (SpawnManager.Instance.SelectedHero != null)
                    {
                        GridManager.Instance.removeValidMove();
                        SpawnManager.Instance.SetSelectedHero(null);
                    //Select Active Hero
                    } else if (!OccupiedUnit.AppliedEffects.ContainsKey(EffectType.Constrained)) {
                        SpawnManager.Instance.SetSelectedHero((BaseHero)OccupiedUnit);
                        ShowMoveOptions();
                    } else { }
                //Clicked Hero other than Active Hero
                } else
                {
                    //There is not a selected Hero => Not in MoveMode
                    if (CombatManager.Instance.activeHero != null)
                    {
                        var coHero = (BaseHero)OccupiedUnit;
                        CombatManager.Instance.TriggerActiveAttack(coHero);
                        SpawnManager.Instance.SetSelectedHero(null);
                        GridManager.Instance.removeValidMove();
                    }
                }
                

            }
            //Clicked on an Enemy
            else if (OccupiedUnit.Faction == Faction.Enemy)
            {
                //There is not a selected Hero => Not in MoveMode
                if (CombatManager.Instance.activeHero != null)
                {
                    var enemy = (BaseEnemy)OccupiedUnit;
                    CombatManager.Instance.TriggerActiveAttack(enemy);
                    SpawnManager.Instance.SetSelectedHero(null);
                    GridManager.Instance.removeValidMove();
                }
            } 
            //Clicked on an Object
            else {
                if (CombatManager.Instance.activeHero != null) {
                    EventTextManager.Instance.NewWarningText("You cannot attack objects");
                } else {
                    EventTextManager.Instance.NewWarningText("You cannot move into objects");
                }
                GridManager.Instance.removeValidMove();
            }
        }
        //Clicked on Empty Tile
        else
        {
            //There is a selected Hero
            if (SpawnManager.Instance.SelectedHero != null)
            {
                if (CombatManager.Instance.activeHero.CheckEnergyCost(1))
                {
                    if (validMove)
                    {
                        CombatManager.Instance.activeHero.RemoveEnergy(1);
                        SetUnit(SpawnManager.Instance.SelectedHero);
                    }
                }
                SpawnManager.Instance.SetSelectedHero(null);
                GridManager.Instance.removeValidMove();
            }
        }
    }

    public void ShowMoveOptions()
    {
        _activeCharacterHighlight.SetActive(false);
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Tile tileOption = GridManager.Instance.GetTileAtPosition(new Vector2(xVector + i, yVector + j));
                if (tileOption != null && tileOption._isWalkable && tileOption.OccupiedUnit == null)
                {
                    tileOption._activeCharacterHighlight.SetActive(true);
                    tileOption.validMove = true;
                }
            }
        }
    }

    public void SetUnit(BaseUnit unit, bool animated = true)
    {
        if (unit.OccupiedTile != null)
        {
            unit.OccupiedTile.OccupiedUnit = null;
            unit.OccupiedTile._activeCharacterHighlight.SetActive(false);
        }
        if (animated) {
            LeanTween.move(unit.gameObject, (transform.position + new Vector3(unit.XOffset, unit.YOffset, 0.0f)), 2f).setEase(LeanTweenType.easeOutExpo);
        } else {
            unit.transform.position = transform.position + new Vector3(unit.XOffset, unit.YOffset, 0.0f);
        }

        OccupiedUnit = unit;
        unit.OccupiedTile = this;
        if(OccupiedUnit == CombatManager.Instance.activeHero)
        {
            _activeCharacterHighlight.SetActive(true);
        }
        if (unit.Faction == Faction.Hero && unit.AppliedEffects.ContainsKey(EffectType.Swiftness)) {
            unit.AppliedEffects[EffectType.Swiftness] -= 1;
            if (unit.AppliedEffects[EffectType.Swiftness] < 1)
            {
                unit.AppliedEffects.Remove(EffectType.Swiftness);
            }
            unit.AddEnergy(1);
        }

        if (isEffectTile) {
            switch (((EffectTile)this).effectType) {
                case "spike":
                    unit.TakeDamage(5);
                    EventTextManager.Instance.NewEventText(unit.UnitName + " received Damage from Spikes");
                    break;
                default:
                    break;
            }
        }
    }
}