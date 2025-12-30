using System;
using Unity.VisualScripting;
using UnityEngine;

public class CellObject : MonoBehaviour
{
    private void Start()
    {
        TickManager.Instance.StartCellObject += StartCellObject;
    }
    protected virtual void StartCellObject()
    {

    }
    public virtual void Moved(Vector2Int startPos,Vector2Int targetPos, int floor)
    {
        transform.position = BoardManager.Instance.CellToWorld(targetPos, 0);
        Values.SetContainedObject(floor, targetPos.x, targetPos.y, this);
        // Need To Assign Passable in targetPos in Child
        Values.SetPlaceable(floor, targetPos.x, targetPos.y, false);

        Values.SetContainedObject(floor, startPos.x, startPos.y, null);
        Values.SetPassable(floor, startPos.x, startPos.y, true);
        Values.SetPlaceable(floor, startPos.x, startPos.y, true);

        
    }
}
