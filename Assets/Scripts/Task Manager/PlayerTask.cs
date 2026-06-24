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
        _ctx.PoolManager.GetSfx(AudioType.TaskAlert);
    }
    public void OnTaskStart()
    {
        TaskProp.OnTaskStart();
        SetTaskPanelState(true);
    }
    public void OnTaskFail(bool withAudio = true)
    {
        if(!_ctx.GameStateController.IsPlayerDead == false) return;
        if(withAudio) _ctx.PoolManager.GetSfx(AudioType.TaskFail);
        Debug.Log("Task failed. Health penalty: " + TaskWorldHealthPenalty);
        _ctx.WorldHealthMeter.LoseHealth(TaskWorldHealthReward);
    }
    public void OnTaskSuccess(bool withAudio = true)
    {
        if(!_ctx.GameStateController.IsPlayerDead == false) return;
        if(withAudio) _ctx.PoolManager.GetSfx(AudioType.TaskSuccess);
        Debug.Log("Task success. Health reward: " + TaskWorldHealthReward);
        _ctx.WorldHealthMeter.GainHealth(TaskWorldHealthReward);
    }
    public void OnTaskEnd(bool isSuccess)
    {
        IsSuccess = isSuccess;
        SetTaskPanelState(false);
        if(_ctx.GameStateController.CurrentState is PlayerTaskState) _ctx.GameStateController.ChangeState(StateType.Gameplay);
        else _ctx.TaskManager.OnTaskEnd();
    }

    public void DisableTriggerObj() => TriggerObj.canInteract = false;

    /// <summary>
    /// Called when a task is assigned to the player. But haven't interacted with it yet.
    /// </summary>
    public void EnableTriggerObj() => TriggerObj.canInteract = true;
    public void SetTaskPanelState(bool sate) => TaskPanel.SetActive(sate);
} 