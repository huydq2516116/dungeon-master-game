using UnityEngine;

public class StartPortalObject : CellObject
{
    private void Start()
    {
        transform.position = BoardManager.Instance.CellToWorld(new Vector2Int(1, 1));
        Values.floorList[0].cells[1, 1].containedObject = this;
        Values.floorList[0].cells[1, 1].passable = true;
    }
}
