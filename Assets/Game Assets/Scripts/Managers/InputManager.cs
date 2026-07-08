using UnityEngine;
public class InputManager : IInputProvider
{
    private GameInput gameInput;

    public float InputX { get; set; }

    //Inputs
    public event System.Action OnJump;
    public event System.Action OnPaused;
    public event System.Action OnDash;

    public void NewInputService()
    {
        gameInput = new GameInput();
        gameInput.Enable();

        gameInput.Player.Jump.performed += ctx => OnJump?.Invoke();
        gameInput.UI.GamePause.performed += ctx => OnPaused?.Invoke();
        gameInput.Player.Dash.performed += ctx => OnDash?.Invoke();
    }
    public void NewInputServiceDisable()
    {
        gameInput.Disable();
    }
    public void UpdateInputX() => InputX = gameInput.Player.Move.ReadValue<Vector2>().x;
}
