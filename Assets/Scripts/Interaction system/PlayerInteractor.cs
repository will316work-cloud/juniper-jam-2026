using UnityEngine;

// Should be attached to the Player object
public class PlayerInteractor : MonoBehaviour
{
    [Header("Settings")]
    public float range = 3f;
    public KeyCode interactKey = KeyCode.E;

    private Interactable currentInteractable;

    void Update()
    {
        UpdateCurrentInteractable();

        if (currentInteractable != null && Input.GetKeyDown(interactKey))
        {
            currentInteractable.Interact();
        }
    }

    void UpdateCurrentInteractable()
    {
        Interactable newInteractable = GetClosestInteractable();

        if (newInteractable == currentInteractable)
            return;

        if (currentInteractable != null)
            currentInteractable.HideIcon();

        currentInteractable = newInteractable;

        if (currentInteractable != null)
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
        Gizmos.color = currentInteractable != null ? Color.green: Color.red;

        Gizmos.DrawWireSphere(transform.position, range);

        if (currentInteractable != null)
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawLine(transform.position, currentInteractable.transform.position);
        }
    }

}