using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    //this script is only meant for in game input, not menu input.

    private InputAction moveAction;
    public InputAction interactAction;
    private InputAction pointAction;
    private InputAction clickAction;

    [HideInInspector] public Vector2 moveInput, mousePosition;
    [HideInInspector] public bool clickPressedThisFrame, clickCurrentlyHeld;

    public void Initialize()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        interactAction = InputSystem.actions.FindAction("Interact");
        pointAction = InputSystem.actions.FindAction("Point");
        clickAction = InputSystem.actions.FindAction("Click");
    }

    private void Update()
    {
        //read input and throw onto public vars

        //movement/interaction
        moveInput = moveAction.ReadValue<Vector2>();

        //mouse input
        clickPressedThisFrame = clickAction.WasPressedThisFrame();
        clickCurrentlyHeld = clickAction.IsPressed();
        mousePosition = pointAction.ReadValue<Vector2>();
    }
}
