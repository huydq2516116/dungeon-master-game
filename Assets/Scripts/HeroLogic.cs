using System.Linq.Expressions;
using UnityEngine;

public class HeroLogic : MonoBehaviour
{
    bool _isMoving;
    Vector3 _moveTarget;

    [SerializeField] float _moveSpeed;
    
    private void Start()
    {
        transform.position = BoardManager.Instance.CellToWorld(new Vector2Int(-1, -1), 0);
        _isMoving = false;

        TickManager.Instance.HeroTick += HeroTick;
    }

    void MoveTo(Vector2Int vector2)
    {
        _moveTarget = BoardManager.Instance.CellToWorld(vector2,0);
        _isMoving = true;
    }

    void HeroTick()
    {
        
    }
    private void Update()
    {
        if (_isMoving) {
            transform.position = Vector3.MoveTowards(transform.position, _moveTarget, _moveSpeed * Time.deltaTime);
            if (transform.position == _moveTarget)
            {
                _isMoving = false;
            }

        }
    }
}
