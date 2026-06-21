using UnityEngine;

public abstract class PlayerTask
{
    protected GameContext _ctx;
    protected GameObject _triggerObj;
    public  TaskType TaskType;
    public bool IsSuccess { get; set; }

    public void Initialize(GameContext ctx, GameObject triggerObj)
    {
        _ctx = ctx;
        _triggerObj = triggerObj;
    }
    public string TaskName { get; protected set; }
    public string TaskDescription { get; protected set; }

    public abstract void OnTaskAnnouncement();
    public abstract void OnTaskStart();
    public abstract void OnTaskFail();
    public abstract void OnTaskSuccess();
    public abstract void OnTaskEnd();
    public abstract void Tick();

    protected void DisableTriggerObj() => _triggerObj.SetActive(false);
}