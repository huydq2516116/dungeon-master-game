using UnityEngine;

public class Intermission : MonoBehaviour
{
    public static Intermission Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        TickManager.Instance.StartIntermission += StartIntermission;
    }
    void StartIntermission()
    {
        
    }

    public void DrawHeroPlan(int floor, Vector2Int startPos, Vector2Int endPos)
    {
        RemovePreviousHeroPlan(floor);
        Vector2Int[] highlightCell = Pathfinder.FindPath(startPos, endPos, null, 0);
        Values.GetFloor(floor).heroTripCells = highlightCell;

        for (int i = 1; i < highlightCell.Length; i++)
        {
            highlightCell[i] = new Vector2Int(highlightCell[i].x, highlightCell[i].y + BoardManager.Instance.GetDistanceBetweenFloor() * floor);
            BoardManager.Instance.SetColorToTile(highlightCell[i], Color.yellow);
        }
    }
    void RemovePreviousHeroPlan(int floor)
    {
        for (int i = 1; i < BoardManager.Instance.GetInitBoardHeight(); i++)
        {
            for (int j = 1; j < BoardManager.Instance.GetMaxWidth(); j++)
            {
                BoardManager.Instance.SetColorToTile(new Vector2Int(j, i + BoardManager.Instance.GetDistanceBetweenFloor() * floor), Color.white);
            }
        }
    }
}
