using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum CurrentScene {
    MainMenu,
    Map,
    HeroSelect,
    Combat,
    InBetween,
    Shop,
    Puzzle,
    Shrine,
    Tomb,
    Well
}


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public CurrentScene currentScene;
    public GameState initState;
    public string nextScene;

    public RoomVariant nextRoom;
    public EnemyGroup nextEnemies;


    void Awake() {
        if (Instance == null) {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        currentScene = CurrentScene.MainMenu;
    }

    public async void LoadScene(string sceneName) {

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        await Task.Delay(1500);

        if (scene.progress >= 0.9f) {
            scene.allowSceneActivation = true;
            if (GameManager.Instance != null) {
                GameManager.Instance.UpdateGameState(initState);
            }
            SetCurrentScene(sceneName);
        }

    }

    public async void LoadSelectedScene() {

        var scene = SceneManager.LoadSceneAsync(nextScene);
        scene.allowSceneActivation = false;

        await Task.Delay(1500);

        if (scene.progress >= 0.9f) {
            scene.allowSceneActivation = true;
            SetCurrentScene(nextScene);
            if (GameManager.Instance != null) {
                GameManager.Instance.UpdateGameState(initState);
            }
        }

    }


    private void SetCurrentScene(string sceneName) {
        switch (sceneName) {
            case "MainMenuScene":
                currentScene = CurrentScene.MainMenu;
                print("SWITCH: " + sceneName);
                break;
            case "MapScene":
                currentScene = CurrentScene.Map;
                print("SWITCH: " + sceneName);
                break;
            case "HeroSelectScene":
                currentScene = CurrentScene.HeroSelect;
                print("SWITCH: " + sceneName);
                break;
            case "CombatScene":
                MapManager.Instance.transform.GetChild(0).GetComponent<Canvas>().gameObject.SetActive(false);
                currentScene = CurrentScene.Combat;
                initState = GameState.PickHeros;
                print("SWITCH: " + sceneName);
                break;
            case "InBetweenScene":
                MapManager.Instance.transform.GetChild(0).GetComponent<Canvas>().gameObject.SetActive(false);
                currentScene = CurrentScene.InBetween;
                initState = GameState.Inactive;
                print("SWITCH: " + sceneName);
                break;
            case "ShopScene":
                MapManager.Instance.transform.GetChild(0).GetComponent<Canvas>().gameObject.SetActive(false);
                currentScene = CurrentScene.Shop;
                initState = GameState.Inactive;
                print("SWITCH: " + sceneName);
                break;
            case "ShrineScene":
                MapManager.Instance.transform.GetChild(0).GetComponent<Canvas>().gameObject.SetActive(false);
                currentScene = CurrentScene.Shrine;
                initState = GameState.Inactive;
                print("SWITCH: " + sceneName);
                break;
            case "PuzzleScene":
                MapManager.Instance.transform.GetChild(0).GetComponent<Canvas>().gameObject.SetActive(false);
                currentScene = CurrentScene.Puzzle;
                initState = GameState.Inactive;
                print("SWITCH: " + sceneName);
                break;
            case "TombChamberScene":
                MapManager.Instance.transform.GetChild(0).GetComponent<Canvas>().gameObject.SetActive(false);
                currentScene = CurrentScene.Tomb;
                initState = GameState.Inactive;
                print("SWITCH: " + sceneName);
                break;
            case "HealingWellScene":
                MapManager.Instance.transform.GetChild(0).GetComponent<Canvas>().gameObject.SetActive(false);
                currentScene = CurrentScene.Well;
                initState = GameState.Inactive;
                print("SWITCH: " + sceneName);
                break;
            default:
                print("ERROR: Cannot resolve SceneName");
                break;

        }
    }

    public void SelectNextScene(RoomType type) {
        switch (type) {
            case RoomType.Fight:
                nextScene = "CombatScene";
                break;
            case RoomType.Shop:
                nextScene = "ShopScene";
                break;
            case RoomType.Shrine:
                nextScene = "ShrineScene";
                break;
            case RoomType.Tomb:
                nextScene = "TombChamberScene";
                break;
            case RoomType.Well:
                nextScene = "HealingWellScene";
                break;
            default:
                nextScene = "InBetweenScene";
                break;
        }
    }

}
