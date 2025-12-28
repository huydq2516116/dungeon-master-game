using UnityEngine;

public class StartPortalObject : CellObject
{
    protected override void StartCellObject()
    {
        transform.position = BoardManager.Instance.CellToWorld(new Vector2Int(1, 1));
        Values.SetContainedObject(0, 1, 1, this);
        Values.SetPassable(0, 1, 1, true);
        Values.SetPlaceable(0, 1, 1, false);
    }
}
