using System;
using UnityEngine;

public class TimeManager : MonoBehaviour,ITime
{
    [SerializeField] private float currentTime;

    private LevelLoadManager _levelLoadManager;
    private IGameStateService _gameStateService;

    public event Action<int> OnTimeChanged;

    public void Construct(LevelLoadManager levelLoadManager,IGameStateService gameStateService)
    {
        _levelLoadManager = levelLoadManager;
        _gameStateService = gameStateService;

        currentTime = _levelLoadManager.LevelTime;

        StartCoroutine(TimerRoutine());
    }
    private System.Collections.IEnumerator TimerRoutine()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            OnTimeChanged?.Invoke(Mathf.CeilToInt(currentTime));

            yield return null; 
        }

        currentTime = 0;
        OnTimeChanged?.Invoke(0);

        //Player Death
        _gameStateService.ChangeState(GameState.Death);
    }
}