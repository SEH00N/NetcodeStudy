using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(menuName = "SO/Input/InputReader")]
public class InputReaderSO : ScriptableObject, IPlayerActions
{
	private Controls controls;

    public event Action OnFireEvent;
    public event Action OnInteractEvent;
    public event Action OnInteractAlternateEvent;

    public Vector2 MoveInput { get; private set; }

    private void OnEnable()
    {
        if(controls == null)
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
        }

        controls.Player.Enable();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.performed)
            OnFireEvent?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.performed)
            OnInteractEvent?.Invoke();
    }

    public void OnInteractAlternate(InputAction.CallbackContext context)
    {
        if(context.performed)
            OnInteractAlternateEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }
}
