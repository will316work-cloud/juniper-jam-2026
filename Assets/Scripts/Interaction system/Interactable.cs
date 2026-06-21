using UnityEngine;

//Do not attach this script to any object, it is used as a base class for all interactable objects
public abstract class Interactable : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject interactionIcon;

    public bool canInteract = true;

    public abstract void Interact();
    public void ShowIcon()
    {
        if (interactionIcon != null)
            interactionIcon.SetActive(true);
    }

    public void HideIcon()
    {
        if (interactionIcon != null)
            interactionIcon.SetActive(false);
    }

    protected virtual void Awake()
    {
        HideIcon();
    }
}