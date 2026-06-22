using UnityEngine;

public abstract class TaskProp : MonoBehaviour
{
    protected GameContext _ctx;

    public abstract void OnTaskStart();
    public void Initialize(GameContext ctx)
    {
        _ctx = ctx;
    }
}