using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] CameraMovement _cameraMovement;

    [SerializeField] Button _nextButton;
    [SerializeField] Button _backButton;
    [SerializeField] Button _expandFloorButton;
    [SerializeField] Button _nextFloorButton;

    int _currentFloor;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _backButton.gameObject.SetActive(false);
        
        _expandFloorButton.onClick.AddListener(() => BoardManager.Instance.Expand(0));
        _nextFloorButton.onClick.AddListener(BoardManager.Instance.CreateNextFloor);
        _nextButton.onClick.AddListener(() => MoveCameraToFloor(1));
        _backButton.onClick.AddListener(() => MoveCameraToFloor(-1));
        _currentFloor = 0;
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

    }
}
