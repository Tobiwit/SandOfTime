using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;


    [SerializeField] public int _width, _height;

    [SerializeField] private Tile _tileNormalPrefab;

    [SerializeField] private Tile _tileEnemyPrefab;

    [SerializeField] private Tile _tileDividerPrefab;

    [SerializeField] private Tile _tilePillarPrefab;

    [SerializeField] private Tile _tileSpikePrefab;

    [SerializeField] private Transform _cam;

    public double spacing = 1;

    private Dictionary<Vector2, Tile> _tiles;

    void Awake()
    {
        Instance = this;
    }

    public void SetCamera(GameObject camera) {
        _cam = camera.transform;
    }


    public void GenerateGrid()
    {
        _height = LevelManager.Instance.nextRoom.tileSize;
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                double _xNew = (double)(x + y)*spacing;
                double _yNew = (double)(0.5 * x - 0.5 * y)*spacing;
                Tile _currentPrefab;
                
                if (x < 2) {
                    _currentPrefab = _tileNormalPrefab;
                } else if (x == 2)
                {
                    _currentPrefab = _tileDividerPrefab;
                } else {
                    _currentPrefab = _tileEnemyPrefab;
                }

                if (LevelManager.Instance.nextRoom.specialTileList.Count > 0) {
                    foreach (SpecialTile tile in LevelManager.Instance.nextRoom.specialTileList) {
                        if (tile.rank == x && tile.position == y) {
                            _currentPrefab = tile.tile;
                        }
                    }
                }

                var spawnedTileTemp = Instantiate(_currentPrefab, new Vector3((float)_xNew, (float)_yNew), Quaternion.identity);
                var spawnedTile = spawnedTileTemp;
                spawnedTile.name = $"Tile {x} {y}";

                spawnedTile.xVector = x;
                spawnedTile.yVector = y;

                _tiles[new Vector2(x, y)] = spawnedTile;

                if(spawnedTile.isObjectTile) {
                    SpawnObject((ObjectTile) spawnedTile);
                }
            }
        }
        _cam = GetComponentInChildren<Camera>().transform;
        _cam.transform.position = new Vector3((float)_width / 2, (float) 0, -10);

        GameManager.Instance.UpdateGameState(GameState.SpawnPlayers);
    }

    public void SpawnObject(ObjectTile tile) {
        var spawnedObject = Instantiate(tile.objectPrefab);
        tile.SetUnit(spawnedObject, false);
    }

    public Tile GetHeroSpawnTile()
    {
        return _tiles.Where(t => t.Key.x == 0 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }

    public Tile GetEnemySpawnTile()
    {
        return _tiles.Where(t => t.Key.x == (_width-1) && t.Value.OccupiedUnit == null).OrderBy(t => Random.value).FirstOrDefault().Value;
    }



    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        else
        {
            return null;
        }
    }

    public void MoveCameraX(float yValue)
    {
        LeanTween.moveX(_cam.transform.gameObject, yValue, 3f).setEaseInExpo();
    }

    public void removeValidMove()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                Tile tileOption = GridManager.Instance.GetTileAtPosition(new Vector2(i, j));
                if (tileOption != null)
                {
                    tileOption._activeCharacterHighlight.SetActive(false);
                    tileOption.validMove = false;
                }
            }
        }
        CombatManager.Instance.activeHero.OccupiedTile._activeCharacterHighlight.SetActive(true);
    }

    public void removeTargetedTiles()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                Tile tileOption = GridManager.Instance.GetTileAtPosition(new Vector2(i, j));
                if (tileOption != null)
                {
                    tileOption._TargetHighlight.SetActive(false);
                    tileOption._AoeTargetHighlight.SetActive(false);
                    tileOption._AoeTargetHighlight.GetComponent<SpriteRenderer>().color = Color.white;
                    tileOption.isTargeted = false;
                }
            }
        }
    }

}
