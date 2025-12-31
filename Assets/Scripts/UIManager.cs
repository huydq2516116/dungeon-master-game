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
    [SerializeField] Button _runGameButton;
    [SerializeField] Button _drawButton;
    //Move Object
    [SerializeField] InputActionReference _clickActionRef;
    bool _clicked;
    CellObject _grabbedObject;
    Vector2Int _grabbedPosition;
    bool _grabbed;

    int _currentFloor;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        TickManager.Instance.StartUIManager += StartUIManager;
    }
    private void StartUIManager()
    {
        _backButton.gameObject.SetActive(false);
        
        _expandFloorButton.onClick.AddListener(() => BoardManager.Expand(_currentFloor));
        _nextFloorButton.onClick.AddListener(BoardManager.CreateNextFloor);
        _nextButton.onClick.AddListener(() => MoveCameraToFloor(1));
        _backButton.onClick.AddListener(() => MoveCameraToFloor(-1));
        _moveObjectButton.onClick.AddListener(Values.ReverseMoveMode);
        _drawButton.onClick.AddListener(() => Intermission.Instance.DrawHeroPlan(_currentFloor, Values.startPositions[_currentFloor], Values.endPositions[_currentFloor]));

        _currentFloor = 0;

        _clickActionRef.action.Enable();
        _clicked = false;

        TickManager.Instance.UITick += UITick;
    }

    void MoveCameraToFloor(int floorChange)
    {
        _currentFloor += floorChange;
        _cameraMovement.MoveCamera(_currentFloor);
    }


    void UITick()
    {
        _backButton.gameObject.SetActive(_currentFloor > 0);
        _nextButton.gameObject.SetActive(_currentFloor < Values.GetMaxFloor() - 1);
        if (Values.isMoveMode)
        {
            MoveObject();
        }
        
        
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

                Vector2Int mousePosInTile = BoardManager.GetWorldPosToCell(worldMousePos,_currentFloor);

                if (mousePosInTile.x < 0 || mousePosInTile.y < 0 || mousePosInTile.x >= 12 || mousePosInTile.y >= 5) return;
                Values.Floor floor = Values.floorList[_currentFloor];
                Values.Cell cell = floor.cells[mousePosInTile.x, mousePosInTile.y];
                CellObject objectSelected = cell.containedObject;
                if (objectSelected != null && !_grabbed)
                {
                    _grabbed = true;
                    _grabbedObject = objectSelected;
                    _grabbedPosition = mousePosInTile;
                    Debug.Log("ObjectSelected: " + objectSelected);

                }
                else if (objectSelected == null && _grabbed)
                {
                    if (!Values.GetPlaceable(_currentFloor, mousePosInTile.x, mousePosInTile.y)) return;
                    _grabbed = false;
                    _grabbedObject.Moved(_grabbedPosition, mousePosInTile, _currentFloor);
                    
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
