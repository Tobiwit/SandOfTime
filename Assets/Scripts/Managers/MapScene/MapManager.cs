using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;


public enum GeneratorTeam {
    None,
    Start,
    End1,
    End2,
    End3,
    Edge
}

public enum RoomType {
    None,
    Start,
    FirstEnd,
    SecondEnd,
    ThirdEnd,
    Fight,
    Shop,
    Shrine,
    Tomb,
    Well,
    BossFight
}

public class MapManager : MonoBehaviour {
    public int mapSize = 6;
    public Dictionary<Vector2, MapTile> mapTiles;
    public List<MapTile> mapTilesList;
    public MapTile tilePrefab;
    public GameObject anker;
    public MapTile edge;
    public bool genFinished;
    public MapTile activeTile;


    public static MapManager Instance;
    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        StartMapGeneration();
    }

    private void StartMapGeneration() {
        mapTiles = new Dictionary<Vector2, MapTile>();
        mapTilesList = new List<MapTile>();
        for (int x = 0; x < mapSize; x++) {
            for (int y = 0; y < mapSize; y++) {
                double _xNew = (double)(x * (32 + 20) * 3) - 100 - (mapSize * 50);
                double _yNew = (double)(y * (32 + 20) * 3) - 100 - (mapSize * 50);
                var spawnedMapTile = Instantiate(tilePrefab, anker.transform.position + new Vector3((float)_xNew, (float)_yNew), Quaternion.identity, anker.transform);
                spawnedMapTile.name = $"MapTile {x} {y}";

                spawnedMapTile.coorX = x;
                spawnedMapTile.coorY = y;

                mapTiles[new Vector2(x, y)] = spawnedMapTile;
                mapTilesList.Add(spawnedMapTile);

                /*
                foreach (Transform child in spawnedMapTile.transform) {
                    child.gameObject.SetActive(true);
                }*/

                if (x == 0) {
                    spawnedMapTile.w_connect = edge;
                }
                if (y == 0) {
                    spawnedMapTile.s_connect = edge;
                }
                if (x == mapSize - 1) {
                    spawnedMapTile.e_connect = edge;
                }
                if (y == mapSize - 1) {
                    spawnedMapTile.n_connect = edge;
                }
            }
        }
        SetGenerationTiles();
    }

    public MapTile GetMapTileAtPosition(Vector2 pos) {
        if (mapTiles.TryGetValue(pos, out var mapTile)) {
            return mapTile;
        } else {
            return null;
        }
    }


    public void SetGenerationTiles() {
        MapTile StartTile = GetMapTileAtPosition(new Vector2(0, 0));
        StartTile.type = RoomType.Start;
        StartTile.team = GeneratorTeam.Start;
        StartTile.GetComponent<Image>().color = new Color(0.55f,0f,0f,1f);
        StartTile.explored = true;
        MapTile FirstEndTile = GetMapTileAtPosition(new Vector2(mapSize - 1, Random.Range(0, mapSize - 1)));
        FirstEndTile.type = RoomType.FirstEnd;
        FirstEndTile.team = GeneratorTeam.End1;
        FirstEndTile.GetComponent<Image>().color = new Color(1f, 0.625f, 0f, 1f);
        MapTile SecondEndTile = GetMapTileAtPosition(new Vector2(Random.Range(0, mapSize - 3), mapSize - 1));
        SecondEndTile.type = RoomType.SecondEnd;
        SecondEndTile.team = GeneratorTeam.End2;
        SecondEndTile.GetComponent<Image>().color = new Color(1f, 0.625f, 0f, 1f);
    }

    public void GenerateMapStep() {
        if (!genFinished) {
            if (mapTilesList.Where(t => t.team == GeneratorTeam.None).FirstOrDefault() == null) {
                MakePathToTeam(GeneratorTeam.End1);
                MakePathToTeam(GeneratorTeam.End2);
                MakeFirstRoomsVisible();
                print("Generation Completed");
                genFinished = true;
                return;
            }
        } else {
            print("Generation already Completed");
            return;
        }
        CreateNewTileConnection(GeneratorTeam.Start);
        CreateNewTileConnection(GeneratorTeam.End1);
        CreateNewTileConnection(GeneratorTeam.End2);
    }

    private void MakeFirstRoomsVisible() {
        MapTile StartTile = GetMapTileAtPosition(new Vector2(0, 0));
        if (StartTile.n_connect != null) {
            StartTile.n_connect.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0f, 0f, 0f, 1f);
        }
        if (StartTile.e_connect != null) {
            StartTile.e_connect.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0f, 0f, 0f, 1f);
        }
    }

    public void CreateNewTileConnection(GeneratorTeam team) {
        List<MapTile> posNewTiles = new List<MapTile>();
        foreach (MapTile tile in mapTilesList) {
            if (tile.team == team) {
                List<MapTile> tilesAround = GetFreeTilesAround(tile);
                foreach (MapTile tempTile in tilesAround) {
                    if (!posNewTiles.Contains(tempTile) && tempTile.team == GeneratorTeam.None) {
                        posNewTiles.Add(tempTile);
                        posNewTiles.Add(tempTile);
                        posNewTiles.Add(tempTile);
                        posNewTiles.Add(tempTile);
                        tempTile.GetComponent<Image>().color = Color.black;
                    } else if (!posNewTiles.Contains(tempTile) && tempTile.team == team) {
                        posNewTiles.Add(tempTile);
                    }
                }

            }
        }
        if (posNewTiles.Where(t => t.team == GeneratorTeam.None).FirstOrDefault() == null) {
            print("Finished Subroutine: " + team);
            return;
        }
        if (posNewTiles.Count > 0) {
            MapTile selTile = posNewTiles.OrderBy(r => Random.value).First();
            List<MapTile> tilesAround = GetFreeTilesAround(selTile);
            List<MapTile> tempResultTile = new List<MapTile>();
            foreach (MapTile tempTile in tilesAround) {
                if (tempTile.team == team) {
                    tempResultTile.Add(tempTile);
                }
            }
            //-----------------------------
            switch (team) {
                case GeneratorTeam.Start:
                    //selTile.GetComponent<Image>().color = Color.red;
                    //selTile.GetComponent<Image>().color = new Color(1f, 0f, 0f, 0.5f);
                    break;
                case GeneratorTeam.End1:
                    //selTile.GetComponent<Image>().color = Color.blue;
                    //selTile.GetComponent<Image>().color = new Color(0f, 0f, 1f, 0.5f);
                    break;
                case GeneratorTeam.End2:
                    //selTile.GetComponent<Image>().color = Color.green;
                    //selTile.GetComponent<Image>().color = new Color(0f, 1f, 0f, 0.5f);
                    break;
            }
            selTile.GetComponent<Image>().color = Color.black;
            //-----------------------------
            // ADD ACTUALLLLLL DIFFICULTY NOT FIXED
            if(selTile.type == RoomType.None) {
                selTile.type = RandomRoomType();
                if (selTile.type == RoomType.Fight || selTile.type == RoomType.BossFight) {
                    selTile.GenerateRoom(10f);
                }
                selTile.UpdateTile();
            }
            selTile.team = team;
            if (tempResultTile.Count == 1) {
                LinkTiles(tempResultTile[0], selTile);
            } else {
                MapTile linkTile = tempResultTile.OrderBy(r => Random.value).First();
                LinkTiles(linkTile, selTile);
            }
        }
    }

    public List<MapTile> GetFreeTilesAround(MapTile tile) {
        List<MapTile> resultList = new List<MapTile>();
        int tempX = tile.coorX - 1;
        int tempY = tile.coorY;
        if (tempX < 0 || tempX >= mapSize || tempY < 0 || tempY >= mapSize) { } else {
            MapTile tempTile = GetMapTileAtPosition(new Vector2(tempX, tempY));
            if (tile.w_connect != tempTile) {
                resultList.Add(tempTile);
            }
        }
        tempX = tile.coorX + 1;
        tempY = tile.coorY;
        if (tempX < 0 || tempX >= mapSize || tempY < 0 || tempY >= mapSize) { } else {
            MapTile tempTile = GetMapTileAtPosition(new Vector2(tempX, tempY));
            if (tile.e_connect != tempTile) {
                resultList.Add(tempTile);
            }
        }
        tempX = tile.coorX;
        tempY = tile.coorY - 1;
        if (tempX < 0 || tempX >= mapSize || tempY < 0 || tempY >= mapSize) { } else {
            MapTile tempTile = GetMapTileAtPosition(new Vector2(tempX, tempY));
            if (tile.s_connect != tempTile) {
                resultList.Add(tempTile);
            }
        }
        tempX = tile.coorX;
        tempY = tile.coorY + 1;
        if (tempX < 0 || tempX >= mapSize || tempY < 0 || tempY >= mapSize) { } else {
            MapTile tempTile = GetMapTileAtPosition(new Vector2(tempX, tempY));
            if (tile.n_connect != tempTile) {
                resultList.Add(tempTile);
            }
        }
        return resultList;
    }

    public void LinkTiles(MapTile tileOne, MapTile tileTwo) {
        if (tileOne.coorX == tileTwo.coorX) {
            if (tileOne.coorY < tileTwo.coorY) {
                tileOne.n_connect = tileTwo;
                tileTwo.s_connect = tileOne;
                tileOne.transform.GetChild(0).gameObject.SetActive(true);
            } else {
                tileOne.s_connect = tileTwo;
                tileTwo.n_connect = tileOne;
                tileOne.transform.GetChild(2).gameObject.SetActive(true);
            }
        } else {
            if (tileOne.coorX < tileTwo.coorX) {
                tileOne.e_connect = tileTwo;
                tileTwo.w_connect = tileOne;
                tileOne.transform.GetChild(1).gameObject.SetActive(true);
            } else {
                tileOne.w_connect = tileTwo;
                tileTwo.e_connect = tileOne;
                tileOne.transform.GetChild(3).gameObject.SetActive(true);
            }
        }
    }


    public void MakePathToTeam(GeneratorTeam team) {
        List<MapTile> posNewTiles = new List<MapTile>();
        foreach (MapTile tile in mapTilesList) {
            if (tile.team == GeneratorTeam.Start) {
                List<MapTile> tilesAround = GetFreeTilesAround(tile);
                foreach (MapTile tempTile in tilesAround) {
                    if (!posNewTiles.Contains(tempTile) && tempTile.team == team) {
                        posNewTiles.Add(tempTile);
                    }
                }
            }
        }
        if (posNewTiles.Count > 0) {
            MapTile selTile = posNewTiles.OrderBy(r => Random.value).First();
            List<MapTile> tilesAround = GetFreeTilesAround(selTile);
            List<MapTile> tempResultTile = new List<MapTile>();
            foreach (MapTile tempTile in tilesAround) {
                if (tempTile.team == GeneratorTeam.Start) {
                    tempResultTile.Add(tempTile);
                }
            }
            if (tempResultTile.Count == 1) {
                LinkTiles(tempResultTile[0], selTile);
            } else {
                MapTile linkTile = tempResultTile.OrderBy(r => Random.value).First();
                LinkTiles(linkTile, selTile);
            }
        }
    }

    public RoomType RandomRoomType() {
        int rand = Random.Range(0, 15);
        if (rand == 0) {
            return RoomType.Shrine;
        }
        if (rand == 1) {
            return RoomType.Shop;
        }
        if (rand == 2) {
            return RoomType.BossFight;
        }
        if (rand == 3) {
            return RoomType.Well;
        }
        if (rand >= 4 && rand < 7) {
            return RoomType.Tomb;
        }
        return RoomType.Fight;
    }


    public void RegenerateMap() {
        foreach (Transform child in anker.transform) {
            GameObject.Destroy(child.gameObject);
        }
        genFinished = false;
        StartMapGeneration();
    }

    public void HandleButtonClick(Vector2 position) {
        MapTile clickedTile = GetMapTileAtPosition(position);
        clickedTile.GetComponentInChildren<TextMeshProUGUI>().text = ".";
    }

    public void SetActiveTile(MapTile tile) {
        activeTile = tile;
    }

    public void ShowNewMapTiles() {
        activeTile.UnveilNewTiles();
    }

    public void DeselectAllTilesExcept(MapTile exceptionTile) {
        foreach (MapTile tile in mapTilesList) {
            if (tile.explored && tile != exceptionTile) {
                tile.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1f);
            } 
        }
    }
}
