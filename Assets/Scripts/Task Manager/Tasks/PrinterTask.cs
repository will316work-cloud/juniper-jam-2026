public class PrinterTask : PlayerTask
{
    public PrinterCrank PrinterCrank; 

    public override void Initialize(GameContext ctx)
    {
        base.Initialize(ctx);
        PrinterCrank.Initialize(ctx);
    }

    public override void OnTaskAnnouncement()
    {
        EnableTriggerObj();
    }

    public override void OnTaskStart()
    {
        PrinterCrank.OnTaskStart();
        _ctx.TaskManager.SetPrinterUiState(true);
        _ctx.PlayerControl.enabled = false;
    }

    public override void OnTaskFail()
    {
        _ctx.WorldHealthMeter.LoseHealth(TaskWorldHealthReward);
    }

    public override void OnTaskSuccess()
    {
        _ctx.WorldHealthMeter.GainHealth(TaskWorldHealthReward);
    }
    public override void OnTaskEnd(bool isSuccess)
    {
        IsSuccess = isSuccess;
        _ctx.GameStateController.ChangeState(StateType.Gameplay);
    }

    public override void Tick()
    {

    }
}