using UnityEngine;
using System.Collections.Generic;

public static class Values
{
    public class Floor
    {
        public int boardWidth;
        public Cell[,] cells;
    }
    public class Cell
    {
        public bool passable;
        public CellObject containedObject;
    }

    public static int coins;
    public static int gems;

    public static int maxFloor;

    public static List<Floor> floorList;

    public static void ChangeContainedObject(int floor, int cell_x, int cell_y, CellObject containedObject)
    {
        floorList[floor].cells[cell_x,cell_y].containedObject = containedObject;
    }
    public static void ChangePassable(int floor, int cell_x, int cell_y, bool passable)
    {
        floorList[floor].cells[cell_x, cell_y].passable = passable;
    }

}
