using UnityEngine;

public interface IInputProvider 
{
    float InputX { get; }
    event System.Action OnJump;
    event System.Action OnPaused;
    event System.Action OnDash;
}
