using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectHeroPanelManager : MonoBehaviour
{
    public static SelectHeroPanelManager Instance;
    public GameObject selectedHeroText;

    private void Awake() {
        Instance = this;
    }

    public void UpdateHeroText() {
        List<BaseUnit> units = HeroManager.Instance.selectedHeros;
        string tempText = "";
        foreach (BaseUnit unit in units) {
            tempText += unit.UnitType + "  ";
        }
        selectedHeroText.GetComponent<TextMeshProUGUI>().text = tempText;
    }

    public void HandleChangeSceneButtonInput() {
        DeckManager.Instance.AddHeroStarterDecksToCurrentDeck();
        LevelManager.Instance.LoadScene("MapScene");
    }

}
