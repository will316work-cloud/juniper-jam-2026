using UnityEngine;

public abstract class PlayerTask : MonoBehaviour
{
    public TaskTriggerObjectInstance TriggerObj;
    public GameObject TaskPanel;
    public TaskProp TaskProp;
    public string TaskName;
    [TextArea] public string TaskDescription;
    public int TaskWorldHealthReward;
    public int TaskWorldHealthPenalty;
    public float AvailableTime;
    [HideInInspector] public bool IsSuccess;
    protected GameContext _ctx;
    public virtual void Initialize(GameContext ctx)
    {
        _ctx = ctx;
        DisableTriggerObj();
        TaskProp.Initialize(ctx);
        TaskPanel.SetActive(false);
    }

    /// <summary>
    /// Called when a task is assigned to the player. But haven't interacted with it yet.
    /// </summary>
    public void OnTaskAnnouncement()
    {
        EnableTriggerObj();
        _ctx.AudioPool.GetAudio(AudioType.TaskAlert);
    }
    public void OnTaskStart()
    {
        TaskProp.OnTaskStart();
        SetTaskPanelState(true);
        _ctx.PlayerControl.enabled = false;
    }
    public void OnTaskFail()
    {
        _ctx.WorldHealthMeter.LoseHealth(TaskWorldHealthReward);
    }
    public void OnTaskSuccess()
    {
        _ctx.WorldHealthMeter.GainHealth(TaskWorldHealthReward);
    }
    public void OnTaskEnd(bool isSuccess)
    {
        IsSuccess = isSuccess;
        SetTaskPanelState(false);
        _ctx.GameStateController.ChangeState(StateType.Gameplay);
    }

    public void DisableTriggerObj() => TriggerObj.canInteract = false;

    /// <summary>
    /// Called when a task is assigned to the player. But haven't interacted with it yet.
    /// </summary>
    public void EnableTriggerObj() => TriggerObj.canInteract = true;

    public void SetTaskPanelState(bool sate) => TaskPanel.SetActive(sate);
}