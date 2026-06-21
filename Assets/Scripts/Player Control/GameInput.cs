using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    //this script is only meant for in game input, not menu input.

    private InputAction moveAction;
    private InputAction interactAction;
    private InputAction pointAction;
    private InputAction clickAction;

    [HideInInspector] public Vector2 moveInput, mousePosition;
    [HideInInspector] public bool interactPressedThisFrame, clickPressedThisFrame, clickCurrentlyHeld;

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
        if (interactAction.triggered)
        {
            interactPressedThisFrame = true;
            Debug.Log("interact");
        }
        else interactPressedThisFrame = false;

        //mouse input
        clickPressedThisFrame = clickAction.triggered;
        clickCurrentlyHeld = clickAction.IsPressed();
        mousePosition = pointAction.ReadValue<Vector2>();
    }
}
