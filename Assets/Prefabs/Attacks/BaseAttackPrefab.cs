using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttackPrefab : MonoBehaviour
{
    [SerializeField] public GameObject _highlight;
    public BaseAttack attackObject;
    public int arrayNum;
    public bool active;
    public bool hovered;
    public int specialHeroID;

    public void SetScript(BaseAttack attack)
    {
        attackObject = attack;
    }

    public void SetArrayNum(int number) {
        arrayNum = number;
    }

    public void HandleClickEvent()
    {
        if (active)
        {
            active = false;
            CombatManager.Instance.SetActiveAttack(null, 0);
        }
        else
        {
            AttacksDisplayManager.Instance.DeactivateAllAttacksExcept(attackObject, arrayNum);
            active = true;
            _highlight.SetActive(true);
            if (CombatManager.Instance.CheckIfAttackPossible(attackObject))
            {
                CombatManager.Instance.SetActiveAttack(attackObject, arrayNum);
            }
            else
            {
                CombatManager.Instance.activeAttack = null;
                //ErrorMessage
            }
        }
    }

    public void Update()
    {
        if(!active && !hovered)
        {
            _highlight.SetActive(false);
        }
    }

    public void SetAttackActivity(bool isActive)
    {
        active = isActive;
        //GuiManager.Instance.ShowAttackInfoDisplay(isActive);
    }

    public void OnEnterHovering()
    {
        hovered = true;
        TargetManager.Instance.HideTargetedTiles();
        GuiManager.Instance.ShowAttackInfoDisplay(true);
        GuiManager.Instance.EditAttackInfoDisplay(attackObject);
        _highlight.SetActive(true);
        TargetManager.Instance.ShowTargetedTiles(attackObject, CombatManager.Instance.activeHero);
    }

    public void OnExitHovering()
    {
        if (!active)
        {
            if (CombatManager.Instance.activeAttack == null)
            {
                GuiManager.Instance.ShowAttackInfoDisplay(false);
                TargetManager.Instance.HideTargetedTiles();
            } else
            {
                GuiManager.Instance.EditAttackInfoDisplay(CombatManager.Instance.activeAttack);
                //TargetManager.Instance.HideTargetedTiles();
                TargetManager.Instance.ShowTargetedTiles(CombatManager.Instance.activeAttack, CombatManager.Instance.activeHero);
            }
            _highlight.SetActive(false);
        }
        hovered = false;
    }

}
