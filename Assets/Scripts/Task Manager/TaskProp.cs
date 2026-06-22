using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TaskProp : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    protected GameContext _ctx;

    /// <summary>
    /// Called when player interacted with the trigger object.
    /// Should reset the PlayerTask logic to default.
    /// </summary>
    public abstract void OnTaskStart();
    public virtual void Initialize(GameContext ctx)
    {
        _ctx = ctx;
    }

    /// <summary>
    /// Called when the left mouse button is pressed down.
    /// </summary>
    public abstract void PointerDownHandler();

    /// <summary>
    /// Called when the left mouse button is released.
    /// </summary>
    public abstract void PointerUpHandler();

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerDownHandler();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PointerUpHandler();
    }
}