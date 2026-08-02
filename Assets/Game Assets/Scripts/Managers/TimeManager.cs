using System;
using UnityEngine;

public class TimeManager : MonoBehaviour,ITime
{
    [SerializeField] private float currentTime;

    private bool isPaused;

    private LevelLoadManager _levelLoadManager;
    private IGameStateService _gameStateService;

    public event Action<int> OnTimeChanged;
    private void OnDisable()
    {
        _gameStateService.OnStateChanged -= HandleGameStateChanged;
    }
    public void Construct(LevelLoadManager levelLoadManager,IGameStateService gameStateService)
    {
        _levelLoadManager = levelLoadManager;
        _gameStateService = gameStateService;

        currentTime = _levelLoadManager.LevelTime;

        _gameStateService.OnStateChanged += HandleGameStateChanged;

        StartCoroutine(TimerRoutine());
    }
    private void HandleGameStateChanged(GameState gameState)
    {
        isPaused = gameState == GameState.Paused;
    }
    private int lastDisplayedTime = -1;

    private System.Collections.IEnumerator TimerRoutine()
    {
        while (currentTime > 0)
        {
            if (!isPaused)
            {
                currentTime -= Time.deltaTime;

                int displayTime = Mathf.CeilToInt(currentTime);

                if (displayTime != lastDisplayedTime)
                {
                    lastDisplayedTime = displayTime;
                    OnTimeChanged?.Invoke(displayTime);
                }
            }

            yield return null;
        }

        OnTimeChanged?.Invoke(0);
        _gameStateService.ChangeState(GameState.Death);
    }
}