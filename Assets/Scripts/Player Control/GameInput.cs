using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    //this script is only meant for in game input, not menu input.

    private InputAction moveAction;
    private InputAction interactAction;
    private InputAction pointAction;
    private InputAction clickAction;

    public Vector2 moveInput;
    public bool interactPressedThisFrame;
    public Vector2 mousePosition;
    public bool clickPressedThisFrame;
    public bool clickCurrentlyHeld;

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
        if (interactAction.triggered) interactPressedThisFrame = true;
        else interactPressedThisFrame = false;

        //mouse input
        clickPressedThisFrame = clickAction.triggered;
        clickCurrentlyHeld = clickAction.IsPressed();
        mousePosition = pointAction.ReadValue<Vector2>();
    }
}
