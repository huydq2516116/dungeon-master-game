using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class BoardManager : MonoBehaviour
{ 
    public class Floor
    {
        public int boardWidth;
        public Cell[,] cells;
    }

    public class Cell
    {
        public bool passable;
        
    }

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

    int maxFloor;


    List<Floor> floorList;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        floorList = new List<Floor>();
        GenerateFloor(0);
        maxFloor = 1;

    }



    private void GenerateFloor(int floor)
    {
        for (int i = 0; i < _initBoardHeight - 2; i++)
        {
            int randomTile = Random.Range(0, leftTiles.Length);
            Tile tile = leftTiles[randomTile];
            _tilemap.SetTile(ToNewBase(new Vector2Int(0, i + 1 + _distanceBetweenFloor * floor)), tile);
        }
        for (int i = 0; i < _initBoardHeight - 2; i++)
        {
            int randomTile = Random.Range(0, rightTiles.Length);
            Tile tile = rightTiles[randomTile];
            _tilemap.SetTile(ToNewBase(new Vector2Int(_initBoardWidth - 1, i + 1 + _distanceBetweenFloor * floor)), tile);
        }
        for (int i = 0; i < _initBoardWidth - 2; i++)
        {
            int randomTile = Random.Range(0, topTiles.Length);
            Tile tile = topTiles[randomTile];
            Vector3Int cell = ToNewBase(new Vector2Int(i + 1, 0 + _distanceBetweenFloor * floor));
            _tilemap.SetTile(cell, tile);

            _tilemap.SetTransformMatrix(cell, GetTileRotation(180));
        }
        for (int i = 0; i < _initBoardWidth - 2; i++)
        {
            int randomTile = Random.Range(0, topTiles.Length);
            Tile tile = topTiles[randomTile];
            Vector3Int cell = ToNewBase(new Vector2Int(i + 1, _initBoardHeight - 1 + _distanceBetweenFloor * floor));
            _tilemap.SetTile(cell, tile);
        }
        for (int i = 0; i < _initBoardWidth - 2; i++)
        {
            for (int j = 0; j < _initBoardHeight - 2; j++)
            {
                Tile tile = innerTile;
                Vector3Int cell = ToNewBase(new Vector2Int(i + 1, j + 1 + _distanceBetweenFloor * floor));
                _tilemap.SetTile(cell, tile);
            }
        }

        //Corner tiles:
        Tile bottomLeftTile = cornerTiles[0];
        Tile bottomRightTile = cornerTiles[1];

        Vector3Int bottomLeft = ToNewBase(new Vector2Int(0, 0 + _distanceBetweenFloor * floor));
        Vector3Int bottomRight = ToNewBase(new Vector2Int(_initBoardWidth - 1, 0 + _distanceBetweenFloor * floor));
        Vector3Int upLeft = ToNewBase(new Vector2Int(0, _initBoardHeight - 1 + _distanceBetweenFloor * floor));
        Vector3Int upRight = ToNewBase(new Vector2Int(_initBoardWidth - 1, _initBoardHeight - 1 + _distanceBetweenFloor * floor));

        _tilemap.SetTile(bottomLeft, bottomLeftTile);
        _tilemap.SetTile(bottomRight, bottomRightTile);
        _tilemap.SetTile(upLeft, bottomRightTile);
        _tilemap.SetTile(upRight, bottomLeftTile);

        _tilemap.SetTransformMatrix(upLeft, GetTileRotation(180));
        _tilemap.SetTransformMatrix(upRight, GetTileRotation(180));

        // Generate Floor
        
        Floor newFloor = new Floor();
        newFloor.boardWidth = _initBoardWidth;
        newFloor.cells = new Cell[_maxBoardWidth, _initBoardHeight];
        floorList.Add(newFloor);
    }
    
    public void Expand(int floor)
    {
        
        int oldBoardWidth = floorList[floor].boardWidth;
        if (oldBoardWidth >= _maxBoardWidth) return;
        int newBoardWidth = oldBoardWidth + 1;
        floorList[floor].boardWidth += 1;
        
        for (int i=1; i< _initBoardHeight - 1; i++)
        {
            Tile tile = innerTile;
            Vector3Int cell = ToNewBase(new Vector2Int(oldBoardWidth-1, i + _distanceBetweenFloor * floor));
            _tilemap.SetTile(cell, tile);
        }
        int  randomTopTile = Random.Range(0, topTiles.Length);
        Tile topTile = topTiles[randomTopTile];
        Vector3Int topCell = ToNewBase(new Vector2Int(oldBoardWidth - 1, _initBoardHeight - 1 + _distanceBetweenFloor * floor));
        Vector3Int bottomCell = ToNewBase(new Vector2Int(oldBoardWidth - 1, _distanceBetweenFloor * floor));

        _tilemap.SetTile(topCell, topTile);
        _tilemap.SetTransformMatrix(topCell, Matrix4x4.identity);
        _tilemap.SetTile(bottomCell, topTile);
        _tilemap.SetTransformMatrix(bottomCell, GetTileRotation(180));

        for (int i = 1; i < _initBoardHeight - 1; i++)
        {
            int randomTile = Random.Range(0, rightTiles.Length);
            Tile tile = rightTiles[randomTile];
            Vector3Int cell = ToNewBase(new Vector2Int(newBoardWidth - 1, i + _distanceBetweenFloor * floor));
            _tilemap.SetTile(cell, tile);
        }

        Tile bottomLeftTile = cornerTiles[0];
        Tile bottomRightTile = cornerTiles[1];
        Vector3Int topRightCell = ToNewBase(new Vector2Int(newBoardWidth - 1, _initBoardHeight - 1 + _distanceBetweenFloor * floor));
        Vector3Int bottomRightCell = ToNewBase(new Vector2Int(newBoardWidth - 1, _distanceBetweenFloor * floor));

        _tilemap.SetTile(topRightCell,bottomLeftTile);
        _tilemap.SetTransformMatrix(topRightCell, GetTileRotation(180));
        _tilemap.SetTile(bottomRightCell,bottomRightTile);
    }
    public void CreateNextFloor()
    {
        GenerateFloor(maxFloor);
        maxFloor++;
    }

    public Vector3 CellToWorld(Vector2Int vector2)
    {
        Vector3Int position = ToNewBase(new Vector2Int(vector2.x,vector2.y));
        return _tilemap.GetCellCenterWorld(position);
    }
    public int GetDistanceBetweenFloor()
    {
        return _distanceBetweenFloor;
    }

    public int GetMaxFloor()
    {
        return maxFloor;
    }
    public Floor GetFloor(int floor)
    {
        return floorList[floor];
    }


    Vector3Int ToNewBase(Vector2Int vector)
    {
        return (Vector3Int)new Vector2Int(vector.x - 3, vector.y - 3);
    }
    Matrix4x4 GetTileRotation(int deg)
    {
        
        return Matrix4x4.TRS(
                            Vector3.zero,
                            Quaternion.Euler(0, 0, deg),
                            Vector3.one);
    }
    
}
