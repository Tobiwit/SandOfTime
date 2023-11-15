using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckAttackPrefab : MonoBehaviour
{
    [SerializeField] public GameObject _highlight;
    public BaseAttack attackObject;
    public bool active;
    public bool hovered;
    public int index;

    public void SetScript(BaseAttack attack) {
        attackObject = attack;
    }

    public void HandleClickEvent() {
        if (active) {
            active = false;
            CardsManager.Instance.selectedCardIndex = -1;
        } else {
            CardsManager.Instance.DeactivateAllAttacksExcept(index);
            active = true;
            _highlight.SetActive(true);
            CardsManager.Instance.selectedCardIndex = index;
        }
    }

    public void Update() {
        if (!active && !hovered) {
            _highlight.SetActive(false);
        }
    }

    public void SetAttackActivity(bool isActive) {
        active = isActive;
    }

    public void OnEnterHovering() {
        hovered = true;

        CardsManager.Instance.EditAttackInfoDisplay(attackObject);
        _highlight.SetActive(true);
    }

    public void OnExitHovering() {
        if (!active) {
            _highlight.SetActive(false);
        }
        hovered = false;
    }
}
