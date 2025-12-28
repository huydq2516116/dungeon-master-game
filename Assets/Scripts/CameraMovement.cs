using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float _camStartPositionX;
    [SerializeField] float _camStartPositionY;
    [SerializeField] float _camMoveSpeed;

    Camera _cam;
    bool c_isMoving;
    Vector3 _moveTarget;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
    }
    
    private void Start()
    {
        c_isMoving = false;

        transform.position = new Vector3(_camStartPositionX, _camStartPositionY, transform.position.z);
    }
    public void MoveCamera(int floor)
    {
        _moveTarget = new Vector3(_camStartPositionX, _camStartPositionY + floor * BoardManager.Instance.GetDistanceBetweenFloor(), transform.position.z);
        c_isMoving = true;
    }

    private void Update()
    {
        if (c_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, _moveTarget, _camMoveSpeed * Time.deltaTime);
            if (transform.position == _moveTarget)
            {
                c_isMoving = false;
            }
        }
    }

}
