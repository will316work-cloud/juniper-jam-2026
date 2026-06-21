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
    public override void OnTaskEnd()
    {
        _ctx.TaskManager.SetPrinterUiState(false);
        DisableTriggerObj();
    }

    public override void Tick()
    {

    }
}