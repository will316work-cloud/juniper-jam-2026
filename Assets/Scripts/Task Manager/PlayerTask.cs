using UnityEngine;

public abstract class PlayerTask : MonoBehaviour
{
    public TaskTriggerObjectInstance TriggerObj;
    public string TaskName;
    [TextArea] public string TaskDescription;
    public int TaskMoneyReward;
    [HideInInspector] public bool IsSuccess;
    protected GameContext _ctx;
    public virtual void Initialize(GameContext ctx)
    {
        _ctx = ctx;
        DisableTriggerObj();
    }

    public abstract void OnTaskAnnouncement();
    public abstract void OnTaskStart();
    public abstract void OnTaskFail();
    public abstract void OnTaskSuccess();
    public abstract void OnTaskEnd(bool isSuccess);
    public abstract void Tick();

    public void DisableTriggerObj() => TriggerObj.canInteract = false;
    public void EnableTriggerObj() => TriggerObj.canInteract = true;
}