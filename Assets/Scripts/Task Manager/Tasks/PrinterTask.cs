public class PrinterTask : PlayerTask
{
    public override void OnTaskAnnouncement()
    {
        // _triggerObj.SetActive(true);
        OnTaskStart();
    }

    public override void OnTaskStart()
    {
        _ctx.PrinterCrank.OnTaskStart();
        _ctx.TaskManager.SetPrinterUiState(true);
        _ctx.PlayerControl.enabled = false;
    }

    public override void OnTaskFail()
    {
        // Punishment logic
    }

    public override void OnTaskSuccess()
    {
        // Reward logic
    }

    void OnTaskEnd()
    {
        _ctx.TaskManager.SetPrinterUiState(false);
        DisableTriggerObj();

        if(IsSuccess)
            OnTaskSuccess();
        else
            OnTaskFail();

        _ctx.TaskManager.OnTaskEnd();

        _ctx.GameStateController.ChangeState(StateType.Gameplay);
    }

    public override void Tick()
    {

    }
}