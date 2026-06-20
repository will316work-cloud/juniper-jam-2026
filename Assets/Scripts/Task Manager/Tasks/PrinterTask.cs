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
        // _ctx.ChairManager.DeactivateMovement();
    }

    public override void OnTaskFail()
    {
        // _ctx.UiManager.HidePrinterUI();
        DisableTriggerObj();
    }

    public override void OnTaskSuccess()
    {
        // _ctx.UiManager.HidePrinterUI();
        _ctx.TaskManager.SetPrinterUiState(false);
        DisableTriggerObj();
        _ctx.TaskManager.RestartTaskSystem();
    }

    public override void Tick()
    {

    }
}