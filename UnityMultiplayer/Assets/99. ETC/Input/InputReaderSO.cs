using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/Input/InputReader")]
public class InputReaderSO : ScriptableObject, Controls.IPlayerActions
{
    private Controls controls = null;

    public event Action<bool> OnPrimaryFireEvent;
    public event Action<Vector2> OnMoveEvent;
    public Vector2 InputDirection {get; private set;}

    private void OnEnable()
    {
        if(controls == null)
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
        }

        controls.Player.Enable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        InputDirection = context.ReadValue<Vector2>();
        OnMoveEvent?.Invoke(InputDirection);
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if(context.performed)
            OnPrimaryFireEvent?.Invoke(true);
        else if(context.canceled)
            OnPrimaryFireEvent?.Invoke(false);
    }
}
