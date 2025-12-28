using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] BoardManager BoardManager;

    [SerializeField] CameraMovement _cameraMovement;

    [SerializeField] Camera _cam;

    [SerializeField] Button _nextButton;
    [SerializeField] Button _backButton;
    [SerializeField] Button _expandFloorButton;
    [SerializeField] Button _nextFloorButton;
    [SerializeField] Button _moveObjectButton;
    //Move Object
    [SerializeField] InputActionReference _clickActionRef;
    bool _clicked;
    CellObject _grabbedObject;
    bool _grabbed;

    int _currentFloor;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _backButton.gameObject.SetActive(false);
        
        _expandFloorButton.onClick.AddListener(() => BoardManager.Expand(0));
        _nextFloorButton.onClick.AddListener(BoardManager.CreateNextFloor);
        _nextButton.onClick.AddListener(() => MoveCameraToFloor(1));
        _backButton.onClick.AddListener(() => MoveCameraToFloor(-1));
        _currentFloor = 0;

        _clickActionRef.action.Enable();
        _clicked = false;

        TickManager.Instance.UITick += UITick;
    }

    void MoveCameraToFloor(int floorChange)
    {
        _currentFloor += floorChange;
        _cameraMovement.MoveCamera(_currentFloor);
        ChangeExpandFloorTarget();
    }
    void ChangeExpandFloorTarget()
    {
        _expandFloorButton.onClick.RemoveAllListeners();
        _expandFloorButton.onClick.AddListener(() => BoardManager.Instance.Expand(_currentFloor));
    }

    void UITick()
    {
        _backButton.gameObject.SetActive(_currentFloor > 0);
        _nextButton.gameObject.SetActive(_currentFloor < BoardManager.Instance.GetMaxFloor() - 1);
        MoveObject();
        
    }

    void MoveObject()
    {
        float clickAction = _clickActionRef.action.ReadValue<float>();
        if (clickAction > 0)
        {
            if (!_clicked)
            {
                Vector2 mousePos = Mouse.current.position.ReadValue();
                Vector2 worldMousePos = _cam.ScreenToWorldPoint(mousePos);

                Vector2Int mousePosInTile = BoardManager.GetWorldPosToCell(worldMousePos);

                if (mousePosInTile.x < 0 || mousePosInTile.y < 0) return;

                Values.Floor floor = Values.floorList[_currentFloor];
                Values.Cell cell = floor.cells[mousePosInTile.x, mousePosInTile.y];
                CellObject objectSelected = cell.containedObject;
                if (objectSelected != null && !_grabbed)
                {
                    _grabbed = true;
                    _grabbedObject = objectSelected;
                }
                else if (objectSelected == null && _grabbed)
                {
                    _grabbed = false;
                    Values.ChangeContainedObject(_currentFloor, mousePosInTile.x, mousePosInTile.y, _grabbedObject);
                    _grabbedObject.gameObject.transform.position = BoardManager.CellToWorld(mousePosInTile);
                    _grabbedObject = null;

                }

            }
            _clicked = true;
        }
        else
        {
            _clicked = false;
        }
    }
}
