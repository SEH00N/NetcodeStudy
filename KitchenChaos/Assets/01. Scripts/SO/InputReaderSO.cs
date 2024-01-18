using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

[CreateAssetMenu(menuName = "SO/Input/InputReader")]
public partial class InputReaderSO : ScriptableObject, IPlayerActions
{
    private const string BINDING_KEYS_INFO_KEY = "BindingKeysInfo";
    
	private Controls controls;

    public event Action OnPauseEvent;
    public event Action OnInteractEvent;
    public event Action OnInteractAlternateEvent;

    public Vector2 MoveInput { get; private set; }

    private Dictionary<KeyBinding, InputAction> actions;

    private void OnEnable()
    {
        Reset();
    }

    public void Reset()
    {
        if(controls != null)
        {
            OnPauseEvent = null;
            OnInteractEvent = null;
            OnInteractAlternateEvent = null;

            controls.Dispose();
        }

        controls = new Controls();
        controls.Player.SetCallbacks(this);

        string bindingKeysInfo = PlayerPrefs.GetString(BINDING_KEYS_INFO_KEY, null);
        if(bindingKeysInfo != null)
            controls.LoadBindingOverridesFromJson(bindingKeysInfo);

        InitBindActions();

        controls.Player.Enable();
    }

    private void InitBindActions()
    {
        this.actions = new Dictionary<KeyBinding, InputAction>();
        List<InputAction> actions = controls.Player.Get().actions.ToList();

        actions.ForEach(a => {
            if(Enum.TryParse(a.name, out KeyBinding bind))
                this.actions.Add(bind, a);
        });
    }

    public string GetBindingText(KeyBinding binding, int alpha = 0)
    {
        return actions[binding].bindings[alpha + 1].ToDisplayString();
    }

    public void Rebinding(KeyBinding binding, Action<string> callback = null)
    {
        controls.Player.Disable();
        RebindingOperation operation = actions[KeyBinding.Move].PerformInteractiveRebinding(1);
        operation.OnComplete(operation => {
            operation.Dispose();
            controls.Player.Enable();
            callback?.Invoke(operation.action.bindings[1].ToDisplayString());

            string json = controls.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString(BINDING_KEYS_INFO_KEY, json);
        });
        operation.Start();
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
    public void OnPause(InputAction.CallbackContext context)
    {
        if(context.performed)
            OnPauseEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }
}
