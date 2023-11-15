using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class MapTile : MonoBehaviour
{
    public int coorX = 0;
    public int coorY = 0;
    public int connections = 0;
    public bool explored = false;
    public MapTile n_connect = null;
    public MapTile e_connect = null;
    public MapTile s_connect = null;
    public MapTile w_connect = null;
    public GeneratorTeam team = GeneratorTeam.None;
    public RoomType type = RoomType.None;
    public EnemyGroup enemyGroup = null;
    public RoomVariant roomVariant = null;


    public void ButtonClick() {
        
        this.GetComponent<Image>().color = new Color(0.55f, 0f, 0f, 1f);
        explored = true;

        MapManager.Instance.DeselectAllTilesExcept(this);
        MapManager.Instance.SetActiveTile(this);

        //Transfering Input to LevelManager:

        if (type == RoomType.Fight || type == RoomType.BossFight) {
            LevelManager.Instance.nextRoom = roomVariant;
            LevelManager.Instance.nextEnemies = enemyGroup;
        }

        LevelManager.Instance.SelectNextScene(type);
        

        /*
        MapManager.Instance.HandleButtonClick(new Vector2(coorX, coorY)); 
        if (n_connect != null) {
            n_connect.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0f, 0f, 0f, 1f);
        }
        if (e_connect != null) {
            e_connect.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0f, 0f, 0f, 1f);
        }
        if (s_connect != null) {
            s_connect.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0f, 0f, 0f, 1f);
        }
        if (w_connect != null) {
            w_connect.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0f, 0f, 0f, 1f);
        }*/
    }

    public void UnveilNewTiles() {
        List<MapTile> mapTileConnections = GetAllConnectedTiles();
        int notExploredTiles = 0;

        foreach (MapTile tile in mapTileConnections) {
            if (tile.explored == false) notExploredTiles++;
        }
        if (mapTileConnections.Count == notExploredTiles) return;

        foreach (MapTile tile in mapTileConnections) {
            tile.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
    }

    public void GenerateRoom(float aproxDifficulty) {
        List<RoomVariant> _roomVariantList = Resources.LoadAll<RoomVariant>("RoomGeneration").ToList();
        roomVariant = _roomVariantList.OrderBy(o => Random.value).First();
        float difficultyMultiplyer = roomVariant.difficultyMultiplyer;
        List<EnemyGroup> _enemyGroupList = Resources.LoadAll<EnemyGroup>("RoomGeneration").ToList();
        if (type == RoomType.Fight) {
            enemyGroup = _enemyGroupList.Where(u => !u.isBossfight && u.difficulty * difficultyMultiplyer < aproxDifficulty + 5 && u.difficulty * difficultyMultiplyer > aproxDifficulty - 5).OrderBy(o => Random.value).First();
        }
        if (type == RoomType.BossFight) {
            enemyGroup = _enemyGroupList.Where(u => u.isBossfight && u.difficulty * difficultyMultiplyer < aproxDifficulty + 5 && u.difficulty * difficultyMultiplyer > aproxDifficulty - 5).OrderBy(o => Random.value).First();
        }
    }

    public List<MapTile> GetAllConnectedTiles() {
        List<MapTile> tileConnections = new List<MapTile>();
        if (n_connect != null) {
            tileConnections.Add(n_connect);
        }
        if (e_connect != null) {
            tileConnections.Add(e_connect);
        }
        if (s_connect != null) {
            tileConnections.Add(s_connect);
        }
        if (w_connect != null) {
            tileConnections.Add(w_connect);
        }
        return tileConnections;
    }

    public void UpdateTile() {
        GetComponentInChildren<TextMeshProUGUI>().text = type.ToString();
        GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
    }
}
