using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TaskProp : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    protected GameContext _ctx;

    public abstract void OnTaskStart();
    public virtual void Initialize(GameContext ctx)
    {
        _ctx = ctx;
    }

    public abstract void PointerDownHandler();
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