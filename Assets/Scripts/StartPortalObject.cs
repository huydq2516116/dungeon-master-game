using UnityEngine;

public class StartPortalObject : CellObject
{
    protected override void StartCellObject()
    {
        transform.position = BoardManager.Instance.CellToWorld(new Vector2Int(1, 1),0);
        Values.SetContainedObject(0, 1, 1, this);
        Values.SetPassable(0, 1, 1, true);
        Values.SetPlaceable(0, 1, 1, false);
    }

    public override void Moved(Vector2Int startPos,Vector2Int targetPos, int floor)
    {
        base.Moved(startPos,targetPos, floor);
        Values.SetPassable(floor, targetPos.x, targetPos.y, true);
        Values.startPositions[floor] = targetPos;
        Intermission.Instance.DrawHeroPlan(floor, targetPos, Values.endPositions[floor]);
    }
}
