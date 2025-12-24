using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    [SerializeField] Button _nextButton;
    [SerializeField] Button _backButton;
    [SerializeField] Button _expandFloorButton;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
    }

    private void Start()
    {
        _expandFloorButton.onClick.AddListener(() => BoardManager.Instance.Expand(0));
    }
}
