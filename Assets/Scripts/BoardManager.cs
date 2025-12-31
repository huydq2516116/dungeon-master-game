using NUnit.Framework;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Tilemaps;


public class BoardManager : MonoBehaviour
{ 

    public static BoardManager Instance;

    [SerializeField] Tilemap _tilemap;
    [SerializeField] Tile[] cornerTiles;
    [SerializeField] Tile[] leftTiles;
    [SerializeField] Tile[] rightTiles;
    [SerializeField] Tile[] topTiles;
    [SerializeField] Tile innerTile;
    [SerializeField] int _initBoardHeight, _initBoardWidth;
    [SerializeField] int _distanceBetweenFloor;
    [SerializeField] int _maxBoardWidth;

    [SerializeField] GameObject _startPortalPrefab, _endPortalPrefab;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        TickManager.Instance.StartBoardManager += StartBoardManager;
    }
    private void StartBoardManager()
    {
        Values.floorList = new List<Values.Floor>();
        Values.startPositions = new List<Vector2Int>();
        Values.endPositions = new List<Vector2Int>();
        GenerateFloor(0);
        Values.maxFloor = 1;
        Values.isMoveMode = false;
    }

    private void GenerateFloor(int floor)
    {
        GenerateFloorInfo(floor);
        GenerateFloorOnTilemap(floor);
    }
    void GenerateFloorOnTilemap(int floor)
    {
        for (int i = 0; i < _initBoardHeight - 2; i++)
        {
            int randomTile = Random.Range(0, leftTiles.Length);
            Tile tile = leftTiles[randomTile];
            _tilemap.SetTile(ToRealBase(new Vector2Int(0, i + 1 + _distanceBetweenFloor * floor)), tile);
        }
        for (int i = 0; i < _initBoardHeight - 2; i++)
        {
            int randomTile = Random.Range(0, rightTiles.Length);
            Tile tile = rightTiles[randomTile];
            _tilemap.SetTile(ToRealBase(new Vector2Int(_initBoardWidth - 1, i + 1 + _distanceBetweenFloor * floor)), tile);
        }
        for (int i = 0; i < _initBoardWidth - 2; i++)
        {
            int randomTile = Random.Range(0, topTiles.Length);
            Tile tile = topTiles[randomTile];
            Vector3Int cell = ToRealBase(new Vector2Int(i + 1, 0 + _distanceBetweenFloor * floor));
            _tilemap.SetTile(cell, tile);

            _tilemap.SetTransformMatrix(cell, GetTileRotation(180));
        }
        for (int i = 0; i < _initBoardWidth - 2; i++)
        {
            int randomTile = Random.Range(0, topTiles.Length);
            Tile tile = topTiles[randomTile];
            Vector3Int cell = ToRealBase(new Vector2Int(i + 1, _initBoardHeight - 1 + _distanceBetweenFloor * floor));
            _tilemap.SetTile(cell, tile);
        }
        for (int i = 0; i < _initBoardWidth - 2; i++)
        {
            for (int j = 0; j < _initBoardHeight - 2; j++)
            {
                Tile tile = innerTile;
                Vector3Int cell = ToRealBase(new Vector2Int(i + 1, j + 1 + _distanceBetweenFloor * floor));
                _tilemap.SetTile(cell, tile);
            }
        }

        //Corner tiles:
        Tile bottomLeftTile = cornerTiles[0];
        Tile bottomRightTile = cornerTiles[1];

        Vector3Int bottomLeft = ToRealBase(new Vector2Int(0, 0 + _distanceBetweenFloor * floor));
        Vector3Int bottomRight = ToRealBase(new Vector2Int(_initBoardWidth - 1, 0 + _distanceBetweenFloor * floor));
        Vector3Int upLeft = ToRealBase(new Vector2Int(0, _initBoardHeight - 1 + _distanceBetweenFloor * floor));
        Vector3Int upRight = ToRealBase(new Vector2Int(_initBoardWidth - 1, _initBoardHeight - 1 + _distanceBetweenFloor * floor));

        _tilemap.SetTile(bottomLeft, bottomLeftTile);
        _tilemap.SetTile(bottomRight, bottomRightTile);
        _tilemap.SetTile(upLeft, bottomRightTile);
        _tilemap.SetTile(upRight, bottomLeftTile);

        _tilemap.SetTransformMatrix(upLeft, GetTileRotation(180));
        _tilemap.SetTransformMatrix(upRight, GetTileRotation(180));

        GameObject startPrefab = Instantiate(_startPortalPrefab);
        StartPortalObject startPortalObject = startPrefab.GetComponent<StartPortalObject>();
        startPortalObject.Generated(floor);
        GameObject endPrefab = Instantiate(_endPortalPrefab);
        EndPortalObject endPortalObject = endPrefab.GetComponent<EndPortalObject>();
        endPortalObject.Generated(floor);
    }
    void GenerateFloorInfo(int floor)
    {
        Values.Floor newFloor = new Values.Floor();
        newFloor.boardWidth = _initBoardWidth;
        newFloor.cells = new Values.Cell[_maxBoardWidth, _initBoardHeight];
        Values.floorList.Add(newFloor);
        for (int i = 0; i < _maxBoardWidth; i++)
        {
            for (int j = 0; j < _initBoardHeight; j++)
            {
                newFloor.cells[i, j] = new Values.Cell();
                Values.SetPassable(floor, i, j, false);
                Values.SetPlaceable(floor, i, j, false);
            }
        }
        for (int i = 1; i < _initBoardWidth - 1; i++)
        {
            for (int j = 1; j < _initBoardHeight - 1; j++)
            {
                Values.SetPassable(floor, i, j, true);
                Values.SetPlaceable(floor, i, j, true);
            }
        }
    }

    public void Expand(int floor)
    {
        int oldBoardWidth = Values.floorList[floor].boardWidth;
        if (oldBoardWidth >= _maxBoardWidth) return;
        int newBoardWidth = oldBoardWidth + 1;
        Values.floorList[floor].boardWidth += 1;
        for (int i=1; i< _initBoardHeight - 1;i++)
        {
            Values.SetPassable(floor, oldBoardWidth - 1, i, true);
            Values.SetPlaceable(floor, oldBoardWidth - 1, i, true);
        }
        
        for (int i=1; i< _initBoardHeight - 1; i++)
        {
            Tile tile = innerTile;
            Vector3Int cell = ToRealBase(new Vector2Int(oldBoardWidth-1, i + _distanceBetweenFloor * floor));
            _tilemap.SetTile(cell, tile);
        }
        int  randomTopTile = Random.Range(0, topTiles.Length);
        Tile topTile = topTiles[randomTopTile];
        Vector3Int topCell = ToRealBase(new Vector2Int(oldBoardWidth - 1, _initBoardHeight - 1 + _distanceBetweenFloor * floor));
        Vector3Int bottomCell = ToRealBase(new Vector2Int(oldBoardWidth - 1, _distanceBetweenFloor * floor));

        _tilemap.SetTile(topCell, topTile);
        _tilemap.SetTransformMatrix(topCell, Matrix4x4.identity);
        _tilemap.SetTile(bottomCell, topTile);
        _tilemap.SetTransformMatrix(bottomCell, GetTileRotation(180));

        for (int i = 1; i < _initBoardHeight - 1; i++)
        {
            int randomTile = Random.Range(0, rightTiles.Length);
            Tile tile = rightTiles[randomTile];
            Vector3Int cell = ToRealBase(new Vector2Int(newBoardWidth - 1, i + _distanceBetweenFloor * floor));
            _tilemap.SetTile(cell, tile);
        }

        Tile bottomLeftTile = cornerTiles[0];
        Tile bottomRightTile = cornerTiles[1];
        Vector3Int topRightCell = ToRealBase(new Vector2Int(newBoardWidth - 1, _initBoardHeight - 1 + _distanceBetweenFloor * floor));
        Vector3Int bottomRightCell = ToRealBase(new Vector2Int(newBoardWidth - 1, _distanceBetweenFloor * floor));

        _tilemap.SetTile(topRightCell,bottomLeftTile);
        _tilemap.SetTransformMatrix(topRightCell, GetTileRotation(180));
        _tilemap.SetTile(bottomRightCell,bottomRightTile);

        
    }
    public void CreateNextFloor()
    {
        GenerateFloor(Values.maxFloor);
        Values.maxFloor++;
    }

    public Vector3 CellToWorld(Vector2Int vector2, int floor)
    {
        Vector3Int position = ToRealBase(new Vector2Int(vector2.x,vector2.y + floor * _distanceBetweenFloor));
        return _tilemap.GetCellCenterWorld(position);
    }
    public int GetDistanceBetweenFloor()
    {
        return _distanceBetweenFloor;
    }

    public int GetInitBoardWidth()
    {
        return _initBoardWidth;
    }
    public int GetMaxWidth()
    {
        return _maxBoardWidth;
    }
    public int GetInitBoardHeight()
    {
        return _initBoardHeight;
    }

    public Vector2Int GetWorldPosToCell(Vector2 pos, int floor)
    {
        Vector2Int gameBase = (Vector2Int)ToGameBase((Vector2Int)_tilemap.WorldToCell(pos));
        return new Vector2Int(gameBase.x, gameBase.y - floor * GetDistanceBetweenFloor());
    }

    public Vector3Int ToRealBase(Vector2Int vector) //Remember to change down if change this
    {
        return (Vector3Int)new Vector2Int(vector.x - 3, vector.y - 3);
    }
    public Vector3Int ToGameBase(Vector2Int vector) //Remember to change up if change this
    {
        return (Vector3Int)new Vector2Int(vector.x + 3, vector.y + 3);
    }

    public void SetColorToTile(Vector2Int cellPos, Color color) {
        _tilemap.SetTileFlags(ToRealBase(cellPos), TileFlags.None);
        _tilemap.SetColor(ToRealBase(cellPos), color);
    }

    Matrix4x4 GetTileRotation(int deg)
    {
        
        return Matrix4x4.TRS(
                            Vector3.zero,
                            Quaternion.Euler(0, 0, deg),
                            Vector3.one);
    } 
}
