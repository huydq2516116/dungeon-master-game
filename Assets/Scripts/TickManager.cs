using System;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    public static TickManager Instance;
    [SerializeField] float tickSpeed;

    public event Action HeroTick;
    public event Action TrapTick;
    public event Action MonsterTick;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        float timer = Time.deltaTime;
        if (timer > tickSpeed)
        {
            HeroTick?.Invoke();
            TrapTick?.Invoke();
            MonsterTick?.Invoke();
        }

    }

}
