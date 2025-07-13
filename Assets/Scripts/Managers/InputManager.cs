using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static System.Action<float> PlayerMove;
    public static System.Action PlayerJump;

    private GameInput gameInput;

    //Inputs
    [HideInInspector]
    public float InputX;

    private void Awake()
    {
        gameInput = new GameInput();

        gameInput.Enable();

        //Input
        SetButtons();
    }
    private void SetButtons()
    {
        gameInput.Player.Jump.performed += JumpPerformed;
    }

    private void JumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(obj.ReadValueAsButton())
        {
            PlayerJump?.Invoke();
        }
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
