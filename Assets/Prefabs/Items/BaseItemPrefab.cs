using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItemPrefab : MonoBehaviour
{
    [SerializeField] public GameObject _highlight;
    public BaseItem item;
    public bool active;
    public bool hovered;

    public void SetData(BaseItem itemScript)
    {
        item = itemScript;
    }
    public void HandleClickEvent()
    {
        Debug.Log("Clicked" + item.displayName);
    }

    /*
    public void Update()
    {
        if(!active && !hovered)
        {
            _highlight.SetActive(false);
        }
    }
    */

    public void SetAttackActivity(bool isActive)
    {
        active = isActive;
        //GuiManager.Instance.ShowAttackInfoDisplay(isActive);
    }

    public void OnEnterHovering()
    {
    }

    public void OnExitHovering()
    {
    }
}
