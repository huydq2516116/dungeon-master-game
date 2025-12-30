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
        
        Vector2Int[] highlightCell = Pathfinder.FindPath(new Vector2Int(1, 1), new Vector2Int(5, 4), null, 0);
        Values.GetFloor(0).heroTripCells = highlightCell;

        for (int i = 1; i < highlightCell.Length; i++)
        {
            BoardManager.Instance.SetColorToTile(highlightCell[i], Color.yellow);
        }
    }

    public void DrawHeroPlan(int floor, Vector2Int startPos, Vector2Int endPos)
    {
        RemovePreviousHeroPlan(floor);
        Vector2Int[] highlightCell = Pathfinder.FindPath(startPos, endPos, null, 0);
        Values.GetFloor(floor).heroTripCells = highlightCell;

        for (int i = 1; i < highlightCell.Length; i++)
        {
            BoardManager.Instance.SetColorToTile(highlightCell[i], Color.yellow);
        }
    }
    void RemovePreviousHeroPlan(int floor)
    {
        for (int i = 1; i < BoardManager.Instance.GetInitBoardHeight(); i++)
        {
            for (int j = 1; j < BoardManager.Instance.GetMaxWidth(); j++)
            {
                BoardManager.Instance.SetColorToTile(new Vector2Int(j, i), Color.white);
            }
        }
    }
}
