using UnityEngine;

public class EndPortalObject : CellObject
{
    protected override void StartCellObject()
    {
        transform.position = BoardManager.Instance.CellToWorld(new Vector2Int(5, 4),0);
        Values.SetContainedObject(0, 5, 4, this);
        Values.SetPassable(0, 5, 4, true);
        Values.SetPlaceable(0, 5, 4, false);
    }

    public override void Moved(Vector2Int startPos, Vector2Int targetPos, int floor)
    {
        base.Moved(startPos, targetPos, floor);
        Values.SetPassable(floor, targetPos.x, targetPos.y, true);
        Values.endPositions[floor] = targetPos;
        Intermission.Instance.DrawHeroPlan(floor, Values.startPositions[floor], targetPos);
    }
}
