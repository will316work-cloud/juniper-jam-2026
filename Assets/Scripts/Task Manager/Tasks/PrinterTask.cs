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
        OnTaskEnd();
    }

    public override void OnTaskSuccess()
    {
        // Reward logic
        OnTaskEnd();
    }

    void OnTaskEnd()
    {
        _ctx.TaskManager.SetPrinterUiState(false);
        DisableTriggerObj();
        _ctx.TaskManager.RestartTaskSystem();
        _ctx.PlayerControl.enabled = true;
    }

    public override void Tick()
    {

    }
}