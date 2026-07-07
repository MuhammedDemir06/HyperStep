using UnityEngine;

public interface IGameStateService
{
    GameState CurrentState { get; }

    event System.Action<GameState> OnStateChanged;
    void ChangeState(GameState newState);
    void Construct(IInputProvider provider);
}
public enum GameState
{
    Paused,GamePlay,Death,Menu //....
}
public class GameStateService : IGameStateService
{
    public GameState CurrentState { get; private set; } = GameState.GamePlay;

    public event System.Action<GameState> OnStateChanged;

    private IInputProvider _inputProvider;
    public void Construct(IInputProvider inputProvider)
    {
        _inputProvider = inputProvider;
        _inputProvider.OnPaused += TogglePause;
    }
    public void ChangeState(GameState newState)
    {
        if(CurrentState == newState) return;

        CurrentState = newState;
        OnStateChanged?.Invoke(CurrentState);

        Debug.Log($"Game State Changed to {CurrentState}!");
    }
    public void TogglePause()
    {
        if (CurrentState == GameState.Death)
            return;

        ChangeState(CurrentState == GameState.Paused
            ? GameState.GamePlay
            : GameState.Paused);
    }
}