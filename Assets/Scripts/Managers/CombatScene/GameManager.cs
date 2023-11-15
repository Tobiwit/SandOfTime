using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateGameState(GameState.PickHeros);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState) {
            case GameState.PickHeros:
                //MenuManager.Instance.ShowHeroSelectPanel(true);
                UnitManager.Instance.SelectHero(2);
                break;
            case GameState.InitializeGrid:
                //MenuManager.Instance.ShowHeroSelectPanel(false);
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.SpawnPlayers:
                //DeckManager.Instance.AddHeroStarterDecksToCurrentDeck();
                SpawnManager.Instance.SpawnHeroes();
                break;
            case GameState.SpawnEnemies:
                SpawnManager.Instance.SpawnEnemies();
                break;
            case GameState.CreateTurnOrder:
                AttacksDisplayManager.Instance.CreateDisplay();
                CombatManager.Instance.SetupTurnOrder();
                break;
            case GameState.PlayerTurn:
                break;
            case GameState.EnemyTurn:
                CombatManager.Instance.EnemyTurnAI();
                Invoke("AdvanceEnemyTurn", 2);
                break;
            case GameState.Victory:
                GridManager.Instance.MoveCameraX(20f);
                LevelManager.Instance.LoadScene("InBetweenScene");
                break;
            case GameState.Lose:
                GridManager.Instance.MoveCameraX(-20f);
                break;
            case GameState.Inactive:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);

    }


    private void AdvanceEnemyTurn()
    {
        if (CombatManager.Instance.activeEnemy != null)
        {
            CombatManager.Instance.activeEnemy.OccupiedTile._activeCharacterHighlight.SetActive(false);
            CombatManager.Instance.activeEnemy.CurrentBlock = 0;
            CombatManager.Instance.activeEnemy = null;
        }
        CombatManager.Instance.AdvanceTurn();

    }


}


public enum GameState
{
    PickHeros,
    InitializeGrid,
    SpawnPlayers,
    SpawnEnemies,
    CreateTurnOrder,
    PlayerTurn,
    EnemyTurn,
    Victory,
    Lose,
    Inactive
}
