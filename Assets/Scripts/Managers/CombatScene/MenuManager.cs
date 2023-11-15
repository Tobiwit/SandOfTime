using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    public static MenuManager Instance;

    [SerializeField] private GameObject _heroSelectPanel;
    [SerializeField] private GameObject _pick1Button;


    void Awake() {
        //GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        Instance = this;
    }

    void OnDestroy() {
        //GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    /*
    private void GameManagerOnGameStateChanged(GameState state) {
        _heroSelectPanel.SetActive(state == GameState.PickHeros);
    }*/

    public void ShowHeroSelectPanel(bool visible) {
        _heroSelectPanel.SetActive(visible);
    }
}
