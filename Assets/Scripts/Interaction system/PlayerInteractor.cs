using UnityEngine;
using UnityEngine.InputSystem;

// Should be attached to the Player object
public class PlayerInteractor : MonoBehaviour
{
    [Header("Settings")]
    public float range = 3f;

    private bool input;
    private Interactable currentInteractable;
    private bool hasAnyInteractable;

    public void Initialize(GameInput gameInput)
    {
         input = gameInput.interactPressedThisFrame;
    }

    void Update()
    {
        UpdateCurrentInteractable();

        if (currentInteractable != null && currentInteractable.canInteract && input)
        {
            currentInteractable.Interact();
        }
    }

    void UpdateCurrentInteractable()
    {
        Interactable newInteractable = GetClosestInteractable();

        hasAnyInteractable = newInteractable != null;

        if (newInteractable == currentInteractable)
            return;

        if (currentInteractable != null)
            currentInteractable.HideIcon();

        currentInteractable = newInteractable;

        if (currentInteractable != null && currentInteractable.canInteract)
            currentInteractable.ShowIcon();
    }



    // Returns the closest interactable object within range
    private Interactable GetClosestInteractable()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range);

        Interactable closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            Interactable interactable = hit.GetComponent<Interactable>();

            if (interactable == null)
                continue;

            if (!interactable.canInteract) // Ignore Inactive Interactable Objects
                continue;

            float distance = Vector3.Distance(transform.position, hit.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = interactable;
            }
        }

        return closest;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = hasAnyInteractable ? Color.green : Color.red;

        Gizmos.DrawWireSphere(transform.position, range);

        if (currentInteractable != null)
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawLine(transform.position,currentInteractable.transform.position);
        }
    }
}