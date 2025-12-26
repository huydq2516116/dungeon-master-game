using UnityEngine;

public class HeroLogic : MonoBehaviour
{
    private void Start()
    {
        transform.position = BoardManager.Instance.CellToWorld(new Vector2Int(1, -1));
    }
}
