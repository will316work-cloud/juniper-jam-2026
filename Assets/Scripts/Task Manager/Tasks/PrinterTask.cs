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
        // Punishment logic
    }

    public override void OnTaskSuccess()
    {
        _ctx.MoneyController.GainMoney(TaskMoneyReward);
        _ctx.WorldHealthMeter.GainHealth(TaskHealthReward);
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