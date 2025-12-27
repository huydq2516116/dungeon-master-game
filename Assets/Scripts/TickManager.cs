using System;
using System.Threading;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    public static TickManager Instance;
    [SerializeField] float tickSpeed;
    [SerializeField] float UItickSpeed;

    public event Action HeroTick;
    public event Action TrapTick;
    public event Action MonsterTick;

    public event Action UITick;
    float timer;

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
    }
    private void Update()
    {
        timer += Time.deltaTime;
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
