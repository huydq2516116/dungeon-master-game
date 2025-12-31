using UnityEngine;
using System.Collections.Generic;

public static class Values
{
    public class Floor
    {
        public int boardWidth;
        public Cell[,] cells;
        public Vector2Int[] heroTripCells;
    }
    public class Cell
    {
        public bool passable;
        public bool placeable;
        public CellObject containedObject;
    }

    public static bool isMoveMode;
    public static int coins;
    public static int gems;

    public static int maxFloor;

    public static List<Floor> floorList;

    public static List<Vector2Int> startPositions;
    public static List<Vector2Int> endPositions;



    public static Floor GetFloor(int floor)
    {
        return floorList[floor];
    }
    public static void SetContainedObject(int floor, int cell_x, int cell_y, CellObject containedObject)
    {
        floorList[floor].cells[cell_x,cell_y].containedObject = containedObject;
    }
    public static CellObject GetContainedObject(int floor, int cell_x, int cell_y)
    {
        return floorList[floor].cells[cell_x, cell_y].containedObject;
    }
    public static void SetPassable(int floor, int cell_x, int cell_y, bool passable)
    {
        floorList[floor].cells[cell_x, cell_y].passable = passable;
    }
    public static bool GetPassable(int floor, int cell_x, int cell_y)
    {
        return floorList[floor].cells[cell_x, cell_y].passable;
    }
    public static void SetPlaceable(int floor, int cell_x, int cell_y, bool placeable)
    {
        floorList[floor].cells[cell_x, cell_y].placeable = placeable;
    }
    public static bool GetPlaceable(int floor, int cell_x, int cell_y)
    {
        return floorList[floor].cells[cell_x, cell_y].placeable;
    }
    public static void ReverseMoveMode()
    {
        isMoveMode = !isMoveMode;
    }
    public static int GetMaxFloor()
    {
        return maxFloor;
    }
    

}
