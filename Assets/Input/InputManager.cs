using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

internal class InputManager
{
    private InputMappingContext NewInputMapping;
    public InputMappingContext.PlayerActionsActions PlayerActions;
    public InputMappingContext.UIActionsActions UIActions;
    
    public InputManager()
    {
        NewInputMapping = new InputMappingContext();
        PlayerActions = NewInputMapping.PlayerActions;
        UIActions = NewInputMapping.UIActions;
        NewInputMapping.Enable();
    }
}

public class PlayerInput
{
    private static InputMappingContext.PlayerActionsActions playerInput;
    private static InputManager InputManager;

    public static UnityAction<InputAction.CallbackContext> MovementContext;
    public static UnityAction<InputAction.CallbackContext> InteractContext;
    public static UnityAction<InputAction.CallbackContext> SpaceContext;
    [RuntimeInitializeOnLoadMethod]
    private static void GetInputMapping()
    {
        InputManager = new InputManager();
        playerInput = InputManager.PlayerActions;
    }

    [RuntimeInitializeOnLoadMethod]
    private static void StartInputEvents()
    {
        playerInput.Movement.performed += ctx =>
            MovementContext?.Invoke(ctx);

        playerInput.Movement.canceled += ctx =>
            MovementContext?.Invoke(ctx);

        playerInput.Interact.performed += ctx =>
            InteractContext?.Invoke(ctx);
        
        playerInput.Space.performed += ctx =>
            SpaceContext?.Invoke(ctx);
        
        playerInput.Space.canceled += ctx =>
            SpaceContext?.Invoke(ctx);
    }
}

public class UIInput
{
    private static InputManager InputManager;
    private static InputMappingContext.UIActionsActions uiInput;
    
    [RuntimeInitializeOnLoadMethod]
    private static void GetInputMapping()
    {
        InputManager = new InputManager();
    }

    [RuntimeInitializeOnLoadMethod]
    private static void StartInputEvents()
    {
    }
}

