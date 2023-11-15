using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    [SerializeField] public HeroOne heroOne;

    public int HeroCount = 0;

    void Awake()
    {
        Instance = this;
    }

    public void SelectHero(int heroNumber)
    {
        HeroCount = heroNumber;
        GameManager.Instance.UpdateGameState(GameState.InitializeGrid);
    }

    public int GetHeroCount()
    {
        return HeroCount;
    }

}
