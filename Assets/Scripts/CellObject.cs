using System;
using Unity.VisualScripting;
using UnityEngine;

public class CellObject : MonoBehaviour
{
    private void Start()
    {
        
    }
    public virtual void Generated(int floor)
    {

    }
    public virtual void Moved(Vector2Int startPos,Vector2Int targetPos, int floor)
    {
        transform.position = BoardManager.Instance.CellToWorld(targetPos, floor);
        Values.SetContainedObject(floor, targetPos.x, targetPos.y, this);
        // Need To Assign Passable in targetPos in Child
        Values.SetPlaceable(floor, targetPos.x, targetPos.y, false);

        Values.SetContainedObject(floor, startPos.x, startPos.y, null);
        Values.SetPassable(floor, startPos.x, startPos.y, true);
        Values.SetPlaceable(floor, startPos.x, startPos.y, true);

        
    }
}
