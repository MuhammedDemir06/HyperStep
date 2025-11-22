using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static System.Action<float> PlayerMove;
    public static System.Action PlayerJump;

    public static System.Action<bool> GamePause;

    private GameInput gameInput;

    //Inputs
    [HideInInspector]
    public float InputX;

    private bool isClickedPause = false;
    private void OnEnable()
    {
        gameInput = new GameInput();
        gameInput.Enable();

        gameInput.Player.Jump.performed += JumpPerformed;
        gameInput.UI.GamePause.performed += GamePausePerformed;
    }

    private void OnDisable()
    {
        gameInput.Player.Jump.performed -= JumpPerformed;
        gameInput.UI.GamePause.performed -= GamePausePerformed;

        gameInput.Disable();
    }
    private void GamePausePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (obj.ReadValueAsButton())
        {
            isClickedPause = !isClickedPause;

            GamePause?.Invoke(isClickedPause);
        }
    }
    private void JumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(obj.ReadValueAsButton())
            PlayerJump?.Invoke();
    }
    private void SetMoveInput()
    {
        InputX = gameInput.Player.Move.ReadValue<Vector2>().x;
        PlayerMove?.Invoke(InputX);
    }
    private void Update()
    {
        SetMoveInput();
    }
}
