using System;
using System.Threading;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    public static TickManager Instance;
    [SerializeField] float tickSpeed;
    [SerializeField] float UItickSpeed;

    public event Action StartBoardManager;
    public event Action StartUIManager;
    public event Action StartCellObject;

    public event Action HeroTick;
    public event Action TrapTick;
    public event Action MonsterTick;

    public event Action UITick;
    float timer;
    bool firstTick;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        timer = 0;
        firstTick = true;
    }
    private void Update()
    {
        timer += Time.deltaTime;

        if (firstTick && timer > tickSpeed)
        {
            firstTick = false;
            StartBoardManager?.Invoke();
            StartUIManager?.Invoke();
            StartCellObject?.Invoke();
        }

        if (timer > tickSpeed)
        {
            timer = 0;

            HeroTick?.Invoke();
            TrapTick?.Invoke();
            MonsterTick?.Invoke();

            
            UITick?.Invoke();
        }
        
    }

}
