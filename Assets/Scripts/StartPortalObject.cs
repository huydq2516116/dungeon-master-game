using UnityEngine;

public class StartPortalObject : CellObject
{
    [SerializeField] private int init_x, init_y;
    public override void Generated(int floor)
    {
        Values.startPositions.Add(new Vector2Int(init_x, init_y));
        transform.position = BoardManager.Instance.CellToWorld(new Vector2Int(init_x, init_y),floor);
        Values.SetContainedObject(floor, init_x, init_y, this);
        Values.SetPassable(floor, init_x, init_y, true);
        Values.SetPlaceable(floor, init_x, init_y, false);
    }

    public override void Moved(Vector2Int startPos,Vector2Int targetPos, int floor)
    {
        base.Moved(startPos,targetPos, floor);
        Values.SetPassable(floor, targetPos.x, targetPos.y, true);
        Values.startPositions[floor] = targetPos;
        Intermission.Instance.DrawHeroPlan(floor, targetPos, Values.endPositions[floor]);
    }
}
